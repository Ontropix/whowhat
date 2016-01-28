using System;
using FluentValidation;
using Platform.Domain;
using Platform.Validation;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.User.Commands
{
    public class RegisterUser : Command
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ThirdPartyId { get; set; }
        public string AccessToken { get; set; }

        public string PhotoSmallUri { get; set; }
        public string PhotoBigUri { get; set; }

        public UserLoginType LoginType { get; set; }
    }

    public class RegisterUserValidator : FluentCommandValidator<RegisterUser>
    {
        public RegisterUserValidator()
        {
            RuleFor(command => command.FirstName).NotEmpty();
            RuleFor(command => command.LastName).NotEmpty();

            RuleFor(command => command.ThirdPartyId).NotEmpty();
            RuleFor(command => command.AccessToken).NotEmpty();

            RuleFor(command => command.PhotoSmallUri).NotEmpty().IsValidUri(UriKind.Absolute);
            RuleFor(command => command.PhotoBigUri).NotEmpty().IsValidUri(UriKind.Absolute);
        }
    }
}