using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Text;
using WhoWhat.Api.Contract.Payload;
using WhoWhat.Api.Contract.Question;
using WhoWhat.Api.Contract.User;
using WhoWhat.Domain.Notification.Data;
using WhoWhat.Domain.User;

namespace WhoWhat.Api.Test
{
    [TestClass]
    [Ignore]
    public class UsersServiceTest : ServiceTest
    {
        [TestMethod]
        public void UserProfile_ShouldReturnUsersData()
        {
            const string userId = "abcde12345";

            CreateInMemoryUser(userId);

            UsersService service = AppHost.Container.Resolve<UsersService>();

            UserProfileResponse response = service.Get(new UserProfileRequest()
            {
                UserId = userId
            });

            Assert.IsNotNull(response, "User response should not be null");
            Assert.AreEqual(userId, response.UserId);
        }

        [TestMethod]
        public void WhenQuestionAnswered_ShouldNotificationBeCreated()
        {
            QuestionHelper questionHelper = new QuestionHelper(AppHost.Container);

            //Arrange
            string questionOwnerUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(questionOwnerUserId);

            string answerOwnerUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(answerOwnerUserId);

            //First user asks question
            OpenSession(questionOwnerUserId); 

            AskQuestionResponse askQuestionResponse = questionHelper.AskQuestion();

            //Second user answers the quetion question
            OpenSession(answerOwnerUserId);

            questionHelper.AnswerQuestion(askQuestionResponse.QuestionId);

            OpenSession(questionOwnerUserId);

            //The first user shoud get a notification that the question has been answered
            UsersService usersService = AppHost.Container.Resolve<UsersService>();

            NotificationsResponse response = usersService.Get(new NotificationsRequest() { Take = 20 });

            string votedUserUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(votedUserUserId);
            OpenSession(votedUserUserId);

            QuestionService questionService = AppHost.Container.Resolve<QuestionService>();

            OpenSession(votedUserUserId);
            //Vote up
            questionService.Post(new VoteQuestionUpRequest()
            {
                QuestionId = askQuestionResponse.QuestionId
            });

            //Vote down
            questionService.Post(new VoteQuestionDownRequest()
            {
                QuestionId = askQuestionResponse.QuestionId
            });

            questionService.Post(new VoteQuestionUpRequest()
            {
                QuestionId = askQuestionResponse.QuestionId
            });

            OpenSession(questionOwnerUserId);
            NotificationsResponse response2 = usersService.Get(new NotificationsRequest() { Take = 20});


            //Assertions
            Assert.IsNotNull(response);
            Console.WriteLine(response.Dump());
            Assert.AreEqual(1, response.Notifications.Count);

            Notification notification = response.Notifications[0];
            Assert.AreEqual(NotificationType.QuestionAnswered, notification.Type);
            Assert.AreEqual(askQuestionResponse.QuestionId, notification.QuestionId);

        }

        [TestMethod]
        public void WhenSubscriptionExists_AnsweringQuestion_ShouldInvokePusher()
        {
            QuestionHelper questionHelper = new QuestionHelper(AppHost.Container);

            //Arrange
            string questionOwnerUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(questionOwnerUserId);

            string answerOwnerUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(answerOwnerUserId);

            //First user asks question
            SetSession(new CustomUserSession()
            {
                UserId = questionOwnerUserId
            });

            AskQuestionResponse askQuestionResponse = questionHelper.AskQuestion();

            //Subscribe to notifications
            UsersService usersService = AppHost.Container.Resolve<UsersService>();
            usersService.Post(new SubscribeToPushupsRequest()
            {
                DeviceId = Guid.NewGuid().ToString(),
                DeviceOs = DeviceOS.WP,
                Token = "http://token.test"
            });

            //Second user answers the quetion question
            SetSession(new CustomUserSession()
            {
                UserId = answerOwnerUserId
            });
            questionHelper.AnswerQuestion(askQuestionResponse.QuestionId);

            Assert.IsNotNull(this.Pusher.WpToast);
            Console.WriteLine(this.Pusher.WpToast);
        }
    }
}
