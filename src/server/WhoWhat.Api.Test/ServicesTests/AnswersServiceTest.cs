using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Text;
using WhoWhat.Api.Contract.Answer;
using WhoWhat.Api.Contract.Question;
using WhoWhat.View;
using WhoWhat.View.Documents;
using WhoWhat.View.Payload;

namespace WhoWhat.Api.Test
{
    [TestClass]
    [Ignore]
    public class AnswersServiceTest : ServiceTest
    {
        [TestMethod]
        public void WhenAnswerBodyModified_ShouldUpdateViewModel()
        {
            QuestionHelper questionHelper = new QuestionHelper(AppHost.Container);

            //Arrange
            string questionOwnerUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(questionOwnerUserId);

            string answerOwnerUserId = Guid.NewGuid().ToString();
            CreateInMemoryUser(answerOwnerUserId);

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


            AnswerService answerService = AppHost.Container.Resolve<AnswerService>();

            answerService.Post(new ChangeAnswerRequest()
            {
                
                AnswerId = answerQuestionResponse.AnswerId,
                Body = "Here is a new body"
            });

            //Assert
            QuestionDocument question = AppHost.Container.Resolve<ViewContext>().Questions.GetById(askQuestionResponse.QuestionId);

            Assert.AreEqual(1, question.Answers.Count, "The question should containt only one answer");

            AnswerPayload answer = question.Answers[answerQuestionResponse.AnswerId];

            answer.PrintDump();

            Assert.AreEqual("Here is a new body", answer.Body);

        }
    }
}
