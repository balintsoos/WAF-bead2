using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class BooksController : BaseController
    {
        private LibraryDBEntities db = new LibraryDBEntities();

        // GET: Books
        [Authorize]
        public ActionResult Index(string searchString)
        {
            List<BookViewModel> resultBooks = new List<BookViewModel>();

            var books = db.Book.Include("Copy").ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString)).ToList();
            }

            foreach (var book in books)
            {
                int copyCount = book.Copy.Count();
                int availableCount = copyCount;

                foreach (var copy in db.Copy.Include("Rent").Where(c => c.ISBN == book.ISBN))
                {
                    if (copy.Rent.Where(r => r.ReturnDate == null).Count() > 0)
                    {
                        availableCount--;
                    }
                }

                resultBooks.Add(new BookViewModel
                {
                    Title = book.Title,
                    Author = book.Author,
                    ISBN = book.ISBN,
                    Year = book.Year,
                    copyCount = copyCount,
                    availableCount = availableCount
                });
            }

            return View(resultBooks);
        }

        // GET: Books/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            List<CopyViewModel> copies = new List<CopyViewModel>();

            foreach (var copy in db.Copy.Include("Rent").Where(c => c.ISBN == id))
            {
                String status;

                var rent = copy.Rent.Where(r => r.ReturnDate == null).FirstOrDefault();

                if (rent == null)
                {
                    status = "In Stock";
                }
                else
                {
                    status = rent.EndDate.ToShortDateString();
                }

                copies.Add(new CopyViewModel
                {
                    Status = status,
                    Condition = copy.Condition,
                });
            }

            ViewBag.ISBN = id;

            return View(copies);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ISBN,Title,Author,Year")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Book.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ISBN,Title,Author,Year")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Book book = db.Book.Find(id);
            db.Book.Remove(book);
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
