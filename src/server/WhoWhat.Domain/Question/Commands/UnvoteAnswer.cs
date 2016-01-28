using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class UnvoteAnswer: Command
    {
        public string VoterId { get; set; }
        public string AnswerId { get; set; }
    }

    public class UnvoteAnswerValidator : FluentCommandValidator<UnvoteAnswer>
    {
        public UnvoteAnswerValidator()
        {
            RuleFor(command => command.VoterId).NotEmpty();
            RuleFor(command => command.AnswerId).NotEmpty();
        }
    }
}
