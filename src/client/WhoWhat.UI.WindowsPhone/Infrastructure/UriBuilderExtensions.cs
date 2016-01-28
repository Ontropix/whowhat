using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Caliburn.Micro;
using Microsoft.Phone.Shell;

namespace WhoWhat.UI.WindowsPhone.Infrastructure
{
    public static class UriBuilderExtensions
    {
        public static UriBuilder<TViewModel> WidthData<TViewModel, TValue>(
            this UriBuilder<TViewModel> builder, Expression<Func<TViewModel, TValue>> property, TValue value
        )
        {
            MemberExpression member = property.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.", property.ToString()));

            string name = member.Member.Name;

            NavigationParams.AddOrUpdate(name, value);

            return builder;
        }

    }

    public static class NavigationParams
    {
        private const string Prefix = "navigation:";

        public static void AddOrUpdate(string key, object value)
        {
            IDictionary<string, object> state = PhoneApplicationService.Current.State;
            string navigationKey = GetKeyNavigationKey(key);

            if (state.ContainsKey(navigationKey))
            {
                state[navigationKey] = value;
            }
            else
            {
                state.Add(navigationKey, value);
            }
        }

        private static string GetKeyNavigationKey(string original)
        {
            return Prefix + original;
        }

        public static object GetAndRemove(string key)
        {
            string navigationKey = GetKeyNavigationKey(key);

            IDictionary<string, object> state = PhoneApplicationService.Current.State;

            if (state.ContainsKey(navigationKey))
            {
                return state[navigationKey];
            }

            return null;
        }

        public static IEnumerable<string> Keys
        {
            get
            {
                IDictionary<string, object> state = PhoneApplicationService.Current.State;
                return state.Where(x => x.Key.StartsWith(Prefix))
                            .Select(x => x.Key.Replace(Prefix, string.Empty))
                            .ToList();
            }
        }

        public static void Clear()
        {
            IDictionary<string, object> state = PhoneApplicationService.Current.State;

            foreach (string key in Keys)
            {
                string navigationKey = GetKeyNavigationKey(key);
                state.Remove(navigationKey);
            }

        }
    }
}
