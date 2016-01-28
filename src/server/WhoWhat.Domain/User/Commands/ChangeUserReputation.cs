using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.User.Commands
{
    public class ChangeUserReputation : Command
    {
        public int Shift { get; set; }
    }

    public class ChangeUserReputationValidator : FluentCommandValidator<ChangeUserReputation>
    {
    }
}