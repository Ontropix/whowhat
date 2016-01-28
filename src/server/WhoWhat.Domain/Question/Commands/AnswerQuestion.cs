using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class AnswerQuestion : Command
    {
        public string AuthorId { get; set; }
        public string Body { get; set; }
        public string AnswerId { get; set; }
    }

    public class AnswerQuestionValidator : FluentCommandValidator<AnswerQuestion>
    {
        public AnswerQuestionValidator()
        {
            RuleFor(command => command.AnswerId).NotEmpty();
            RuleFor(command => command.AuthorId).NotEmpty();
            RuleFor(command => command.Body).NotEmpty().Length(QuestionRules.AnswerBodyLengthMin, QuestionRules.AnswerBodyLengthMax);
        }
    }
}
