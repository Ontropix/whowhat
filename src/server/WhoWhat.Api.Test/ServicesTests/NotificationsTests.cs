using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoWhat.Api.Contract.Answer;
using WhoWhat.Api.Contract.Payload;
using WhoWhat.Api.Contract.Question;
using WhoWhat.Api.Contract.User;
using WhoWhat.Domain.Notification.Data;

namespace WhoWhat.Api.Test
{
    [TestClass]
    public class NotificationsTests : ServiceTest
    {
        private List<Notification> GetNotifications(string userId, int count = Int32.MaxValue)
        {
            OpenSession(userId);
            UsersService usersService = AppHost.Container.Resolve<UsersService>();
            NotificationsResponse response = usersService.Get(new NotificationsRequest() { Take = count });
            return response.Notifications;
        }

        [TestMethod]
        public void WhenQuestionAnswered_ShouldNotificationBeCreated()
        {
            //Preparing users
            string questionAuthorId = IdGenerator.Generate();
            CreateInMemoryUser(questionAuthorId);

            string answerAuthorId = IdGenerator.Generate();
            CreateInMemoryUser(answerAuthorId);
            
            //Asking
            OpenSession(questionAuthorId);
            AskQuestionResponse askQuestionResponse = QuestionHelper.AskQuestion();

            //Answering
            OpenSession(answerAuthorId);
            AnswerStatusResponse answerResponse = QuestionHelper.AnswerQuestion(askQuestionResponse.QuestionId);

            List<Notification> notifications = GetNotifications(questionAuthorId);
            
            Assert.AreEqual(1, notifications.Count);
            Notification notification = notifications[0];
            Assert.AreEqual(NotificationType.QuestionAnswered, notification.Type);
            Assert.AreEqual(askQuestionResponse.QuestionId, notification.QuestionId);
        }
    }
}
