using FluentValidation;
using Platform.Domain;
using Platform.Validation.Domain;

namespace WhoWhat.Domain.Question.Commands
{
    public class VoteQuestionUp : Command
    {
        public string VoterId { get; set; }
    }

    public class VoteQuestionUpValidator : FluentCommandValidator<VoteQuestionUp>
    {
        public VoteQuestionUpValidator()
        {
            RuleFor(command => command.VoterId).NotEmpty();
        }
    }
}
