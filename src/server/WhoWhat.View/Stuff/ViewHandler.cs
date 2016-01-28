using Platform.Uniform;

namespace WhoWhat.View
{
    public abstract class ViewHandler<TDocument> where TDocument : class, IDocument, new()
    {
        protected readonly  IDocumentStore<TDocument> DocumentStore;
        protected readonly ViewContext ViewContext;

        protected ViewHandler(IDocumentStore<TDocument> documentStore, ViewContext viewContext)
        {
            DocumentStore = documentStore;
            ViewContext = viewContext;
        }
    }
}