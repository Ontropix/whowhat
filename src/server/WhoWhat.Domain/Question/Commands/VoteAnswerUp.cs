using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class VoteAnswerUp: Command
    {
        public string AnswerId { get; set; }
        public string VoterId { get; set; }
    }

    public class VoteAnswerUpValidator : FluentCommandValidator<VoteAnswerUp>
    {
        public VoteAnswerUpValidator()
        {
            RuleFor(command => command.AnswerId).NotEmpty();
            RuleFor(command => command.VoterId).NotEmpty();
        }
    }
}
