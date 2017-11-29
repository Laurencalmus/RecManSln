using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecMan.Models;
using RecMan.DAL;

namespace RecMan.Controllers
{
    public class FileController : Controller
    {
        private RecManContext db = new RecManContext();
        //
        // GET: /File/
        public ActionResult Index(int id)
        {
            var fileToRetrieve = db.Files.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}