using System.Collections.Generic;
using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class ChangeQuestionInfo : Command
    {
        public string Body { get; set; }
        public HashSet<string> Tags { get; set; }
    }

    public class ChangeQuestionInfoValidator : FluentCommandValidator<ChangeQuestionInfo>
    {
        public ChangeQuestionInfoValidator()
        {
            RuleFor(command => command.Body).NotEmpty().Length(QuestionRules.QuestionBodyLengthMin, QuestionRules.QuestionBodyLengthMax);
            RuleFor(command => command.Tags).NotNull();
        }
    }
}