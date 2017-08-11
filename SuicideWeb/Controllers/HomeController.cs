using System.Net;
using System.Web.Mvc;
using VkNet;
using VkNet.Enums.Filters;

namespace SuicideWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult GetId(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            VkApi app = new VkApi();


            ulong appID = 5984263;
            string login = "87022365516";
            string pass = "13071307";
            Settings set = Settings.All;
            try
            {
                app.Authorize(new ApiAuthParams
                {
                    ApplicationId = appID,
                    Login = login,
                    Password = pass,
                    Settings = set
                });
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }

            VkNet.Model.User user;
            try
            {
                string[] temp = id.Split('/');
                user = app.Users.Get(temp[temp.Length -1]);
            }
            catch
            {
                user = null;
            }
            if (user != null)
            {
                ViewBag.UserId = user.Id;
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.BirthDate = user.BirthDate;
            }
            else
                ViewBag.UserId = "Не найден";
            return View();
        }
    }
}