using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class VoteQuestionDown : Command
    {
        public string VoterId { get; set; }
    }

    public class VoteQuestionDownValidator : FluentCommandValidator<VoteQuestionDown>
    {
        public VoteQuestionDownValidator()
        {
            RuleFor(command => command.VoterId).NotEmpty();
        }
    }
}
