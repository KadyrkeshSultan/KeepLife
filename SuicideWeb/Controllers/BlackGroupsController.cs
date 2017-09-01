using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SuicideWeb.Models;

namespace SuicideWeb.Controllers
{
    public class BlackGroupsController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        // GET: BlackGroups
        public ActionResult Index()
        {
            return View(db.BlackGroups.ToList());
        }

        
        // GET: BlackGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlackGroups/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GroupId")] BlackGroup blackGroup)
        {
            if (ModelState.IsValid)
            {
                db.BlackGroups.Add(blackGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blackGroup);
        }

        // GET: BlackGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlackGroup blackGroup = db.BlackGroups.Find(id);
            if (blackGroup == null)
            {
                return HttpNotFound();
            }
            return View(blackGroup);
        }

        // POST: BlackGroups/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GroupId")] BlackGroup blackGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blackGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blackGroup);
        }

        // GET: BlackGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlackGroup blackGroup = db.BlackGroups.Find(id);
            if (blackGroup == null)
            {
                return HttpNotFound();
            }
            return View(blackGroup);
        }

        // POST: BlackGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlackGroup blackGroup = db.BlackGroups.Find(id);
            db.BlackGroups.Remove(blackGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
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
