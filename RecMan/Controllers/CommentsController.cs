using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecMan.DAL;
using RecMan.Models;
using Microsoft.AspNet.Identity;

namespace RecMan.Controllers
{
    public class CommentsController : Controller
    {
        private RecManContext db = new RecManContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Resource);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.ResourceId = new SelectList(db.Resources, "ResourceID", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Message,UserId,UserName,DateTime,DateTimeEdit,ResourceID")] int id, Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.UserId = User.Identity.GetUserId();
                comment.UserName = HttpContext.User.Identity.Name;
                comment.ResourceID = id;
                comment.DateTime = System.DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Resources", new { id = id });
            }

            ViewBag.ResourceId = new SelectList(db.Resources, "ResourceID", "Title", comment.ResourceID);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResourceId = new SelectList(db.Resources, "ResourceID", "Title", comment.ResourceID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(CommentEditViewModel vm, int ResId, int id)
        {
            var comment = db.Comments.Find(vm.ID);
            var CreatedDate = comment.DateTime;

            if (ModelState.IsValid)
            {
                comment.Message = vm.Message;
                comment.UserId = User.Identity.GetUserId();
                comment.UserName = HttpContext.User.Identity.Name;
                comment.DateTimeEdit = System.DateTime.Now;
                comment.DateTime = CreatedDate;
                comment.ResourceID = ResId;
                db.SaveChanges();
                return RedirectToAction("Details", "Resources", new { id = ResId });
            }
            return View();
        }

                /*comment.DateTimeEdit = System.DateTime.Now;
                comment.ID = id;
                comment.ResourceID = ResId;
                comment.DateTime = comment.DateTime;
                comment.UserId = User.Identity.GetUserId();
                comment.UserName = HttpContext.User.Identity.Name;
                //db.Entry(comment).Property(c => c.DateTime).IsModified = false;  //doesn't work
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Resources", new { id = ResId });
            }
            //ViewBag.ResourceId = new SelectList(db.Resources, "ResourceID", "Title", comment.ResourceID);
            return View(comment);
        }*/

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id, int ResId)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "Resources", new { id = ResId });
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
