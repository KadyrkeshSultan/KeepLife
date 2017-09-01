using SuicideWeb.Models;
using System.Net;
using System.Web.Mvc;
using VkNet;

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

            VkApi app = AuthorizeVk.app;
            
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