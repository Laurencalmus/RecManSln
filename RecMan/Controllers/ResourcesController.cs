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
using System.IO;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity;

namespace RecMan.Controllers
{
    public class ResourcesController : Controller
    {
        private RecManContext db = new RecManContext();

        // GET: Resources
        //[Authorize]
        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.SourceSortParm = String.IsNullOrEmpty(sortOrder) ? "source_desc" : "";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.TopicSortParm = sortOrder == "Topic" ? "topic_desc" : "Topic";
            ViewBag.LevelSortParm = sortOrder == "Level" ? "level_desc" : "Level";
            ViewBag.FocusSortParm = sortOrder == "Focus" ? "focus_desc" : "Focus";
            var resources = from Resources in db.Resources
                            select Resources;
            if (!String.IsNullOrEmpty(searchString))
            {
                resources = resources.Where(model => model.Title.Contains(searchString)
                                       || model.Source.Contains(searchString)
                                       || (model.Level.ToString().Contains(searchString)
                                       || (model.Focus.ToString()).Contains(searchString)  //how do i query enum?
                                       || model.Topic.Contains(searchString)));
            }

            switch (sortOrder)
            {
                case "Level":
                    resources = resources.OrderBy(r => r.Level);
                    break;
                case "level_desc":
                    resources = resources.OrderByDescending(r => r.Level);
                    break;
                case "Focus":
                    resources = resources.OrderBy(r => r.Focus);
                    break;
                case "focus_desc":
                    resources = resources.OrderByDescending(r => r.Focus);
                    break;
                case "source_desc":
                    resources = resources.OrderByDescending(r => r.Source);
                    break;
                case "Title":
                    resources = resources.OrderBy(r => r.Title);
                    break;
                case "title_desc":
                    resources = resources.OrderByDescending(r => r.Title);
                    break;
                case "Topic":
                    resources = resources.OrderBy(r => r.Topic);
                    break;
                case "topic_desc":
                    resources = resources.OrderByDescending(r => r.Topic);
                    break;
                default:
                    resources = resources.OrderBy(r => r.Level);
                    break;
            }

            //if (User.Identity.IsAuthenticated)   //or Request.IsAuthenticated     If I do that on an Action decorated with [Authorize] it works fine, 
            //{
                return View(resources.ToList());       //however if I do that on this Action (not decorated with [Authorize]) it is always false, 
            /*}
            //regardless of whether I am logged in or not. 
            else
            {
                RedirectToAction("Login", "Account");
            }*/
        }


        // GET: Resources/Details/5
        public ActionResult Details(int? id)
        {
            /*Boolean userTest = ViewBag.Userr;
            if (User.Identity.GetUserId()== db.Model.Comment.ID )
                { userTest = true}*/
            ViewBag.Userr = User.Identity.GetUserId();
            ViewBag.UserrName = HttpContext.User.Identity.Name;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            /*Resource resource = db.Resources.Find(id);*/
            Resource resource = db.Resources.Include(r => r.Files).SingleOrDefault(r => r.ResourceID == id);
            var PdfFile = db.Files.Where(p => p.ResourceId == id).Select(f => f.Content).FirstOrDefault();

            ViewData["PDF"] = PdfFile;

            var comments = db.Comments.Where(c => c.ResourceID == id);

            if (resource == null)
            {
                return HttpNotFound();
            }

            return View(resource);
            //return RedirectToAction("Create", "Comments");
        }

        // POST: Resources/Details             //for like counter
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id)
        {
            var resource = db.Resources.Find(id);
            //ViewBag.Userr = User.Identity.GetUserId();

            //if (ModelState.IsValid)
            //{
            Like like = new Like();
                like.ResourceId = id;
                like.UserId = User.Identity.GetUserId();
                like.Liked = true;
            db.Likes.Add(like);
            db.SaveChanges();
            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });

            /*var userlike = db.Likes.Where(l => l.UserId == like.UserId && l.ResourceId == id);

                if (userlike.Count() == 0)
                {
                    db.Likes.Add(like);
                    db.SaveChanges();
                }
                else if (userlike.Count() == 1)
                {
                    var likeDel = db.Likes.FirstOrDefault(l => l.UserId == like.UserId && l.ResourceId == id);
                    db.Likes.Remove(likeDel);
                    db.SaveChanges();
                }
                else 
                {
                    db.Likes.Add(like);
                    like.Liked = true;
                    db.SaveChanges();
                }
                //db.Entry(resource).State = EntityState.Modified;
                //db.SaveChanges(); 
            //}
            List<Like> resourceLikes = db.Likes.Where(r => r.ResourceId == id).ToList();    //set likeCount  //all the likes for that resource(id)
            int numberLikes = resourceLikes.Count();  //int numberLikes = # of likes (from above)

            Resource thisResource = db.Resources.Include(r => r.Files).SingleOrDefault(r => r.ResourceID == id); 
            //var PdfFile = db.Files.Where(p => p.ResourceId == id).Select(f => f.Content).FirstOrDefault(); //What is this?

            thisResource.LikeCount = numberLikes;   //set like count of this resource to reflect new like  ???
            db.SaveChanges();

            return View(resource);*/
        }

        /*public string UnlikeThis(int id)
        {
            Article art = DB.Articles.FirstOrDefault(x => x.ID == id);
            if (User.Identity.IsAuthenticated || Session["Username"] != null)
            {
                var username = User.Identity.Name;
                Member m = DB.Members.FirstOrDefault(x => x.Username == username);
                Like l = DB.Likes.FirstOrDefault(x => x.ArticleID == id && x.UserID == m.ID);
                art.Likes--;
                DB.Likes.Remove(l);
                DB.SaveChanges();

            }
            return art.Likes.ToString();
        }*/


        // GET: Resources/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Resources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "ResourceID,Title,Source,Level,Focus,Topic,Content,File,FileType,ContentType,UserId")] Resource resource, HttpPostedFileBase upload)
        {
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                            resource.UserName = HttpContext.User.Identity.Name;
                        }
                        if (upload != null && upload.ContentLength > 0)
                        {
                            var avatar = new Models.File
                            {
                                FileName = System.IO.Path.GetFileName(upload.FileName),
                                FileType = FileType.Avatar,
                                ContentType = upload.ContentType
                            };
                            using (var reader = new System.IO.BinaryReader(upload.InputStream))
                            {
                                avatar.Content = reader.ReadBytes(upload.ContentLength);
                            }
                            resource.Files = new List<Models.File> { avatar };
                        }
                        db.Resources.Add(resource);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
                return View(resource);
            }
        }


        // GET: Resources/Edit/5
        [Authorize(Roles = "Manager, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resources.Include(r => r.Files).SingleOrDefault(r => r.ResourceID == id);
            var PdfFile = db.Files.Where(p => p.ResourceId == id).Select(s => s.Content).FirstOrDefault();

            ViewData["PDF"] = PdfFile;
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResourceID,Title,Source,Level,Focus,Topic,Content,File,FilePath,ContentType")] int? id, HttpPostedFileBase upload)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var resourceToUpdate = db.Resources.Find(id);
            if (TryUpdateModel(resourceToUpdate, "",
                new string[] { "Title", "Source", "Level", "Focus", "Topic", "Content" }))
            {
                try
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        if (resourceToUpdate.Files.Any(f => f.FileType == FileType.Avatar))
                        {
                            db.Files.Remove(resourceToUpdate.Files.First(f => f.FileType == FileType.Avatar));
                        }
                        var avatar = new Models.File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }
                    }
                    db.Entry(resourceToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Resources/Delete/5
        [Authorize(Roles = "Manager, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Resource resource = db.Resources.Find(id);
            db.Resources.Remove(resource);
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

