using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;
using StructureMap.Attributes;
using WhoWhat.Api.Cache;
using WhoWhat.Api.Contract.Answer;
using WhoWhat.Api.Contract.Payload;
using WhoWhat.Api.Contract.Question;
using WhoWhat.Core.Content;
using WhoWhat.Core.UserService;
using WhoWhat.Domain.Question.Commands;
using WhoWhat.View.Documents;

namespace WhoWhat.Api
{
    [NoCache]
    public class QuestionService : CommandService
    {
        private const string PopularQuestionsCacheKey = "popular_questions";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [SetterProperty]
        public IImageStorageService ImageStorageService { get; set; }

        #region Listing

        public object Get(PopularQuestionsRequest request)
        {
            object response = RequestContext.ToOptimizedResultUsingCache(
                cacheClient: Cache,
                cacheKey: PopularQuestionsCacheKey,
                expireCacheIn: new TimeSpan(1, 0, 0),
                factoryFn: () =>
                {
                    List<QuestionDocument> popularQuestions = GetPopularQuestions(request.Skip, request.Take);
                    IEnumerable<string> userIds = popularQuestions.Select(q => q.AuthorId).Distinct();
                    Dictionary<string, UserCache> usersMap = UserCacheService.Get(userIds).ToDictionary(userCache => userCache.UserId);

                    return new QuestionSummariesResponse
                    {
                        Questions = popularQuestions.Select(q => q.AsQuestion(usersMap[q.AuthorId]))
                    };
                });

            return response;
        }

        public QuestionSummariesResponse Get(RecentQuestionsRequest request)
        {
            List<QuestionDocument> recentQuestions = ViewContext.Questions
                                             .AsQueryable()
                                             .Skip(request.Skip)
                                             .Take(request.Take)
                                             .OrderByDescending(q => q.CreatedAt)
                                             .ToList();

            IEnumerable<string> userIds = recentQuestions.Select(q => q.AuthorId).Distinct();
            Dictionary<string, UserCache> usersMap = UserCacheService.Get(userIds).ToDictionary(userCache => userCache.UserId);

            return new QuestionSummariesResponse
            {
                Questions = recentQuestions.Select(q => q.AsQuestion(usersMap[q.AuthorId]))
            };
        }

        public QuestionSummariesResponse Get(UnansweredQuestionsRequest request)
        {
            List<QuestionDocument> unansweredQuestions = ViewContext.Questions
                                                 .AsQueryable()
                                                 .Where(x => x.IsResolved == false)
                                                 .Skip(request.Skip)
                                                 .Take(request.Take)
                                                 .OrderByDescending(q => q.CreatedAt)
                                                 .ToList();

            IEnumerable<string> userIds = unansweredQuestions.Select(q => q.AuthorId).Distinct();
            Dictionary<string, UserCache> usersMap = UserCacheService.Get(userIds).ToDictionary(userCache => userCache.UserId);

            return new QuestionSummariesResponse
            {
                Questions = unansweredQuestions.Select(q => q.AsQuestion(usersMap[q.AuthorId]))
            };
        }

        #endregion

        public QuestionDetailsResponse Get(QuestionDetailsRequest request)
        {
            QuestionDocument question = ViewContext.Questions.GetById(request.QuestionId);

            if (question == null)
            {
                throw HttpError.NotFound("Question with QuestionId = {0} does not exist".Fmt(request.QuestionId));
            }

            UserCache author = UserCacheService.Get(question.AuthorId);

            List<string> userIds = question.Answers.Values.Select(x => x.AuthorId).Distinct().ToList();

            Logger.Debug("Get.QuestionDetailsRequest.UserIds. " + userIds.Dump());

            // UserId -> User
            Dictionary<string, UserCache> answerAuthorsMap = UserCacheService.Get(userIds).ToDictionary(userCache => userCache.UserId);

            List<AnswerDto> answers =
                question.Answers.Values.Select(x =>
                {
                    UserCache userCache = answerAuthorsMap[x.AuthorId];
                    return x.AsAnswer(userCache);
                }).ToList();

            return new QuestionDetailsResponse()
            {
                Question = question.AsQuestion(author),
                Answers = answers,
                Votes = question.Votes.ToDictionary(x => x.Key, x => x.Value.Direction)
            };
        }

        [Authenticate]
        public AskQuestionResponse Post(AskQuestionRequest request)
        {
            byte[] bytes = Convert.FromBase64String(request.Bytes);
            var stream = new MemoryStream(bytes);

            Uri normal = SaveNormalImage(stream);
            Uri small = SaveSmallImage(stream);

            HashSet<string> tags = request.Tags.Select(tag => tag.ToLowerInvariant()).ToHashSet();
            string questionId = IdGenerator.Generate();

            var command = new AskQuestion
            {
                AggregateId = questionId,
                Body = request.Body,
                Tags = tags,
                AuthorId = TypedSession.UserId,
                ImageUri = normal.ToString(),
                ThumbnailUri = small.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            SendCommand(command);

            return new AskQuestionResponse()
            {
                QuestionId = questionId
            };
        }

        [Authenticate]
        public ChangeQuestionInfoResponse Post(ChangeQuestionInfoRequest request)
        {
            QuestionDocument question = ViewContext.Questions.GetById(request.QuestionId);

            if (question == null)
            {
                throw HttpError.NotFound("Question with QuestionId = {0} does not exist".Fmt(request.QuestionId));
            }

            // todo add validation

            var command = new ChangeQuestionInfo
            {
                AggregateId = request.QuestionId,
                Body = request.Body,
                Tags = request.Tags
            };

            SendCommand(command);

            return new ChangeQuestionInfoResponse()
            {
                QuestionId = request.QuestionId
            };
        }

        [Authenticate]
        public ChangeQuestionInfoResponse Post(ChangeQuestionImageRequest request)
        {
            var command = new ChangeQuestionImage()
            {
                AggregateId = request.QuestionId
            };

            if (request.Bytes == null)
            {
                throw new ArgumentException("image is null");
            }

            byte[] bytes = Convert.FromBase64String(request.Bytes);
            MemoryStream stream = new MemoryStream(bytes);

            Uri normal = SaveNormalImage(stream);
            Uri small = SaveSmallImage(stream);

            command.ImageUri = normal.ToString();
            command.ThumbnailUri = small.ToString();

            SendCommand(command);

            return new ChangeQuestionInfoResponse()
            {
                QuestionId = request.QuestionId
            };
        }

        [Authenticate]
        public VoteQuestionResponse Post(VoteQuestionUpRequest request)
        {
            QuestionDocument questionDocument = ViewContext.Questions.GetById(request.QuestionId);

            if (questionDocument == null)
            {
                throw HttpError.NotFound("Question with QuestionId = {0} does not exist".Fmt(request.QuestionId));
            }

            var command = new VoteQuestionUp
            {
                AggregateId = request.QuestionId,
                VoterId = TypedSession.UserId
            };

            SendCommand(command);

            questionDocument = ViewContext.Questions.GetById(request.QuestionId);

            return new VoteQuestionResponse
            {
                QuestionId = request.QuestionId,
                Rating = questionDocument.Rating
            };
        }

        [Authenticate]
        public VoteQuestionResponse Post(VoteQuestionDownRequest request)
        {
            QuestionDocument questionDocument = ViewContext.Questions.GetById(request.QuestionId);

            if (questionDocument == null)
            {
                throw HttpError.NotFound("Question with QuestionId = {0} does not exist".Fmt(request.QuestionId));
            }

            var command = new VoteQuestionDown()
            {
                AggregateId = request.QuestionId,
                VoterId = TypedSession.UserId
            };

            SendCommand(command);

            questionDocument = ViewContext.Questions.GetById(request.QuestionId);

            return new VoteQuestionResponse
            {
                QuestionId = request.QuestionId,
                Rating = questionDocument.Rating
            };
        }

        [Authenticate]
        public VoteQuestionResponse Post(UnvoteQuestionRequest request)
        {

            QuestionDocument questionDocument = ViewContext.Questions.GetById(request.QuestionId);

            if (questionDocument == null)
            {
                throw HttpError.NotFound("Question with QuestionId = {0} does not exist".Fmt(request.QuestionId));
            }

            var command = new UnvoteQuestion()
            {
                AggregateId = request.QuestionId,
                VoterId = TypedSession.UserId
            };

            SendCommand(command);

            questionDocument = ViewContext.Questions.GetById(request.QuestionId);

            return new VoteQuestionResponse
            {
                QuestionId = request.QuestionId,
                Rating = questionDocument.Rating
            };
        }

        [Authenticate]
        public AnswerStatusResponse Post(AnswerQuestionRequest request)
        {
            string answerId = IdGenerator.Generate();

            var command = new AnswerQuestion
            {
                AggregateId = request.QuestionId,
                Body = request.Body,
                AuthorId = TypedSession.UserId,
                AnswerId = answerId
            };

            SendCommand(command);

            return new AnswerStatusResponse()
            {
                AnswerId = answerId
            };
        }

        [Authenticate]
        public AnswerStatusResponse Post(RemoveQuestionRequest request)
        {
            var command = new RemoveQuestion()
            {
                AggregateId = request.QuestionId
            };

            SendCommand(command);

            return new AnswerStatusResponse()
            {
                AnswerId = request.QuestionId
            };
        }

        #region helpers

        private List<QuestionDocument> GetPopularQuestions(int skip, int take)
        {
            //Questions for the last month with rating > 3 and number of answers > 3
            List<QuestionDocument> popularQuestions = ViewContext.Questions.AsQueryable()
                                                                 .Where(q => q.CreatedAt > DateTime.UtcNow.AddDays(-30) &&
                                                                             q.AnswersCount >= 3 &&
                                                                             q.Rating >= 3)
                                                                 .Skip(skip)
                                                                 .Take(take)
                                                                 .OrderBy(q => q.Votes)
                                                                 .ToList();

            return popularQuestions;
        }

        private Uri SaveSmallImage(MemoryStream stream)
        {
            ScalledImage scaledSmall = ImageScaler.ScaleImage(stream, 100, 100);
            Uri small = ImageStorageService.SaveImage(scaledSmall.Stream, "small");
            return small;
        }

        private Uri SaveNormalImage(MemoryStream stream)
        {
            ScalledImage scaledNormal = ImageScaler.ScaleImage(stream, 600, 600);
            Uri normal = ImageStorageService.SaveImage(scaledNormal.Stream, "normal");
            return normal;
        }

        #endregion
    }
}
