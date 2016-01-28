using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoWhat.Domain.Question;
using WhoWhat.Domain.Question.Events;
using WhoWhat.Domain.User;
using WhoWhat.View.Documents;
using WhoWhat.View.Payload;

namespace WhoWhat.Tests.View
{
    [TestClass]
    public class BT_QuestionDocument : BehaviourTestBase<QuestionDocument>
    {
        #region Predefined Events

        private UserDocument CreateUser(string userId)
        {
            return new UserDocument()
            {
                Id = userId,

                FirstName = "Tolik",
                LastName = "Anabolik",

                PhotoBigUri = "http://code9.biz/profile/image123_big.png",
                PhotoSmallUri = "http://code9.biz/profile/image123_small.png",

                ThirdPartyId = IdGenerator.Generate(),
                AccessToken = IdGenerator.Generate(),
                LoginType = UserLoginType.Vk,
                Role = AccessRole.User,
                Reputation = 0,
                PushupsSettings = null
            };
        }

        private QuestionDocument CreateVirginQuestion(string questionId)
        {
            return new QuestionDocument()
            {
                Id = questionId,
                AuthorId = IdGenerator.Generate(),

                Body = "It is question, isn't it?",

                ImageUri = "http://code9.biz/img/image123.png",
                ThumbnailUri = "http://code9.biz/thubm/image123.png",

                CreatedAt = DateTime.Now,
                Rating = 0,
                VotesCount = 0,
                AnswersCount = 0,

                IsClosed = false,
                IsResolved = false,

                Tags = new HashSet<string>()
                {
                    "cool",
                    "greatman",
                    "interesting"
                }
            };
        }

        private QuestionAsked QuestionAskedEvent(string questionId, string authorId = null)
        {
            return new QuestionAsked
            {
                AggregateId = questionId,
                AuthorId = authorId ?? IdGenerator.Generate(),

                Body = "It is question, isn't it?",

                ImageUri = "http://code9.biz/img/image123.png",
                ThumbnailUri = "http://code9.biz/thubm/image123.png",

                Tags = new HashSet<string>()
                {
                    "cool",
                    "greatman",
                    "interesting"
                }
            };
        }

        private QuestionAnswered QuestionAnsweredEvent(string questionId, string answerId, string answerAuthorId = null)
        {
            return new QuestionAnswered()
            {
                AggregateId = questionId,

                AnswerId = answerId,
                AuthorId = answerAuthorId ?? IdGenerator.Generate(),

                Body = "Answer Body!"
            };
        }

        #endregion

        [TestMethod]
        public void When_QuestionAsked_Should_Be()
        {
            string questionId = IdGenerator.Generate();
            UserDocument user = CreateUser(IdGenerator.Generate());

            Given(user);

            When(QuestionAskedEvent(questionId, user.Id), null);

            Expected(new QuestionDocument()
            {
                Id = questionId,
                AuthorId = user.Id,

                Body = "It is question, isn't it?",

                ImageUri = "http://code9.biz/img/image123.png",
                ThumbnailUri = "http://code9.biz/thubm/image123.png",

                Tags = new HashSet<string>()
                {
                    "cool",
                    "greatman",
                    "interesting"
                }
            });
        }

        [TestMethod]
        public void When_QuestionInfoChanged_Should_Be()
        {
            string questionId = IdGenerator.Generate();

            QuestionDocument question = CreateVirginQuestion(questionId);
            Given(question);

            const string newBody = "New body";
            DateTime editedDate = DateTime.Now;

            When(new QuestionInfoChanged()
            {
                AggregateId = questionId,
                Body = newBody,
                Tags = new HashSet<string>() { "NEWTAG" },
            }, null);

            Expected(questionId, q => q.Body == newBody);
            Expected(questionId, q => q.Tags.Count == 1);
            Expected(questionId, q => q.Tags.Contains("NEWTAG"));
            Expected(questionId, q => q.EditedAt == editedDate);
        }

        [TestMethod]
        public void When_QuestionImageChanged_Should_Be()
        {
            string questionId = IdGenerator.Generate();
            QuestionDocument question = CreateVirginQuestion(questionId);
            Given(question);

            const string imageUri = "http://newimage.uri";
            DateTime editedDate = DateTime.Now;

            When(new QuestionImageChanged()
            {
                AggregateId = questionId,
                ImageUri = imageUri,
                ThumbnailUri = imageUri,
            }, null);

            Expected(questionId, q => q.ImageUri == imageUri);
            Expected(questionId, q => q.ThumbnailUri == imageUri);
            Expected(questionId, q => q.EditedAt == editedDate);
        }

        [TestMethod]
        public void When_QuestionAnswered_Should_Be()
        {
            string questionId = IdGenerator.Generate();
            string answerAuthorId = IdGenerator.Generate();
            string answerId = IdGenerator.Generate();
            DateTime answerCreatedAt = DateTime.Now;

            UserDocument user = CreateUser(answerAuthorId);
            QuestionDocument question = CreateVirginQuestion(questionId);

            Given(user);
            Given(question);

            When(new QuestionAnswered()
            {
                AggregateId = questionId,
                AuthorId = answerAuthorId,
                AnswerId = answerId,
                Body = "This is new answer",
            }, null);

            Expected(questionId, q => q.AnswersCount == 1);
            Expected(questionId, q => q.Answers[answerId].Body == "This is new answer" && q.Answers[answerId].CreatedAt >= answerCreatedAt);
        }

        [TestMethod]
        public void When_QuestionRemoved_Should_Be()
        {
            string questionId = IdGenerator.Generate();

            QuestionDocument question = CreateVirginQuestion(questionId);

            Given(question);

            When(new QuestionRemoved() { AggregateId = questionId }, null);

            Expected(questionId, q => q == null);
        }

        [TestMethod]
        public void When_QuestionClosed_Should_Be()
        {
            string questionId = IdGenerator.Generate();
            QuestionDocument question = CreateVirginQuestion(questionId);

            Given(question);

            When(new QuestionClosed() { AggregateId = questionId }, null);

            Expected(questionId, q => q.IsClosed);
        }

        [TestMethod]
        public void When_QuestionVotedUp_Should_Be()
        {
            string questionId = IdGenerator.Generate();
            string voterId = IdGenerator.Generate();

            QuestionDocument question = CreateVirginQuestion(questionId);
            Given(question);

            When(new QuestionVotedUp()
            {
                AggregateId = questionId,
                VoterId = voterId,
                RatingShift = QuestionScoreTable.QuestionVotedUp
            }, null);

            Expected(questionId, q => q.Rating == QuestionScoreTable.QuestionVotedUp);
            Expected(questionId, q => q.VotesCount == 1);
            Expected(questionId, q => q.Votes[voterId].Direction == VoteDirection.Up);
            Expected(questionId, q => q.Votes[voterId].RatingShift == QuestionScoreTable.QuestionVotedUp);
        }

        [TestMethod]
        public void When_QuestionVotedDown_Should_Be()
        {
            string questionId = IdGenerator.Generate();
            string voterId = IdGenerator.Generate();
            QuestionDocument question = CreateVirginQuestion(questionId);

            Given(question);

            When(new QuestionVotedDown()
            {
                AggregateId = questionId,
                VoterId = voterId,
                RatingShift = QuestionScoreTable.QuestionVotedDown
            }, null);

            Expected(questionId, q => q.Rating == QuestionScoreTable.QuestionVotedDown);
            Expected(questionId, q => q.VotesCount == 1);
            Expected(questionId, q => q.Votes[voterId].Direction == VoteDirection.Down);
            Expected(questionId, q => q.Votes[voterId].RatingShift == QuestionScoreTable.QuestionVotedDown);
        }

        [TestMethod]
        public void When_QuestionUnvoted_Should_Be()
        {
            string questionId = IdGenerator.Generate();
            string voterId = IdGenerator.Generate();

            QuestionDocument question = CreateVirginQuestion(questionId);
            question.Votes.Add(voterId, new VotePayload(VoteDirection.Up, QuestionScoreTable.QuestionVotedUp));
            question.VotesCount++;
            question.Rating += QuestionScoreTable.QuestionVotedUp;

            Given(question);

            When(new QuestionUnvoted()
            {
                AggregateId = questionId,
                VoterId = voterId,
            }, null);

            Expected(questionId, q => q.Rating == 0);
            Expected(questionId, q => q.VotesCount == 0);
            Expected(questionId, q => !q.Votes.ContainsKey(voterId));
        }

        [TestMethod]
        public void When_AnswerVotedUp_Should_Be()
        {
            string questionId = IdGenerator.Generate();
            string answerId = IdGenerator.Generate();
            string voterId = IdGenerator.Generate();

            QuestionDocument question = CreateVirginQuestion(questionId);
            question.Answers.Add(answerId, new AnswerPayload()
            {
                Id = answerId,
                Body = "Answer body",
                Rating = 0
            });
            question.AnswersCount = 1;

            Given(question);

            Given(new AnswerDocument
            {
                Id = answerId,
                Body = "Answer body",
                Rating = 0
            });

            When(new AnswerVotedUp()
            {
                AggregateId = questionId,
                AnswerId = answerId,
                VoterId = voterId,
                RatingShift = QuestionScoreTable.AnswerVotedUp
            }, null);

            Expected(questionId, q => q.AnswersCount == 1);
            Expected(questionId, q => q.Answers.ContainsKey(answerId));
            Expected(questionId, q => q.Answers[answerId].Rating == QuestionScoreTable.AnswerVotedUp);
            Expected(questionId, q => q.Answers[answerId].Votes.ContainsKey(voterId));
        }

        [TestMethod]
        public void When_AnswerVotedDown_Should_Be()
        {
            string questionId = IdGenerator.Generate();
            string answerId = IdGenerator.Generate();
            string voterId = IdGenerator.Generate();

            QuestionDocument question = CreateVirginQuestion(questionId);
            question.Answers.Add(answerId, new AnswerPayload()
            {
                Id = answerId,
                Body = "Answer body",
                Rating = 0
            });
            question.AnswersCount = 1;

            Given(question);

            Given(new AnswerDocument
            {
                Id = answerId,
                Body = "Answer body",
                Rating = 0
            });

            When(new AnswerVotedDown()
            {
                AggregateId = questionId,
                AnswerId = answerId,
                VoterId = voterId,
                RatingShift = QuestionScoreTable.AnswerVotedDown
            }, null);

            Expected(questionId, q => q.AnswersCount == 1);
            Expected(questionId, q => q.Answers.ContainsKey(answerId));
            Expected(questionId, q => q.Answers[answerId].Rating == QuestionScoreTable.AnswerVotedDown);
            Expected(questionId, q => q.Answers[answerId].Votes.ContainsKey(voterId));
        }

        [TestMethod]
        public void When_AnswerUnvoted_Should_Be()
        {
            string questionId = IdGenerator.Generate();
            string answerId = IdGenerator.Generate();
            string voterId = IdGenerator.Generate();

            QuestionDocument question = CreateVirginQuestion(questionId);
            question.Answers.Add(answerId, new AnswerPayload()
            {
                Id = answerId,
                Body = "Answer body",
                Rating = QuestionScoreTable.AnswerVotedUp,
            });
            question.AnswersCount = 1;
            question.Answers[answerId].Votes.Add(voterId, new VotePayload(VoteDirection.Up, QuestionScoreTable.AnswerVotedUp));
            Given(question);

            Given(new AnswerDocument
            {
                Id = answerId,
                Body = "Answer body",
                Rating = QuestionScoreTable.AnswerVotedUp,
                VotesCount = 1,
                Votes = new Dictionary<string, VotePayload>()
                {
                    { voterId, new VotePayload(VoteDirection.Up, QuestionScoreTable.QuestionVotedUp) }
                }
            });

            When(new AnswerUnvoted()
            {
                AggregateId = questionId,
                AnswerId = answerId,
                VoterId = voterId,
            }, null);

            Expected(questionId, q => q.AnswersCount == 1);
            Expected(questionId, q => q.Answers.ContainsKey(answerId));
            Expected(questionId, q => q.Answers[answerId].Rating == 0);
            Expected(questionId, q => !q.Answers[answerId].Votes.ContainsKey(voterId));
        }
    }
}
