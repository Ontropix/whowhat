using System;
using Platform.Domain;
using WhoWhat.Domain.Notification.Commands;
using WhoWhat.Domain.Notification.Data;
using WhoWhat.Domain.Question.Events;
using WhoWhat.Domain.WorkflowHandlers;
using WhoWhat.View.Documents;

namespace WhoWhat.View.WorkflowHandlers
{
    public class NotificationWorkflowHandler : WorkflowHandler, IMessageHandler<QuestionAnswered>,
        IMessageHandler<QuestionVotedUp>,
        IMessageHandler<QuestionVotedDown>,
        IMessageHandler<AnswerMarkedAsAccepted>,
        IMessageHandler<AnswerVotedUp>,
        IMessageHandler<AnswerVotedDown>
    {
        public NotificationWorkflowHandler(ReadOnlyViewContext context, ICommandBus commandBus, IEntityIdGenerator idGenerator)
            : base(context, commandBus, idGenerator)
        {
        }

        private CreateNotification BuildCreateNotificationCommand(string producerId, string targetId, NotificationType type, QuestionDocument question, int ratingShift)
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

            return command;
        }

        public void Handle(QuestionAnswered message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            CreateNotification command = BuildCreateNotificationCommand(message.AuthorId,
                question.Author.UserId,
                NotificationType.QuestionAnswered,
                question, 0);
            SendCommand(command, message.Metadata.SenderId);
        }

        public void Handle(QuestionVotedUp message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            CreateNotification command = BuildCreateNotificationCommand(message.VoterId,
                question.Author.UserId,
                NotificationType.QuestionVotedUp,
                question,
                ScoreTable.QuestionVotedUp);
            SendCommand(command, message.Metadata.SenderId);
        }

        public void Handle(QuestionVotedDown message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            CreateNotification command = BuildCreateNotificationCommand(message.VoterId,
                question.Author.UserId,
                NotificationType.QuestionVotedDown,
                question,
                ScoreTable.QuestionVotedDown);
            SendCommand(command, message.Metadata.SenderId);
        }

        public void Handle(AnswerMarkedAsAccepted message)
        {
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);
            AnswerDocument answer = _context.Answers.GetById(message.AnswerId);

            CreateNotification command = BuildCreateNotificationCommand(question.Author.UserId,
                answer.Author.UserId,
                NotificationType.AnswerMarkedAsAccepted,
                question,
                ScoreTable.AnswerMarkedAsAccepted);
            SendCommand(command, message.Metadata.SenderId);
        }

        public void Handle(AnswerVotedUp message)
        {
            AnswerDocument answer = _context.Answers.GetById(message.AnswerId);
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            CreateNotification command = BuildCreateNotificationCommand(message.VoterId,
                answer.Author.UserId,
                NotificationType.QuestionVotedDown,
                question,
                ScoreTable.AnswerVotedUp);

            SendCommand(command, message.Metadata.SenderId);
        }

        public void Handle(AnswerVotedDown message)
        {
            AnswerDocument answer = _context.Answers.GetById(message.AnswerId);
            QuestionDocument question = _context.Questions.GetById(message.AggregateId);

            CreateNotification command = BuildCreateNotificationCommand(message.VoterId,
                answer.Author.UserId,
                NotificationType.QuestionVotedDown,
                question,
                ScoreTable.AnswerVotedDown);

            SendCommand(command, message.Metadata.SenderId);
        }
    }
}
