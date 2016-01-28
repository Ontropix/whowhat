using System;
using System.Collections.Generic;
using WhoWhat.View.Documents;

namespace WhoWhat.View
{
    public static class ViewContextClassMap
    {
        private const string UsersCollectionName = "users";
        private const string QuestionsCollectionName = "questions";
        private const string AnswersCollectionName = "answers";
        private const string NotificationsCollectionName = "notifications";
        
        private static readonly Dictionary<Type, string> _typeMap = new Dictionary<Type, string>();
        public static string GetCollectionName<TDocument>()
        {
            return _typeMap[typeof (TDocument)];
        }

        /// <summary>
        /// Gets map for Documents registration
        /// </summary>
        /// <returns>Key: Document type, Value: collection name</returns>
        public static Dictionary<Type, string> GetClassMap()
        {
            return _typeMap;
        }

        static ViewContextClassMap()
        {
            _typeMap.Add(typeof(UserDocument), UsersCollectionName);
            _typeMap.Add(typeof(QuestionDocument), QuestionsCollectionName);
            _typeMap.Add(typeof(AnswerDocument), AnswersCollectionName);
            _typeMap.Add(typeof(NotificationDocument), NotificationsCollectionName);
        }
    }
}