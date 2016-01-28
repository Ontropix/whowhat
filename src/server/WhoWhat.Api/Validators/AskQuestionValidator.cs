using ServiceStack.FluentValidation;
using WhoWhat.Api.Contract.Question;

namespace WhoWhat.Api.Validators
{
    public class AskQuestionValidator : AbstractValidator<AskQuestionRequest>
    {
        public AskQuestionValidator()
        {
            RuleFor(x => x.Body).NotEmpty();
            RuleFor(x => x.Bytes).NotEmpty();
        }
    }
}
