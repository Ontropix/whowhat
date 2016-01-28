using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class VoteAnswerDown: Command
    {
        public string AnswerId { get; set; }
        public string VoterId { get; set; }
    }

    public class VoteAnswerDownValidator : FluentCommandValidator<VoteAnswerDown>
    {
        public VoteAnswerDownValidator()
        {
            RuleFor(command => command.AnswerId).NotEmpty();
            RuleFor(command => command.VoterId).NotEmpty();
        }
    }
}
