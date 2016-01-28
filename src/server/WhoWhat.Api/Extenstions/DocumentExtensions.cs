using System.Linq;
using WhoWhat.Api.Contract.Payload;
using WhoWhat.Core.UserService;
using WhoWhat.View;
using WhoWhat.View.Documents;
using WhoWhat.View.Payload;

namespace WhoWhat.Api
{
    public static class DocumentExtensions
    {
        public static QuestionDto AsQuestion(this QuestionDocument document, UserCache userCache)
        {
            return new QuestionDto
            {
                QuestionId = document.Id,
                Body = document.Body,
                Tags = document.Tags,
                IsResolved = document.IsResolved,
                CreatedAt = document.CreatedAt,
                EditedAt = document.EditedAt,
                Thumbnail = document.ThumbnailUri,
                ImageUri = document.ImageUri,
                Rating = document.Rating,
                

                VotesCount = document.Votes.Count,
                AnswersCount = document.AnswersCount,
                
                Author = new Author()
                {
                    UserId = userCache.UserId,
                    FirstName = userCache.FirstName,
                    LastName = userCache.LastName,
                    AvatarUri = userCache.AvatarUri,
                    Rating = userCache.Reputation
                }
            };
        }

        public static AnswerDto AsAnswer(this AnswerPayload answerPayload, UserCache userCache)
        {
            return new AnswerDto()
            {
                AnswerId = answerPayload.Id,
                IsAccepted = answerPayload.IsAccepted,
                Body = answerPayload.Body,
                CreatedAt = answerPayload.CreatedAt,
                EditedAt = answerPayload.EditedAt,
                Rating = answerPayload.Rating,

                Author = new Author()
                {
                    UserId = userCache.UserId,
                    FirstName = userCache.FirstName,
                    LastName = userCache.LastName,
                    AvatarUri = userCache.AvatarUri,
                    Rating = userCache.Reputation
                },

                Votes = answerPayload.Votes.ToDictionary(x => x.Key, x => x.Value.Direction)
            };
        }

        public static Notification AsNotification(this NotificationDocument document)
        {
            return new Notification
            {
                Id = document.Id,
                ProducerUserId = document.ProducerUserId,
                TargetUserId = document.TargetUserId,
                Type = document.Type,

                QuestionId = document.QuestionId,
                QuestionBody = document.QuestionBody,
                QuestionThumbnailUri = document.QuestionThumbnailUri,
                
                CreatedAt = document.CreatedAt,

                RatingShift = document.RatingShift
            };
        }
    }
}
