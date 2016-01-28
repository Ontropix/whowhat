using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class ChangeAnswerBody: Command
    {
        public string AnswerId { get; set; }
        public string NewBody { get; set; }
    }

    public class ChangeAnswerBodyValidator : FluentCommandValidator<ChangeAnswerBody>
    {
        public ChangeAnswerBodyValidator()
        {
            RuleFor(command => command.AnswerId).NotEmpty();
            RuleFor(command => command.NewBody).NotEmpty().Length(QuestionRules.AnswerBodyLengthMin, QuestionRules.AnswerBodyLengthMax);
        }
    }
}
