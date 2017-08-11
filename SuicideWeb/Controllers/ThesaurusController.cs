using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SuicideWeb.Models;

namespace SuicideWeb.Controllers
{
    public class ThesaurusController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        // GET: Thesaurus
        public ActionResult Index()
        {
            return View(db.Thesauruss.ToList());
        }
        

        // GET: Thesaurus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Thesaurus/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Senten")] Thesaurus thesaurus)
        {
            if (ModelState.IsValid)
            {
                db.Thesauruss.Add(thesaurus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(thesaurus);
        }

        // GET: Thesaurus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thesaurus thesaurus = db.Thesauruss.Find(id);
            if (thesaurus == null)
            {
                return HttpNotFound();
            }
            return View(thesaurus);
        }

        // POST: Thesaurus/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Senten")] Thesaurus thesaurus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thesaurus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thesaurus);
        }

        // GET: Thesaurus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thesaurus thesaurus = db.Thesauruss.Find(id);
            if (thesaurus == null)
            {
                return HttpNotFound();
            }
            return View(thesaurus);
        }

        // POST: Thesaurus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Thesaurus thesaurus = db.Thesauruss.Find(id);
            db.Thesauruss.Remove(thesaurus);
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
