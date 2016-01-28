using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class AcceptAnswer : Command
    {
        public string AnswerId { get; set; }
    }

    public class AcceptAnswerValidator : FluentCommandValidator<AcceptAnswer>
    {
        public AcceptAnswerValidator()
        {
            RuleFor(command => command.AnswerId).NotEmpty();
        }
    }
}