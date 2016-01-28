using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class RemoveAnswer : Command
    {
        public string AnswerId { get; set; }
    }

    public class RemoveAnswerCommandValidator : FluentCommandValidator<RemoveAnswer>
    {
        public RemoveAnswerCommandValidator()
        {
            RuleFor(command => command.AnswerId).NotEmpty();
        }
    }
}
