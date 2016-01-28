using Platform.Uniform;
using WhoWhat.View.Documents;

namespace WhoWhat.View
{
    public class ViewContext
    {
        private readonly UniformContext _uniformContext;

        public ViewContext(UniformContext uniformContext)
        {
            _uniformContext = uniformContext;
        }

        public IReadOnlyDocumentStore<UserDocument> Users
        {
            get { return _uniformContext.GetReadOnlyDocumentStore<UserDocument>(); }
        }

        public IReadOnlyDocumentStore<QuestionDocument> Questions
        {
            get { return _uniformContext.GetReadOnlyDocumentStore<QuestionDocument>(); }
        }

        public IReadOnlyDocumentStore<AnswerDocument> Answers
        {
            get { return _uniformContext.GetReadOnlyDocumentStore<AnswerDocument>(); }
        }

        public IReadOnlyDocumentStore<NotificationDocument> Notifications
        {
            get { return _uniformContext.GetReadOnlyDocumentStore<NotificationDocument>(); }
        }
    }
}
