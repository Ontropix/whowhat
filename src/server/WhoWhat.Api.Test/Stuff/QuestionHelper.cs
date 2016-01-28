using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Funq;
using WhoWhat.Api.Contract.Answer;
using WhoWhat.Api.Contract.Payload;
using WhoWhat.Api.Contract.Question;
using WhoWhat.Api.Contract.User;
using WhoWhat.UI.Web.Bootstraper;

namespace WhoWhat.Api.Test
{
    public class QuestionHelper
    {
        private readonly Container container;

        public QuestionHelper(Container container)
        {
            this.container = container;
        }

        public AskQuestionResponse AskQuestion()
        {
            QuestionService service = container.Resolve<QuestionService>();

            MemoryStream image = CreateJpegImage();

            AskQuestionResponse response = service.Post(new AskQuestionRequest()
            {
                Body = "Ask question body",
                Tags = new List<string>()
                {
                    "live",
                    "black",
                    "towns"
                },
                Bytes = Convert.ToBase64String(image.GetBuffer())
            });

            return response;
        }

        public AnswerStatusResponse AnswerQuestion(string questionId)
        {
            QuestionService service = container.Resolve<QuestionService>();

            var response = service.Post(new AnswerQuestionRequest()
            {
                QuestionId = questionId,
                Body = "Answer question body",
            });

            return response;
        }

        public AnswerStatusResponse RemoveAnswer(string answerId)
        {
            AnswerService service = container.Resolve<AnswerService>();

            var response = service.Post(new RemoveAnswerRequest
            {
                AnswerId = answerId
            });

            return response;
        }

        public IEnumerable<Notification> GetNotifications(string userId, int count = Int32.MaxValue)
        {
            UsersService usersService = container.Resolve<UsersService>();
            NotificationsResponse response = usersService.Get(new NotificationsRequest() { Take = count });
            return response.Notifications;
        }

        private MemoryStream CreateJpegImage()
        {
            //Create an empty image.
            Bitmap image = new Bitmap(2000, 2000);

            //draw a useless line for some data
            Graphics imageData = Graphics.FromImage(image);
            imageData.DrawLine(new Pen(Color.Red), 0, 0, 2000, 2000);

            //Convert to byte array
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);

            return memoryStream;
        }
    }
}
