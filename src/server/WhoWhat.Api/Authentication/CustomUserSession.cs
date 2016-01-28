using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using NLog;
using Platform.Domain;
using ServiceStack.CacheAccess;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using WhoWhat.Core;
using WhoWhat.Domain.User;
using WhoWhat.Domain.User.Commands;
using WhoWhat.View;
using WhoWhat.View.Documents;

namespace WhoWhat.Api
{
    [DataContract]
    public class CustomUserSession : AuthUserSession
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [DataMember(Order = 50)]
        public string UserId { get; set; }
        
        public override void OnAuthenticated(IServiceBase authService, IAuthSession session, IOAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            base.OnAuthenticated(authService, session, tokens, authInfo);

            IOAuthTokens authToken = session.ProviderOAuthAccess.FirstOrDefault(x => x.Provider == FbOAuth2Provider.Name);

            if (authToken != null)
            {
                SaveOrUpdateRegistration(UserLoginType.Facebook, authService, authToken, authInfo);
                return;
            }

            authToken = session.ProviderOAuthAccess.FirstOrDefault(x => x.Provider == VkOAuth2Provider.Name);

            if (authToken != null)
            {
                SaveOrUpdateRegistration(UserLoginType.Vk, authService, authToken, authInfo);
                return;
            }

            System.Diagnostics.Debug.Fail("Invalid auth provider.");
        }

        private void SaveOrUpdateRegistration(UserLoginType loginType, IServiceBase resolver, IOAuthTokens authToken, Dictionary<string, string> authInfo)
        {
            ViewContext context = resolver.ResolveService<ViewContext>();
            CommandBus commandBus = resolver.ResolveService<CommandBus>();
            IEntityIdGenerator entityIdGenerator = resolver.ResolveService<IEntityIdGenerator>();

            Logger.Debug("Login type: {0}", loginType);

            UserDocument existingUser = context.Users
                                               .AsQueryable()
                                               .FirstOrDefault(user => user.ThirdPartyId == authToken.UserId &&
                                                                       user.LoginType == loginType);

            if (existingUser != null)
            {
                UpdateUserRegistration command = new UpdateUserRegistration()
                {
                    AggregateId = existingUser.Id,
                    FirstName = authInfo["first_name"],
                    LastName = authInfo["last_name"],
                    AccessToken = authToken.AccessToken,
                    PhotoSmallUri = authInfo["photo"],
                    PhotoBigUri = authInfo["photo_big"],
                };

                Logger.Info("Existing user: {0}", command.Dump());
                
                UserId = existingUser.Id;
                commandBus.Send(command, UserId);

            }
            else
            {
                RegisterUser command = new RegisterUser
                {
                    AggregateId = entityIdGenerator.Generate(),
                    FirstName = authInfo["first_name"],
                    LastName = authInfo["last_name"],
                    LoginType = loginType,
                    ThirdPartyId = authToken.UserId,
                    AccessToken = authToken.AccessToken,
                    PhotoSmallUri = authInfo["photo"],
                    PhotoBigUri = authInfo["photo_big"]
                };

                Logger.Info("New user: {0}", command.Dump());

                UserId = command.AggregateId;
                commandBus.Send(command, UserId);
            }

            ICacheClient cacheClient = resolver.ResolveService<ICacheClient>();
            UpdateSession(cacheClient);
        }

        public void UpdateSession(ICacheClient cacheClient)
        {
            string sessionKey = SessionFeature.GetSessionKey(this.Id);

            CustomUserSession session = cacheClient.Get<CustomUserSession>(sessionKey);
            session.UserId = UserId;

            //The sesion  never expiered.
            cacheClient.Set(sessionKey, session);
        }
        
        public override bool IsAuthorized(string provider)
        {
            string sessionKey = SessionFeature.GetSessionKey(this.Id);
            ICacheClient cacheClient = AppHostBase.Resolve<ICacheClient>();

            CustomUserSession session = cacheClient.Get<CustomUserSession>(sessionKey);

            if (session == null)
            {
                return false;
            }

            return session.IsAuthenticated;
        }

    }
}