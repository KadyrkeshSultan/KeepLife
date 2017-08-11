using SuicideAlpha;
using SuicideWeb.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VkNet;
using VkNet.Enums.Filters;
using DuoVia.FuzzyStrings;
using System.Data.Entity;
using System.Collections.Generic;
using System;

namespace SuicideWeb.Controllers
{
    public class ScanController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
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
            #region Authorize 
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
            #endregion
            ResultScan[] result = new ResultScan[persons.Count()];
            string[] compareSenten;
            Wall wall;
            var listSen = db.Thesauruss.ToList();
            int count = 0;
            var dba = db.BlackGroups.ToList();
            foreach (var person in persons)
            {
                result[count] = new ResultScan();
                try
                {
                    result[count].UserId = person.PersonId;
                    result[count].User = person.Name;
                    result[count].TextStatus = app.Status.Get(person.VkId).Text;
                    wall = new Wall(app, person.VkId);
                    compareSenten = wall.GetTextWall().ToLower().Split('.', '\n');
                    var groups = app.Groups.Get(person.VkId);

                    foreach (var g in groups)
                    {
                        foreach (var bg in dba)
                            if (g.Id == bg.GroupId)
                                result[count].MatchesGroups++;
                    }

                    foreach (string textS in result[count].TextStatus.Split('.'))
                        foreach (var compare in listSen)
                        {
                            var dice = textS.LongestCommonSubsequence(compare.Senten);
                            if (dice.Item2 > 0.20)
                                result[count].MatchesDic++;
                        }
                }
                catch
                {
                    compareSenten = null;
                }
                if (compareSenten != null)
                    foreach (string senten in compareSenten)
                    {
                        if (senten != "")
                            foreach (var compare in listSen)
                            {
                                var dice = senten.LongestCommonSubsequence(compare.Senten);
                                if (dice.Item2 > 0.20)
                                    result[count].MatchesDic++;
                            }
                    }
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
            #region Authorize 
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
            #endregion
            ResultScan result = new ResultScan();
            string[] compareSenten;
            Wall wall;
            var listSen = db.Thesauruss.ToList();
            var dbs = db.BlackGroups.ToList();

            try
            {
                result.User = person.Name;
                result.TextStatus = app.Status.Get(person.VkId).Text;
                result.MatchesDic = 0;
                wall = new Wall(app, person.VkId);
                var groups = app.Groups.Get(person.VkId);
                result.TextWall = wall.GetTextWall();
                compareSenten = result.TextWall.Split('\n', '.');

                foreach (var g in groups)
                {
                    foreach (var bg in dbs)
                        if (g.Id == bg.GroupId)
                            result.MatchesGroups++;
                }
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
                foreach (string senten in
                compareSenten)
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
            return View(result);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
