using ServiceStack.FluentValidation;
using WhoWhat.Api.Contract.Question;

namespace WhoWhat.Api.Validators
{
    public class SearchQuestionByKeywordValidator : AbstractValidator<SearchQuestionByKeywordRequest>
    {
        public SearchQuestionByKeywordValidator()
        {
            RuleFor(x => x.Keyword).NotEmpty();
            RuleFor(x => x.Take).LessThanOrEqualTo(100);
        }
    }
}
