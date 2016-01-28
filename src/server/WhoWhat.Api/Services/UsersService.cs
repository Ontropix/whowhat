using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;
using WhoWhat.Api.Cache;
using WhoWhat.Api.Contract.Payload;
using WhoWhat.Api.Contract.Question;
using WhoWhat.Api.Contract.User;
using WhoWhat.Core.UserService;
using WhoWhat.Domain.User;
using WhoWhat.Domain.User.Commands;
using WhoWhat.View.Documents;

namespace WhoWhat.Api
{
    [NoCache]
    [Authenticate]
    public class UsersService : CommandService
    {
        private const string Me = "me";

        public NotificationsResponse Get(NotificationsRequest request)
        {
            var notifications = ViewContext.Notifications
                                           .AsQueryable()
                                           .Where(x => x.TargetUserId == TypedSession.UserId)
                                           .OrderByDescending(x => x.CreatedAt)
                                           .Skip(request.Skip)
                                           .Take(request.Take)
                                           .ToList()
                                           .Distinct(new NotificationTypeComparer());

            //Update NotificationsCheckedAt
            UserCache user = UserCacheService.Get(TypedSession.UserId);
            user.NotificationsCheckedAt = DateTime.UtcNow;
            UserCacheService.Set(user.UserId, user);

            return new NotificationsResponse
            {
                Notifications = notifications.Select(x => x.AsNotification()).ToList(),
                LatestNotificationChecking = user.NotificationsCheckedAt
            };

        }

        public NotificationsCountResponse Get(NotificationsCountRequest request)
        {
            UserCache user = UserCacheService.Get(TypedSession.UserId);
            int count = ViewContext.Notifications.AsQueryable().Count(
                x => x.CreatedAt >= user.NotificationsCheckedAt &&
                x.TargetUserId == user.UserId);

            var response = new NotificationsCountResponse
            {
                Count = count
            };

            return response;
        }

        public UserProfileResponse Get(UserProfileRequest request)
        {
            UserDocument user = ViewContext.Users.GetById(request.UserId == Me ? TypedSession.UserId : request.UserId);

            if (user == null)
            {
                throw HttpError.NotFound("User with UserId = {0} does not exist".Fmt(request.UserId));
            }

            return new UserProfileResponse
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Rating = user.Reputation,
                PhotoBigUri = user.PhotoBigUri,
                PhotoSmallUri = user.PhotoSmallUri,

                QuestionsCount = ViewContext.Questions.AsQueryable().Count(x => x.AuthorId == user.Id),
                AnswersCount = ViewContext.Answers.AsQueryable().Count(x => x.AuthorId == user.Id),

                FbProfile = user.LoginType == UserLoginType.Facebook ? "https://www.facebook.com/" + user.ThirdPartyId : string.Empty,
                VkProfile = user.LoginType == UserLoginType.Vk ? "http://vk.com/id" + user.ThirdPartyId : string.Empty
            };
        }

        public QuestionSummariesResponse Get(UserAnswersRequest request)
        {
            string userId = request.UserId == Me ? TypedSession.UserId : request.UserId;

            List<AnswerDocument> userAnswers = ViewContext.Answers
                                                          .AsQueryable()
                                                          .Where(q => q.AuthorId == userId)
                                                          .Skip(request.Skip)
                                                          .Take(request.Take)
                                                          .OrderByDescending(q => q.CreatedAt)
                                                          .ToList();

            List<string> questionIds = userAnswers.Select(x => x.QuestionId).ToList();

            IEnumerable<QuestionDocument> questions = new List<QuestionDocument>();

            if (questionIds.Count != 0)
            {
                questions = ViewContext.Questions.GetById(questionIds);
            }

            UserCache user = UserCacheService.Get(TypedSession.UserId);
            return new QuestionSummariesResponse()
            {
                Questions = questions.Select(x => x.AsQuestion(user))
            };
        }

        public QuestionSummariesResponse Get(UserQuestionsRequest request)
        {
            string userId = request.UserId == Me ? TypedSession.UserId : request.UserId;
            UserCache user = UserCacheService.Get(userId);

            List<QuestionDto> userQuestions = ViewContext.Questions
                                                    .AsQueryable()
                                                    .Where(q => q.AuthorId == userId)
                                                    .Skip(request.Skip)
                                                    .Take(request.Take)
                                                    .OrderByDescending(q => q.CreatedAt)
                                                    .AsEnumerable()
                                                    .Select(q => q.AsQuestion(user))
                                                    .ToList();

            return new QuestionSummariesResponse
            {
                Questions = userQuestions
            };
        }

        public PushupsResponse Post(SubscribeToPushupsRequest request)
        {
            UserDocument user = ViewContext.Users.GetById(TypedSession.UserId);

            if (user.PushupsSettings.ContainsKey(request.DeviceId))
            { //Nothing has changed, simply return a response
                return new PushupsResponse();
            }

            var command = new SubscribeUserToPushups
            {
                AggregateId = TypedSession.UserId,
                DeviceId = request.DeviceId,
                DeviceOs = request.DeviceOs,
                Token = request.Token
            };

            SendCommand(command);

            return new PushupsResponse();
        }

        public PushupsResponse Post(UnsubscribeFromPushupsRequest request)
        {
            UserDocument user = ViewContext.Users.GetById(TypedSession.UserId);

            if (!user.PushupsSettings.ContainsKey(request.DeviceId))
            {
                return new PushupsResponse();
            }

            var command = new UnsubscribeUserFromPushups()
            {
                AggregateId = TypedSession.UserId,
                DeviceId = request.DeviceId,
            };

            SendCommand(command);

            return new PushupsResponse();
        }
    }
}
