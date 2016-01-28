using System;
using Platform.Domain;
using Platform.Validation;
using Platform.Validation.Domain;
using WhoWhat.Domain.Notification.Data;
using FluentValidation;

namespace WhoWhat.Domain.Notification.Commands
{
    public class CreateNotification : Command
    {
        public string ProducerUserId { get; set; }
        public string TargetUserId { get; set; }
        public NotificationType Type { get; set; }

        public string QuestionId { get; set; }
        public string QuestionBody { get; set; }
        public string QuestionThumbnailUri { get; set; }

        public int RatingShift { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CreateNotificationValidator : FluentCommandValidator<CreateNotification>
    {
        public CreateNotificationValidator()
        {
            RuleFor(command => command.QuestionId).NotEmpty();
            RuleFor(command => command.QuestionBody).NotEmpty();
            RuleFor(command => command.QuestionThumbnailUri).NotEmpty().IsValidUri(UriKind.Absolute);

            RuleFor(command => command.ProducerUserId).NotEmpty();
            RuleFor(command => command.TargetUserId).NotEmpty();

            RuleFor(command => command.RatingShift).GreaterThan(0);
            RuleFor(command => command.CreatedAt).NotFuture();
        }
    }
}