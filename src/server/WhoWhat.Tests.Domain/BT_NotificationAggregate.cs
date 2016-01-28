using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoWhat.Domain.Notification;
using WhoWhat.Domain.Notification.Commands;
using WhoWhat.Domain.Notification.Data;
using WhoWhat.Domain.Notification.Events;

namespace WhoWhat.Tests.Domain
{
    [TestClass]
    public class BT_NotificationAggregate : BehaviourTestBase<NotificationAggregate>
    {
        [TestMethod]
        public void WhenCommand_CreateNotification_ShouldBeEvent_NotificationCreated()
        {
            string notificationId = _idGenerator.Generate();
            string questionid = _idGenerator.Generate();
            string targetUserId = _idGenerator.Generate();
            string producerId = _idGenerator.Generate();
            DateTime createdAt = DateTime.Now;

            Given();

            When(new CreateNotification()
            {
                AggregateId = notificationId,

                QuestionId = questionid,
                QuestionBody = "Question Body!",
                QuestionThumbnailUri = "http://thubnain.com/1.jpg",

                ProducerUserId = producerId,
                TargetUserId = targetUserId,

                RatingShift = 111,
                CreatedAt = createdAt,
                Type = NotificationType.QuestionAnswered
                
            }, (aggregate, command) => aggregate.When(command));

            Expected(new NotificationCreated()
            {
                AggregateId = notificationId,

                QuestionId = questionid,
                QuestionBody = "Question Body!",
                QuestionThumbnailUri = "http://thubnain.com/1.jpg",

                ProducerUserId = producerId,
                TargetUserId = targetUserId,

                RatingShift = 111,
                CreatedAt = createdAt,
                Type = NotificationType.QuestionAnswered
            });
        }

        [TestMethod]
        public void WhenCommand_UpdateUserRegistration_ShouldBeEvent_UserRegistrationUpdated()
        {
            string notificationId = _idGenerator.Generate();
            string questionid = _idGenerator.Generate();
            string targetUserId = _idGenerator.Generate();
            string producerId = _idGenerator.Generate();
            DateTime createdAt = DateTime.Now;

            Given(new NotificationCreated()
            {
                AggregateId = notificationId,

                QuestionId = questionid,
                QuestionBody = "Question Body!",
                QuestionThumbnailUri = "http://thubnain.com/1.jpg",

                ProducerUserId = producerId,
                TargetUserId = targetUserId,

                RatingShift = 111,
                CreatedAt = createdAt,
                Type = NotificationType.QuestionAnswered
            });

            When(new RemoveNotification()
            {
                AggregateId = notificationId
            }, (aggregate, command) => aggregate.When(command));

            Expected(new NotificationRemoved()
            {
                AggregateId = notificationId
            });
        }
    }
}
