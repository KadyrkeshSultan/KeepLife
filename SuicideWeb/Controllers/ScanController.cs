using SuicideAlpha;
using SuicideWeb.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VkNet;
using DuoVia.FuzzyStrings;
using System.Collections.Generic;

namespace SuicideWeb.Controllers
{
    public class ScanController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
        private static VkApi app;
        public List<Thesaurus> listSen;
        public List<BlackGroup> blackGroups;

        private void Authorize()
        {
            app = AuthorizeVk.app;
            listSen = db.Thesauruss.ToList();
            blackGroups = db.BlackGroups.ToList();
        }
        // GET: Scan 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Group(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var persons = db.Persons.Where(p => p.GroupId == id);
            Authorize();
            ResultScan[] result = new ResultScan[persons.Count()];

            int count = 0;
            foreach (var person in persons)
            {
                result[count] = GetResultPerson(person);
                count++;
            }
                
            return View(result);
        }


        public ActionResult Person(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            Authorize();
            return View(GetResultPerson(person));
        }

        private ResultScan GetResultPerson(Person person)
        {
            ResultScan result = new ResultScan();
            string[] compareSenten;
            Wall wall;

            try
            {
                result.UserId = person.PersonId;
                result.User = person.Name;
                result.TextStatus = app.Status.Get(person.VkId).Text;
                result.MatchesDic = 0;
                wall = new Wall(app, person.VkId);
                var groups = app.Groups.Get(person.VkId);
                result.TextWall = wall.GetTextWall();
                compareSenten = result.TextWall.Split('\n', '.');

                foreach (var g in groups)
                    if (blackGroups.FirstOrDefault(bg => bg.GroupId == g.Id) != null)
                        result.MatchesGroups++;
            }
            catch
            {
                compareSenten = null;
                result.TextStatus = null;
            }

            if (result.TextStatus != null)
                foreach (string textS in result.TextStatus.Split('.'))
                    foreach (var compare in listSen)
                    {
                        var dice = textS.LongestCommonSubsequence(compare.Senten);
                        if (dice.Item2 > 0.20)
                        {
                            result.MatchesDic++;
                            result.InThesaurus += compare.Senten + "|";
                            result.Original += textS + "|";
                        }
                    }
            if (compareSenten != null)
                foreach (string senten in compareSenten)
                {
                    if (senten != "")
                        foreach (var compare in listSen)
                        {
                            var dice = senten.LongestCommonSubsequence(compare.Senten);
                            if (dice.Item2 > 0.20)
                            {
                                result.MatchesDic++;
                                result.InThesaurus += compare.Senten + "|";
                                result.Original += senten + "|";
                            }
                        }
                }
            if (result.MatchesDic < 3)
                result.Level = ResultScan.SeverityLevel.Низкий;
            else if (result.MatchesDic > 3 && result.MatchesDic < 5)
                result.Level = ResultScan.SeverityLevel.Средний;
            else
                result.Level = ResultScan.SeverityLevel.Высокий;

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                app.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
