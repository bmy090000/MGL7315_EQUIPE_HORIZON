using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MaBiblio.DAL;
using MaBiblio.Models;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace MaBiblio.Controllers
{
    public class BookVM
    {
        public List<SelectListItem> AuthNames { get; set; }
        public Book Book { get; set; }


    }
    public class BookController : Controller
    {
        private BiblioContext db = new BiblioContext();

        // GET: Book
        public ActionResult Index()
        {
            return List(null);
        }

        public ActionResult List(string filter)
        {
            IQueryable<Book> books;

            if (filter == null)
                books = db.Books.Include(bb => bb.Author).OrderBy(d => d.ID);
            else
            {
                books = db.Books.Where(b => b.Title.ToLower().Contains(filter.ToLower())).Include(bb => bb.Author).OrderBy(d => d.ID);
                ViewBag.FindFilter = filter;
            }
            return View(books.ToList());
        }
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult IndexPost()
        {
            if (Request["clearFind"] != null)
                return Index();

            return List(Request["findFilter"]);
        }


        public ActionResult Create()
        {   IQueryable<Author> a = db.Authors.OrderBy(d => d.ID);
            var vm = new BookVM();
            vm.AuthNames = new List<SelectListItem>();
            foreach (Author da in a.ToList())
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = da.FullName,
                    Value = da.ID.ToString(),
                    Selected = false
                };

                vm.AuthNames.Add(selectList);
            }
            return View("Create",vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,ISBN,Price")]Book book)
        {
            try
            {
                
                var au = int.Parse(Request["Author"]);
                book.Author=db.Authors.Find(au);
                if (ModelState.IsValid)
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            
            return View(book);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            IQueryable<Author> a = db.Authors.OrderBy(d => d.ID);
            var vm = new BookVM();
            vm.Book = book;
            vm.AuthNames = new List<SelectListItem>();
            foreach (Author da in a.ToList())
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = da.FullName,
                    Value = da.ID.ToString(),
                    Selected = da.ID == book.Author.ID
                };

                vm.AuthNames.Add(selectList);
            }
            return View(vm);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "Title,ISBN,Price")]Book book, int?id)
        {

            var bookToUpdate = db.Books.Find(id);
            db.Books.Remove(bookToUpdate);
            db.SaveChanges();
            var au = int.Parse(Request["Author"]);
            book.Author = db.Authors.Find(au);
            db.Books.Add(book);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    
        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
