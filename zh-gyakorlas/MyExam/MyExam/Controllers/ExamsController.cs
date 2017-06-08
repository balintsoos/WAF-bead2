using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyExam.Models;

namespace MyExam.Controllers
{
    public class ExamsController : Controller
    {
        private ExamDBEntities db = new ExamDBEntities();

        // GET: Exams
        public ActionResult Index()
        {
            var exam = db.Exam.Include(e => e.Student).Include(e => e.Task).Include(e => e.Task1).Include(e => e.Task2).Include(e => e.Task3);
            return View(exam.ToList());
        }

        // GET: Exams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exam.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // GET: Exams/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Student, "UserId", "UserId");
            ViewBag.T1 = new SelectList(db.Task, "Id", "Question");
            ViewBag.T2 = new SelectList(db.Task, "Id", "Question");
            ViewBag.T3 = new SelectList(db.Task, "Id", "Question");
            ViewBag.T4 = new SelectList(db.Task, "Id", "Question");
            return View();
        }

        // POST: Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,StartTime,EndTime,T1,A1,T2,A2,T3,A3,T4,A4")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Exam.Add(exam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Student, "UserId", "UserId", exam.StudentId);
            ViewBag.T1 = new SelectList(db.Task, "Id", "Question", exam.T1);
            ViewBag.T2 = new SelectList(db.Task, "Id", "Question", exam.T2);
            ViewBag.T3 = new SelectList(db.Task, "Id", "Question", exam.T3);
            ViewBag.T4 = new SelectList(db.Task, "Id", "Question", exam.T4);
            return View(exam);
        }

        // GET: Exams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exam.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Student, "UserId", "UserId", exam.StudentId);
            ViewBag.T1 = new SelectList(db.Task, "Id", "Question", exam.T1);
            ViewBag.T2 = new SelectList(db.Task, "Id", "Question", exam.T2);
            ViewBag.T3 = new SelectList(db.Task, "Id", "Question", exam.T3);
            ViewBag.T4 = new SelectList(db.Task, "Id", "Question", exam.T4);
            return View(exam);
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,StartTime,EndTime,T1,A1,T2,A2,T3,A3,T4,A4")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Student, "UserId", "UserId", exam.StudentId);
            ViewBag.T1 = new SelectList(db.Task, "Id", "Question", exam.T1);
            ViewBag.T2 = new SelectList(db.Task, "Id", "Question", exam.T2);
            ViewBag.T3 = new SelectList(db.Task, "Id", "Question", exam.T3);
            ViewBag.T4 = new SelectList(db.Task, "Id", "Question", exam.T4);
            return View(exam);
        }

        // GET: Exams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exam.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exam exam = db.Exam.Find(id);
            db.Exam.Remove(exam);
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
