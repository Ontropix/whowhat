using System;
using System.Collections.Generic;
using FluentValidation;
using Platform.Domain;
using Platform.Validation;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class AskQuestion: Command
    {
        public string Body { get; set; }

        public string ImageUri { get; set; }

        public string ThumbnailUri { get; set; }

        public HashSet<string> Tags { get; set; }

        public string AuthorId { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class AskQuestionValidator : FluentCommandValidator<AskQuestion>
    {
        public AskQuestionValidator()
        {
            RuleFor(command => command.Body).NotEmpty().Length(QuestionRules.QuestionBodyLengthMin, QuestionRules.QuestionBodyLengthMax);
            RuleFor(command => command.ImageUri).NotEmpty().IsValidUri(UriKind.Absolute);
            RuleFor(command => command.ThumbnailUri).NotEmpty().IsValidUri(UriKind.Absolute);
            RuleFor(command => command.Tags).NotNull();
            RuleFor(command => command.AuthorId).NotEmpty();
            RuleFor(command => command.CreatedAt).NotFuture();
        }
    }
}
