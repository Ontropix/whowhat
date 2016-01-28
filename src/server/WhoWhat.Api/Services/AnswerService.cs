using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;
using WhoWhat.Api.Cache;
using WhoWhat.Api.Contract.Answer;
using WhoWhat.Domain.Question.Commands;
using WhoWhat.View.Documents;

namespace WhoWhat.Api
{
    [NoCache]
    [Authenticate]
    public class AnswerService : CommandService
    {
        #region Voting

        public VoteAnswerResponse Post(VoteAnswerUpRequest request)
        {
            string answerId = request.AnswerId;
            AnswerDocument answer = GetAnswerById(answerId);

            SendCommand(new VoteAnswerUp()
            {
                AggregateId = answer.QuestionId,
                AnswerId = answerId,
                VoterId = TypedSession.UserId
            });

            return new VoteAnswerResponse()
            {
                AnswerId = answerId,
                Rating = answer.Rating++
            };
        }

        public VoteAnswerResponse Post(VoteAnswerDownRequest request)
        {
            string answerId = request.AnswerId;
            AnswerDocument answer = GetAnswerById(answerId);

            SendCommand(new VoteAnswerDown()
            {
                AggregateId = answer.QuestionId,
                AnswerId = answerId,
                VoterId = TypedSession.UserId
            });

            return new VoteAnswerResponse()
            {
                AnswerId = answerId,
                Rating = answer.Rating--
            };
        }

        public VoteAnswerResponse Post(UnvoteAnswerRequest request)
        {
            string answerId = request.AnswerId;
            AnswerDocument answer = GetAnswerById(answerId);

            SendCommand(new UnvoteAnswer()
            {
                AggregateId = answer.QuestionId,
                AnswerId = answerId,
                VoterId = TypedSession.UserId
            });

            return new VoteAnswerResponse()
            {
                AnswerId = answerId,
                Rating = answer.Rating
            };
        }

        #endregion

        public AnswerStatusResponse Post(ChangeAnswerRequest request)
        {
            string answerId = request.AnswerId;

            AnswerDocument answer = GetAnswerById(request.AnswerId);

            if (answer.AuthorId != TypedSession.UserId)
            {
                throw new InvalidOperationException("User is not able to change questions of other users");
            }

            if ((DateTime.UtcNow - answer.CreatedAt) > TimeSpan.FromMinutes(15))
            {
                throw new InvalidOperationException("The user can only change the answer created within 15 minutes");
            }

            SendCommand(new ChangeAnswerBody()
            {
                AggregateId = answer.QuestionId,
                AnswerId = answerId,
                NewBody = request.Body
            });

            return new AnswerStatusResponse()
            {
                AnswerId = answerId
            };
        }

        public AnswerStatusResponse Post(RemoveAnswerRequest request)
        {
            string answerId = request.AnswerId;

            AnswerDocument answer = GetAnswerById(request.AnswerId);

            if (answer.AuthorId != TypedSession.UserId)
            {
                throw new InvalidOperationException("User is not able to remove questions of other users");
            }

            SendCommand(new RemoveAnswer()
            {
                AggregateId = answer.QuestionId,
                AnswerId = answerId
            });

            return new AnswerStatusResponse()
            {
                AnswerId = answerId
            };
        }

        public AnswerStatusResponse Post(AcceptAnswerRequest request)
        {
            string answerId = request.AnswerId;

            AnswerDocument answer = GetAnswerById(request.AnswerId);

            AcceptAnswer command = new AcceptAnswer
            {
                AggregateId = answer.QuestionId,
                AnswerId = answerId
            };

            SendCommand(command);

            return new AnswerStatusResponse()
            {
                AnswerId = answerId
            };
        }

        public AnswerStatusResponse Post(UnacceptAnswerRequest request)
        {
            string answerId = request.AnswerId;

            AnswerDocument answer = GetAnswerById(request.AnswerId);

            UnacceptAnswer command = new UnacceptAnswer
            {
                AggregateId = answer.QuestionId,
                AnswerId = answerId
            };

            SendCommand(command);

            return new AnswerStatusResponse()
            {
                AnswerId = answerId
            };
        }
        

        private AnswerDocument GetAnswerById(string answerId)
        {
            AnswerDocument answer = ViewContext.Answers.GetById(answerId);

            if (answer == null)
            {
                throw HttpError.NotFound("Answer with AnswerId = {0} does not exist".Fmt(answerId));
            }

            return answer;
        }
    }
}
