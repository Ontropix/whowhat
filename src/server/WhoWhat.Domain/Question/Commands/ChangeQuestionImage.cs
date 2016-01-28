using System;
using FluentValidation;
using Platform.Domain;
using Platform.Validation;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class ChangeQuestionImage : Command
    {
        public string ImageUri { get; set; }
        public string ThumbnailUri { get; set; }
    }

    public class ChangeQuestionImageValidator : FluentCommandValidator<ChangeQuestionImage>
    {
        public ChangeQuestionImageValidator()
        {
            RuleFor(command => command.ImageUri).NotEmpty().IsValidUri(UriKind.Absolute);
            RuleFor(command => command.ThumbnailUri).NotEmpty().IsValidUri(UriKind.Absolute);
        }
    }
}