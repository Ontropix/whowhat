using System.Collections.Generic;
using System.Linq;
using ServiceStack.Authentication.OAuth2;
using ServiceStack.Configuration;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace WhoWhat.Core
{
    public class FbOAuth2Provider : OAuth2Provider
    {
        public const string Name = "fb";
        public const string Realm = "https://graph.facebook.com/oauth/authorize";

        public FbOAuth2Provider(IResourceManager appSettings)
            : base(appSettings, Realm, Name)
        {

            string profile = "https://graph.facebook.com/fql"
                .AddQueryParam("q",
                               "SELECT uid, pic_big, pic_small, first_name, sex, last_name FROM user WHERE uid=me()");

            base.AuthorizeUrl = (base.AuthorizeUrl ?? "https://graph.facebook.com/oauth/authorize");
            base.AccessTokenUrl = (base.AccessTokenUrl ?? "https://graph.facebook.com/oauth/access_token");
            base.UserProfileUrl = (base.UserProfileUrl ?? profile);
        }

        protected override Dictionary<string, string> CreateAuthInfo(string accessToken)
        {

            /*
             {
                  "data": [
                    {
                      "uid": "677222139010730",
                      "pic_big": "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-ash2/t1.0-1/s200x200/283657_497467330319546_446701069_n.jpg",
                      "first_name": "Andrei",
                      "sex": "male",
                      "last_name": "Shneider",
                      "pic_small": "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-ash2/t1.0-1/s50x50/283657_497467330319546_446701069_t.jpg"
                    }
                  ]
               }
             */

            string request = base.UserProfileUrl.AddQueryParam("access_token", accessToken);
            string jsonFromUrl = request.GetJsonFromUrl();

            JsonObject response = JsonObject.Parse(jsonFromUrl).ArrayObjects("data").First();

            return new Dictionary<string, string>
                {
                    {
                        "user_id", response["uid"]
                    },
                    {
                        "first_name", response["first_name"]
                    },
                    {
                        "last_name", response["last_name"]
                    },
                    {
                        "photo", response["pic_small"]
                    },
                    {
                        "photo_big", response["pic_big"]
                    }
                };

        }
    }
}
