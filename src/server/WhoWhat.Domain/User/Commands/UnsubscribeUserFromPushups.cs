using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.User.Commands
{
    public class UnsubscribeUserFromPushups : Command
    {
        public string DeviceId { get; set; }
    }

    public class UnsubscribeUserFromPushupsValidator : FluentCommandValidator<SubscribeUserToPushups>
    {
        public UnsubscribeUserFromPushupsValidator()
        {
            RuleFor(command => command.DeviceId).NotEmpty();
        }
    }
}