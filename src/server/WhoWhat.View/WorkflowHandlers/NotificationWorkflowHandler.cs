using System;
using Platform.Dispatching;
using Platform.Domain;
using WhoWhat.Domain.Notification.Commands;
using WhoWhat.Domain.Notification.Data;
using WhoWhat.Domain.Question.Events;
using WhoWhat.View.Documents;

namespace WhoWhat.View.WorkflowHandlers
{
    public class NotificationWorkflowHandler : WorkflowHandler,
                                               IMessageHandler<QuestionAnswered>,
                                               IMessageHandler<QuestionVotedUp>,
                                               IMessageHandler<QuestionVotedDown>,
                                               IMessageHandler<AnswerAccepted>,
                                               IMessageHandler<AnswerVotedUp>,
                                               IMessageHandler<AnswerVotedDown>
    {
        public NotificationWorkflowHandler(ViewContext context, CommandBus commandBus, IEntityIdGenerator idGenerator)
            : base(context, commandBus, idGenerator)
        {
        }


        private void BuildCreateNotificationCommand(string producerId,
                                                    string targetId,
                                                    NotificationType type,
                                                    QuestionDocument question,
                                                    int ratingShift,
                                                    string senderId)
        {
            var command = new CreateNotification
            {
                AggregateId = _idGenerator.Generate(),
                ProducerUserId = producerId,
                TargetUserId = targetId,
                Type = type,

                QuestionId = question.Id,
                QuestionBody = question.Body,
                QuestionThumbnailUri = question.ThumbnailUri,

                RatingShift = ratingShift,

                CreatedAt = DateTime.Now
            };

            SendCommand(command, senderId);
        }

        public void Handle(QuestionAnswered message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            if (message.AuthorId != question.AuthorId)
            {
                BuildCreateNotificationCommand(producerId: message.AuthorId,
                                               targetId: question.AuthorId,
                                               type: NotificationType.QuestionAnswered,
                                               question: question,
                                               ratingShift: 0,
                                               senderId: message.Metadata.SenderId);
            }
        }

        public void Handle(QuestionVotedUp message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            BuildCreateNotificationCommand(message.VoterId,
                                           question.AuthorId,
                                           NotificationType.QuestionVotedUp,
                                           question,
                                           message.RatingShift,
                                           message.Metadata.SenderId);
        }

        public void Handle(QuestionVotedDown message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            BuildCreateNotificationCommand(message.VoterId,
                                           question.AuthorId,
                                           NotificationType.QuestionVotedDown,
                                           question,
                                           message.RatingShift,
                                           message.Metadata.SenderId);
        }

        public void Handle(AnswerAccepted message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            BuildCreateNotificationCommand(question.AuthorId,
                                           question.Answers[message.AnswerId].AuthorId,
                                           NotificationType.AnswerMarkedAsAccepted,
                                           question,
                                           -0,
                                           message.Metadata.SenderId);
        }

        public void Handle(AnswerVotedUp message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            BuildCreateNotificationCommand(message.VoterId,
                                           question.Answers[message.AnswerId].AuthorId,
                                           NotificationType.AnswerVotedUp,
                                           question,
                                           message.RatingShift,
                                           message.Metadata.SenderId);
        }

        public void Handle(AnswerVotedDown message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            BuildCreateNotificationCommand(message.VoterId,
                                           question.Answers[message.AnswerId].AuthorId,
                                           NotificationType.AnswerVotedDown,
                                           question,
                                           message.RatingShift,
                                           message.Metadata.SenderId);
        }
    }
}
