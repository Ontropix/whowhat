using System;
using System.Reflection;
using MongoDB.Driver.Builders;
using Platform.MongoDb;
using Platform.Uniform;
using Platform.Uniform.InMemory;
using Platform.Uniform.Mongodb;
using StructureMap;
using WhoWhat.View;
using WhoWhat.View.Documents;

namespace WhoWhat.Core.Configuration
{
    internal class UniformConfigurator
    {
        internal UniformConfigurator RegisterInMemoryUniform(IContainer container)
        {
            UniformContext uniformContext = new InMemoryContext();
            container.Configure(config => config.For<UniformContext>().Singleton().Use(uniformContext));

            return this;
        }

        internal UniformConfigurator RegisterMongoUniform(IContainer container)
        {
            var appSettings = container.GetInstance<AppSettings>();
            var mongoView = new MongoInstance(appSettings.ViewConnectionString);

            UniformContext uniformContext = new MongoContext(mongoView.GetDatabase(), ViewContextClassMap.GetClassMap());
            container.Configure(config => config.For<UniformContext>().Singleton().Use(uniformContext));

            return this;
        }

        internal UniformConfigurator RegisterViewContext(IContainer container)
        {
            var uniform = container.GetInstance<UniformContext>();
            var viewContext = new ViewContext(uniform);
            container.Configure(config => config.For<ViewContext>().Singleton().Use(viewContext));

            return this;
        }

        internal UniformConfigurator RegisterDocuments(IContainer container)
        {
            container.Configure(config =>
                                    config.Scan(scan =>
                                    {
                                        scan.Assembly(typeof (Namespace_ViewProject).Assembly);
                                        scan.WithDefaultConventions();
                                        scan.AddAllTypesOf(typeof (IDocument));
                                    })
                );

            return this;
        }

        internal UniformConfigurator RegisterDocumentStores(IContainer container)
        {
            UniformContext uniform = container.GetInstance<UniformContext>();
            foreach (IDocument document in container.GetAllInstances<IDocument>())
            {
                Type documentType = document.GetType();
                Type storeType = typeof (IDocumentStore<>).MakeGenericType(documentType);

                MethodInfo method = uniform.GetType().GetMethod("GetDocumentStore").MakeGenericMethod(documentType);
                container.Configure(config => config.For(storeType).Use(method.Invoke(uniform, null)));
            }

            return this;
        }


        // todo should be moved to platform
        internal UniformConfigurator CreateIndexes(IContainer container)
        {
            var appSettings = container.GetInstance<AppSettings>();
            var mongoView = new MongoInstance(appSettings.ViewConnectionString).GetDatabase();

            var questions = mongoView.GetCollection<QuestionDocument>("questions");
            questions.CreateIndex(IndexKeys<QuestionDocument>.Descending(x => x.Rating));
            questions.CreateIndex(IndexKeys<QuestionDocument>.Descending(x => x.CreatedAt));
            questions.CreateIndex(IndexKeys<QuestionDocument>.Descending(x => x.CreatedAt, x => x.IsResolved));
            questions.CreateIndex(IndexKeys<QuestionDocument>.Descending(x => x.CreatedAt, x => x.AnswersCount, x => x.Rating));


            var answers = mongoView.GetCollection<AnswerDocument>("answers");
            answers.CreateIndex(IndexKeys<AnswerDocument>.Descending(x => x.Rating));

            var users = mongoView.GetCollection<UserDocument>("users");
            users.CreateIndex(IndexKeys<UserDocument>.Descending(x => x.Reputation));

            var notifications = mongoView.GetCollection<NotificationDocument>("notifications");
            notifications.CreateIndex(IndexKeys<NotificationDocument>.Descending(x => x.CreatedAt));
            notifications.CreateIndex(IndexKeys<NotificationDocument>.Descending(x => x.CreatedAt, x => x.TargetUserId));

            return this;
        }
    }
}