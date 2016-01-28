using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class UnacceptAnswer : Command
    {
        public string AnswerId { get; set; }
    }

    public class UnacceptAnswerValidator : FluentCommandValidator<UnacceptAnswer>
    {
        public UnacceptAnswerValidator()
        {
            RuleFor(command => command.AnswerId).NotEmpty();
        }
    }
}