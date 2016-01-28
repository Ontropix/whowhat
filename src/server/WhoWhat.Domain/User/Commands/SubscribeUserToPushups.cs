using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.User.Commands
{
    public class SubscribeUserToPushups : Command
    {
        public string DeviceId { get; set; }
        public DeviceOS DeviceOs { get; set; }
        public string Token { get; set; }
    }

    public class SubscribeUserToPushupsValidator : FluentCommandValidator<SubscribeUserToPushups>
    {
        public SubscribeUserToPushupsValidator()
        {
            RuleFor(command => command.DeviceId).NotEmpty();
            RuleFor(command => command.Token).NotEmpty();
        }
    }
}