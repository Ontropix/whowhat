using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoWhat.Domain.Question;
using WhoWhat.Domain.Question.Commands;
using WhoWhat.Domain.Question.Events;

namespace WhoWhat.Tests.Domain
{
    [TestClass]
    public class BT_QuestionAggregate : BehaviourTestBase<QuestionAggregate>
    {
        #region Predefined Events

        private QuestionAsked QuestionAskedEvent(string questionId, string authorId = null)
        {
            return new QuestionAsked
            {
                AggregateId = questionId,
                AuthorId = authorId ?? _idGenerator.Generate(),

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
                AuthorId = answerAuthorId ?? _idGenerator.Generate(),

                Body = "Answer Body!"
            };
        }

        #endregion

        #region Question

        [TestMethod]
        public void WhenAskCommand_ShouldBeAskedEvent()
        {
            DateTime creationDate = DateTime.UtcNow;
            string aggregateId = _idGenerator.Generate();
            string authorId = _idGenerator.Generate();

            Given();

            When(new AskQuestion
            {
                AggregateId = aggregateId,
                AuthorId = authorId,

                Body = "Who is that man?",
                CreatedAt = creationDate,

                ImageUri = "http://http://code9.biz/img/image123.png",
                ThumbnailUri = "http://http://code9.biz/thubm/image123.png",

                Tags = new HashSet<string>()
                {
                    "cool",
                    "greatman",
                    "interesting"
                }

            }, (aggregate, command) => aggregate.When(command));

            Expected(new QuestionAsked
            {
                AggregateId = aggregateId,
                AuthorId = authorId,

                Body = "Who is that man?",

                ImageUri = "http://http://code9.biz/img/image123.png",
                ThumbnailUri = "http://http://code9.biz/thubm/image123.png",

                Tags = new HashSet<string>()
                {
                    "cool",
                    "greatman",
                    "interesting"
                }
            });
        }

        [TestMethod]
        public void WhenChangeInfoCommand_ShouldBeInfoChangedEvent()
        {
            string aggregateId = _idGenerator.Generate();

            Given(QuestionAskedEvent(aggregateId));

            When(new ChangeQuestionInfo()
            {
                AggregateId = aggregateId,
                Body = "Who is this woman?",
                Tags = new HashSet<string>()
                {
                    "cool",
                    "women",
                    "beautiful"
                }

            }, (aggregate, command) => aggregate.When(command));

            Expected(new QuestionInfoChanged
            {
                AggregateId = aggregateId,
                Body = "Who is this woman?",
                Tags = new HashSet<string>()
                {
                    "cool",
                    "women",
                    "beautiful"
                }
            });
        }

        [TestMethod]
        public void WhenCloseQuestionCommand_ShouldBeQuestionClosedEvent()
        {
            string questionId = _idGenerator.Generate();

            Given(QuestionAskedEvent(questionId));

            When(new CloseQuestion()
            {
                AggregateId = questionId

            }, (aggregate, command) => aggregate.When(command));

            Expected(new QuestionClosed()
            {
                AggregateId = questionId,
            });
        }

        [TestMethod]
        public void WhenRemoveQuestionCommand_ShouldBeQuestionRemovedEvent()
        {
            string questionId = _idGenerator.Generate();

            Given(QuestionAskedEvent(questionId));

            When(new RemoveQuestion()
            {
                AggregateId = questionId

            }, (aggregate, command) => aggregate.When(command));

            Expected(new QuestionRemoved()
            {
                AggregateId = questionId
            });
        }

        #endregion

        #region Answering

        [TestMethod]
        public void WhenAnswerQuestionCommand_ShouldBeQuestionAnsweredEvent()
        {
            string questionId = _idGenerator.Generate();

            string answerId = _idGenerator.Generate();
            string answerAuthorId = _idGenerator.Generate();

            Given(QuestionAskedEvent(questionId));

            When(new AnswerQuestion()
            {
                AggregateId = questionId,

                AnswerId = answerId,
                AuthorId = answerAuthorId,

                Body = "In Minsk, Belarus!"

            }, (aggregate, command) => aggregate.When(command));

            Expected(new QuestionAnswered()
            {
                AggregateId = questionId,

                AnswerId = answerId,
                AuthorId = answerAuthorId,

                Body = "In Minsk, Belarus!"
            });
        }

        [TestMethod]
        public void WhenChangeAnswerCommand_ShouldBeAnswerChangedEvent()
        {
            string questionId = _idGenerator.Generate();

            string answerId = _idGenerator.Generate();
            string answerAuthorId = _idGenerator.Generate();

            Given(QuestionAskedEvent(questionId),
                new QuestionAnswered()
                {
                    AggregateId = questionId,

                    AnswerId = answerId,
                    AuthorId = answerAuthorId,

                    Body = "In Minsk, Belarus!"
                });

            When(new ChangeAnswerBody()
            {
                AggregateId = questionId,
                AnswerId = answerId,
                NewBody = "In Brest, Belarus!"

            }, (aggregate, command) => aggregate.When(command));

            Expected(new AnswerBodyChanged()
            {
                AggregateId = questionId,
                AnswerId = answerId,
                NewBody = "In Brest, Belarus!"
            });
        }

        [TestMethod]
        public void WhenAcceptAnswerCommand_ShouldBeAnswerAcceptedEvent()
        {
            string aggregateId = _idGenerator.Generate();

            string answerId = _idGenerator.Generate();
            string answerAuthorId = _idGenerator.Generate();

            Given(QuestionAskedEvent(aggregateId),
                new QuestionAnswered()
                {
                    AggregateId = aggregateId,

                    AnswerId = answerId,
                    AuthorId = answerAuthorId,

                    Body = "In Minsk, Belarus!"
                });

            When(new AcceptAnswer()
            {
                AggregateId = aggregateId,
                AnswerId = answerId

            }, (aggregate, command) => aggregate.When(command));

            Expected(new AnswerAccepted()
            {
                AggregateId = aggregateId,
                AnswerId = answerId
            });
        }

        [TestMethod]
        public void WhenUnacceptAnswerCommand_ShouldBeAnswerUnacceptedEvent()
        {
            string questionId = _idGenerator.Generate();
            
            string answerId = _idGenerator.Generate();
            string answerAuthorId = _idGenerator.Generate();

            Given(
                QuestionAskedEvent(questionId),
                new QuestionAnswered()
                {
                    AggregateId = questionId,

                    AnswerId = answerId,
                    AuthorId = answerAuthorId,

                    Body = "In Minsk, Belarus!"
                },
                new AnswerAccepted()
                {
                    AggregateId = questionId,
                    AnswerId = answerId
                });

            When(new UnacceptAnswer()
            {
                AggregateId = questionId,
                AnswerId = answerId

            }, (aggregate, command) => aggregate.When(command));

            Expected(new AnswerUnaccepted()
            {
                AggregateId = questionId,
                AnswerId = answerId
            });
        }

        [TestMethod]
        public void WhenRemoveAnswerCommand_ShouldBeAnswerRemovedEvent()
        {
            string questionId = _idGenerator.Generate();

            string answerId = _idGenerator.Generate();
            string answerAuthorId = _idGenerator.Generate();

            Given(QuestionAskedEvent(questionId),
                new QuestionAnswered()
                {
                    AggregateId = questionId,

                    AnswerId = answerId,
                    AuthorId = answerAuthorId,

                    Body = "In Minsk, Belarus!"
                });

            When(new RemoveAnswer()
            {
                AggregateId = questionId,
                AnswerId = answerId

            }, (aggregate, command) => aggregate.When(command));

            Expected(new AnswerRemoved()
            {
                AggregateId = questionId,
                AnswerId = answerId
            });
        }

        #endregion

        #region Voting

        [TestMethod]
        public void When_VoteQuestionUp_Command_ShouldBe_QuestionVotedUp_Event()
        {
            string aggregateId = _idGenerator.Generate();
            string voterId = _idGenerator.Generate();

            Given(QuestionAskedEvent(aggregateId));

            When(new VoteQuestionUp()
            {
                AggregateId = aggregateId,
                VoterId = voterId

            }, (aggregate, command) => aggregate.When(command));

            Expected(new QuestionVotedUp()
            {
                AggregateId = aggregateId,
                VoterId = voterId,
                RatingShift = QuestionScoreTable.QuestionVotedUp
            });
        }

        [TestMethod]
        public void When_VoteQuestionDown_Command_ShouldBe_QuestionVotedDown_Event()
        {
            string aggregateId = _idGenerator.Generate();
            string voterId = _idGenerator.Generate();

            Given(QuestionAskedEvent(aggregateId));

            When(new VoteQuestionDown()
            {
                AggregateId = aggregateId,
                VoterId = voterId

            }, (aggregate, command) => aggregate.When(command));

            Expected(new QuestionVotedDown()
            {
                AggregateId = aggregateId,
                VoterId = voterId,
                RatingShift = QuestionScoreTable.QuestionVotedDown
            });
        }

        [TestMethod]
        public void When_UnvoteQuestion_Command_ShouldBe_QuestionUnvoted_Event()
        {
            string aggregateId = _idGenerator.Generate();
            string voterId = _idGenerator.Generate();

            Given(
                QuestionAskedEvent(aggregateId),
                new QuestionVotedDown()
                {
                    AggregateId = aggregateId,
                    VoterId = voterId
                });

            When(new UnvoteQuestion()
            {
                AggregateId = aggregateId,
                VoterId = voterId

            }, (aggregate, command) => aggregate.When(command));

            Expected(new QuestionUnvoted()
            {
                AggregateId = aggregateId,
                VoterId = voterId
            });
        }

        [TestMethod]
        public void When_VoteQuestionUp_Then_VotedQuestionDown_ShouldBe_QuestionUnvoted_And_QuestionVotedDown_Event()
        {
            string aggregateId = _idGenerator.Generate();
            string voterId = _idGenerator.Generate();

            Given(
                QuestionAskedEvent(aggregateId),
                new QuestionVotedUp()
                {
                    AggregateId = aggregateId,
                    VoterId = voterId
                });

            When(new VoteQuestionDown()
            {
                AggregateId = aggregateId,
                VoterId = voterId

            }, (aggregate, command) => aggregate.When(command));

            Expected(
                new QuestionUnvoted()
                {
                    AggregateId = aggregateId,
                    VoterId = voterId
                },
                new QuestionVotedDown()
                {
                    AggregateId = aggregateId,
                    VoterId = voterId,
                    RatingShift = QuestionScoreTable.QuestionVotedDown
                });
        }

        [TestMethod]
        public void When_VoteQuestionDown_Then_VotedQuestionUp_ShouldBe_QuestionUnvoted_And_QuestionVotedUp_Event()
        {
            string aggregateId = _idGenerator.Generate();
            string voterId = _idGenerator.Generate();

            Given(
                QuestionAskedEvent(aggregateId),
                new QuestionVotedDown()
                {
                    AggregateId = aggregateId,
                    VoterId = voterId
                });

            When(new VoteQuestionUp()
            {
                AggregateId = aggregateId,
                VoterId = voterId

            }, (aggregate, command) => aggregate.When(command));

            Expected(
                new QuestionUnvoted()
                {
                    AggregateId = aggregateId,
                    VoterId = voterId
                },
                new QuestionVotedUp()
                {
                    AggregateId = aggregateId,
                    VoterId = voterId,
                    RatingShift = QuestionScoreTable.QuestionVotedUp
                });
        }


        [TestMethod]
        public void When_VoteAnswerUp_Command_ShouldBe_AnswerVotedUp_Event()
        {
            string questionId = _idGenerator.Generate();
            string answerId = _idGenerator.Generate();
            string voterId = _idGenerator.Generate();

            Given(
                QuestionAskedEvent(questionId),
                QuestionAnsweredEvent(questionId, answerId)
                );

            When(new VoteAnswerUp()
            {
                AggregateId = questionId,
                AnswerId = answerId,
                VoterId = voterId

            }, (aggregate, command) => aggregate.When(command));

            Expected(new AnswerVotedUp()
            {
                AggregateId = questionId,
                AnswerId = answerId,
                VoterId = voterId,
                RatingShift = QuestionScoreTable.AnswerVotedUp
            });
        }

        [TestMethod]
        public void When_VoteAnswerDown_Command_ShouldBe_AnswerVotedDown_Event()
        {
            string questionId = _idGenerator.Generate();
            string answerId = _idGenerator.Generate();
            string voterId = _idGenerator.Generate();

            Given(
                QuestionAskedEvent(questionId),
                QuestionAnsweredEvent(questionId, answerId)
                );

            When(new VoteAnswerDown()
            {
                AggregateId = questionId,
                AnswerId = answerId,
                VoterId = voterId

            }, (aggregate, command) => aggregate.When(command));

            Expected(new AnswerVotedDown()
            {
                AggregateId = questionId,
                AnswerId = answerId,
                VoterId = voterId,
                RatingShift = QuestionScoreTable.AnswerVotedDown
            });
        }

        [TestMethod]
        public void When_UnvoteAnswer_Command_ShouldBe_AnswerUnvoted_Event()
        {
            string aggregateId = _idGenerator.Generate();
            string answerId = _idGenerator.Generate();
            string voterId = _idGenerator.Generate();

            Given(
                QuestionAskedEvent(aggregateId),
                QuestionAnsweredEvent(aggregateId, answerId),
                new AnswerVotedUp()
                {
                    AggregateId = aggregateId,
                    AnswerId = answerId,
                    VoterId = voterId
                });

            When(new UnvoteAnswer()
            {
                AggregateId = aggregateId,
                AnswerId = answerId,
                VoterId = voterId

            }, (aggregate, command) => aggregate.When(command));

            Expected(new AnswerUnvoted()
            {
                AggregateId = aggregateId,
                AnswerId = answerId,
                VoterId = voterId
            });
        }

        #endregion
    }
}
