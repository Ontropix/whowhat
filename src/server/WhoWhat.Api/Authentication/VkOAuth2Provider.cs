using System.Collections.Generic;
using System.Xml.Linq;
using ServiceStack.Authentication.OAuth2;
using ServiceStack.Configuration;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceModel.Extensions;
using ServiceStack.Text;

namespace WhoWhat.Core
{
    public class VkOAuth2Provider : OAuth2Provider
    {
        public const string Name = "vk";
        public const string Realm = "https://oauth.vk.com/authorize";

        public VkOAuth2Provider(IResourceManager appSettings)
            : base(appSettings, Realm, Name)
        {
            base.AuthorizeUrl = (base.AuthorizeUrl ?? "https://oauth.vk.com/authorize");
            base.AccessTokenUrl = (base.AccessTokenUrl ?? "https://oauth.vk.com/access_token");
            base.UserProfileUrl = (base.UserProfileUrl ?? 
                "https://api.vk.com/method/getProfiles.xml?fields=uid,first_name,last_name,nickname,screen_name,sex,bdate,city,country,timezone,photo,photo_medium,photo_big");
        }

        protected override Dictionary<string, string> CreateAuthInfo(string accessToken)
        {
            /*
                <user>
                    <uid>3532453</uid>
                    <first_name>Андрей</first_name>
                    <last_name>Шнейдер</last_name>
                    <sex>2</sex>
                    <nickname />
                    <screen_name>meggerz</screen_name>
                    <bdate>8.11</bdate>
                    <city>0</city>
                    <country>3</country>
                    <timezone>2</timezone>
                    <photo>http://cs409429.vk.me/v409429453/2dcc/0JCoigt7oKY.jpg</photo>
                    <photo_medium>http://cs409429.vk.me/v409429453/2dcb/tkOeodpDKvE.jpg</photo_medium>
                    <photo_big>http://cs409429.vk.me/v409429453/2dc8/gpeU1rT0n98.jpg</photo_big>
                  </user>
             */

            string request = base.UserProfileUrl.AddQueryParam("access_token", accessToken);

            XDocument response = XDocument.Parse(request.GetXmlFromUrl());
            XElement root = response.Root.Element("user");

            return new Dictionary<string, string>
            {
                {
                    "user_id", root.GetString("uid")
                },			                    					
                {
                    "first_name", root.GetString("first_name")
                },
                {
                    "last_name", root.GetString("last_name")
                },
                {
                    "photo", root.GetString("photo")
                },
                {
                    "photo_big", root.GetString("photo_big")
                }
            };

        }
    }
}