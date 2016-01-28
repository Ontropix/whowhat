using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Text;
using WhoWhat.Api.Contract.Answer;
using WhoWhat.Api.Contract.Question;
using WhoWhat.Domain.Question;
using WhoWhat.View;
using WhoWhat.View.Documents;

namespace WhoWhat.Api.Test
{
    [TestClass]
    //[Ignore]
    public class QuestionServiceTest : ServiceTest
    {
        [TestMethod]
        public void WhenQuestionAnswered_ShouldAnswerResponseBeValid()
        {
            QuestionHelper questionHelper = new QuestionHelper(AppHost.Container);

            string userId = Guid.NewGuid().ToString();

            CreateInMemoryUser(userId);
            SetSession(new CustomUserSession()
            {
                UserId = userId
            });

            AskQuestionResponse askQuestionResponse = questionHelper.AskQuestion();

            askQuestionResponse.PrintDump();
            Assert.IsNotNull(askQuestionResponse.QuestionId, "Question should be created. QuestionId cannot be null.");

            var answerResponse = questionHelper.AnswerQuestion(askQuestionResponse.QuestionId);
            answerResponse.PrintDump();
            Assert.IsNotNull(answerResponse.AnswerId, "Answer should be created. AnswerId cannot be null.");

            var removeResponse = questionHelper.RemoveAnswer(answerResponse.AnswerId);
        }

        [TestMethod]
        public void WhenRecentQuestions_ShouldReturnNotEmptyResponse()
        {
            //No session is required

            QuestionService service = AppHost.Container.Resolve<QuestionService>();

            QuestionSummariesResponse response = service.Get(new RecentQuestionsRequest());

            Assert.IsNotNull(response);

            //Print a dump of the results to Console
            response.PrintDump();
        }

        [TestMethod]
        public void WhenVoteQuestionUp_ShouldIncreaseQuestionRating()
        {
            QuestionHelper questionHelper = new QuestionHelper(AppHost.Container);

            //Arrange
            string questionOwnerUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(questionOwnerUserId);

            string votedUserUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(votedUserUserId);

            SetSession(new CustomUserSession()
            {
                UserId = questionOwnerUserId
            });

            AskQuestionResponse askQuestionResponse = questionHelper.AskQuestion();

            //Act
            SetSession(new CustomUserSession()
            {
                UserId = votedUserUserId
            });

            QuestionService service = AppHost.Container.Resolve<QuestionService>();

            service.Post(new VoteQuestionUpRequest()
            {
                QuestionId = askQuestionResponse.QuestionId
            });

            //Assert
            QuestionDocument question = AppHost.Container.Resolve<ViewContext>()
                .Questions.GetById(askQuestionResponse.QuestionId);

            Assert.AreEqual(QuestionScoreTable.QuestionVotedUp, question.Rating, "Vote vthe question up should increase its rating +1");
        }

        [TestMethod]
        public void WhenVoteQuestionDown_ShouldDecreaseQuestionRating()
        {
            QuestionHelper questionHelper = new QuestionHelper(AppHost.Container);

            //Arrange
            string questionOwnerUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(questionOwnerUserId);

            string votedUserUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(votedUserUserId);

            SetSession(new CustomUserSession()
            {
                UserId = questionOwnerUserId
            });

            AskQuestionResponse askQuestionResponse = questionHelper.AskQuestion();

            //Act
            SetSession(new CustomUserSession()
            {
                UserId = votedUserUserId
            });

            QuestionService service = AppHost.Container.Resolve<QuestionService>();

            service.Post(new VoteQuestionDownRequest()
            {
                QuestionId = askQuestionResponse.QuestionId
            });

            //Assert
            QuestionDocument question = AppHost.Container.Resolve<ViewContext>().Questions.GetById(askQuestionResponse.QuestionId);

            Assert.AreEqual(QuestionScoreTable.QuestionVotedDown, question.Rating, "Vote question down should decrease its rating -1");
        }

        [TestMethod]
        public void WhenVoteQuestionUpDown_ShouldDownReplaceUp()
        {
            QuestionHelper questionHelper = new QuestionHelper(AppHost.Container);

            string questionOwnerUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(questionOwnerUserId);

            string votedUserUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(votedUserUserId);

            SetSession(new CustomUserSession()
            {
                UserId = questionOwnerUserId
            });

            AskQuestionResponse askQuestionResponse = questionHelper.AskQuestion();

            //Act
            SetSession(new CustomUserSession()
            {
                UserId = votedUserUserId
            });

            QuestionService service = AppHost.Container.Resolve<QuestionService>();

            //Vote up
            service.Post(new VoteQuestionUpRequest()
            {
                QuestionId = askQuestionResponse.QuestionId
            });

            //Vote down
            service.Post(new VoteQuestionDownRequest()
            {
                QuestionId = askQuestionResponse.QuestionId
            });

            //Assert
            QuestionDocument question = AppHost.Container.Resolve<ViewContext>().Questions.GetById(askQuestionResponse.QuestionId);

            Assert.AreEqual(QuestionScoreTable.QuestionVotedDown, question.Rating, 
                "Vote question up and down should decrease its rating -1");
        }

        [TestMethod]
        public void WhenVoteAnswerUp_ShouldIncreaseUserRating()
        {
            QuestionHelper questionHelper = new QuestionHelper(AppHost.Container);

            //Arrange
            string questionOwnerUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(questionOwnerUserId);

            string answerOwnerUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(answerOwnerUserId);

            string votedUserUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(votedUserUserId);

            //First user asks question
            SetSession(new CustomUserSession()
            {
                UserId = questionOwnerUserId
            });

            AskQuestionResponse askQuestionResponse = questionHelper.AskQuestion();

            //Second user answers the quetion question
            SetSession(new CustomUserSession()
            {
                UserId = answerOwnerUserId
            });

            AnswerStatusResponse answerQuestionResponse = questionHelper.AnswerQuestion(askQuestionResponse.QuestionId);

            //Third user votes for the answer up.
            SetSession(new CustomUserSession()
            {
                UserId = votedUserUserId
            });

            AnswerService service = AppHost.Container.Resolve<AnswerService>();

            service.Post(new VoteAnswerUpRequest()
            {
                AnswerId = answerQuestionResponse.AnswerId
            });

            //Assert
            AnswerDocument answer = AppHost.Container.Resolve<ViewContext>().Answers.GetById(answerQuestionResponse.AnswerId);

            Assert.IsTrue(answer.Rating > 0);

            UserDocument user = AppHost.Container.Resolve<ViewContext>().Users.GetById(answerOwnerUserId);

            Assert.AreEqual(QuestionScoreTable.QuestionVotedUp, user.Reputation, "Vote for the answer up should increase rating of the user who answered (+10)");
        }
    }
}
