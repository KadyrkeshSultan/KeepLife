using VkNet;
using VkNet.Enums.Filters;

namespace SuicideWeb.Models
{
    public class AuthorizeVk
    {
        public static VkApi app;

        public AuthorizeVk()
        {
            app = new VkApi();

            ulong appID = 5984263;
            string login = "87022365516";
            string pass = "13071307";
            Settings set = Settings.All;

            app.Authorize(new ApiAuthParams
            {
                ApplicationId = appID,
                Login = login,
                Password = pass,
                Settings = set
            });
        }
    }
}