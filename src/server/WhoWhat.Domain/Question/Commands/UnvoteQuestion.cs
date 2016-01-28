using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class UnvoteQuestion : Command
    {
        public string VoterId { get; set; }
    }

    public class UnvoteQuestionValidator : FluentCommandValidator<UnvoteQuestion>
    {
        public UnvoteQuestionValidator()
        {
            RuleFor(command => command.VoterId).NotEmpty();
        }
    }
}