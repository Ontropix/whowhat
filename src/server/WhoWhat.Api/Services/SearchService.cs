using System.Collections.Generic;
using System.Linq;
using WhoWhat.Api.Cache;
using WhoWhat.Api.Contract.Question;
using WhoWhat.Core.UserService;

namespace WhoWhat.Api
{
    [NoCache]
    public class SearchService : CommandService
    {
        public QuestionSummariesResponse Get(SearchQuestionByTagRequest request)
        {
            string tag = request.Tag.ToLowerInvariant().Trim();

            var questions = ViewContext.Questions
                                       .AsQueryable()
                                       .Skip(request.Skip)
                                       .Take(request.Take)
                                       .Where(x => x.Tags.Contains(tag))
                                       .OrderByDescending(q => q.CreatedAt)
                                       .ToList();


            IEnumerable<string> userIds = questions.Select(q => q.AuthorId).Distinct();
            Dictionary<string, UserCache> usersMap = UserCacheService.Get(userIds).ToDictionary(userCache => userCache.UserId);

            return new QuestionSummariesResponse
            {
                Questions = questions.Select(q => q.AsQuestion(usersMap[q.AuthorId]))
            };
        }

        public QuestionSummariesResponse Get(SearchQuestionByKeywordRequest request)
        {
            string keyword = request.Keyword.Trim();

            var questions = ViewContext.Questions
                                       .AsQueryable()
                                       .Skip(request.Skip)
                                       .Take(request.Take)
                                       .Where(x => x.Body.Contains(keyword))
                                       .OrderByDescending(q => q.CreatedAt)
                                       .ToList();

            IEnumerable<string> userIds = questions.Select(q => q.AuthorId).Distinct();
            Dictionary<string, UserCache> usersMap = UserCacheService.Get(userIds).ToDictionary(userCache => userCache.UserId);

            return new QuestionSummariesResponse
            {
                Questions = questions.Select(q => q.AsQuestion(usersMap[q.AuthorId]))
            };
        }
    }
}
