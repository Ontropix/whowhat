using ServiceStack.FluentValidation;
using WhoWhat.Api.Contract.Question;

namespace WhoWhat.Api.Validators
{
    public class SearchQuestionByTagValidator : AbstractValidator<SearchQuestionByTagRequest>
    {
        public SearchQuestionByTagValidator()
        {
            RuleFor(x => x.Tag).NotEmpty();
            RuleFor(x => x.Take).LessThanOrEqualTo(100);
        }
    }
}
