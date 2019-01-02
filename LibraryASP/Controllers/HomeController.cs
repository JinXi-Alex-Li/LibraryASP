using LibraryASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryASP.Controllers
{
    public class HomeController : Controller
    {
        private LibraryDBEntities db = new LibraryDBEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View(db.Libraries.ToList());
        }

        // GET: Home/Details/5
        public ActionResult Details(string ISBN)
        {
            Library book = db.Libraries.Find(ISBN);
            return View(book);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            if (!ModelState.IsValid) return View(); // If something is wrong

            try
            {
                string isbn = collection["ISBN"];
                string title = collection["Title"];
                string author = collection["Author"];
                string description = collection["Description"];

                var result = db.Libraries.Where(b => b.ISBN == isbn).FirstOrDefault();

                Library NewBook = new Library
                {
                    ISBN = isbn,
                    Title = title,
                    Author = author,
                    Description = description
                };

                if (result == null) // If the book does not exist, then insert
                {
                    db.Libraries.Add(NewBook);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else // Otherwise, prompt for changing the content of the entry
                {
                    ViewBag.ISBN = isbn;
                    ViewBag.Title = title;
                    ViewBag.Author = author;
                    ViewBag.Description = description;
                    return RedirectToAction("Option");
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(string ISBN)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(string ISBN, FormCollection collection)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                string title = collection["Title"];
                string author = collection["Author"];
                string desc = collection["Description"];

                var result = db.Libraries.Where(b => b.ISBN == ISBN).FirstOrDefault();

                if (result != null) // if the ISBN exists, then modify, otherwise, do nothing
                {
                    result.Title = title;
                    result.Author = author;
                    result.Description = desc;
                    db.SaveChanges();
                }


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(string ISBN)
        {
            var result = db.Libraries.Where(b => b.ISBN == ISBN).FirstOrDefault();

            return View(result);
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(string ISBN, FormCollection collection)
        {
            try
            {
                var result = db.Libraries.Where(b => b.ISBN == ISBN).FirstOrDefault();

                if (result != null) // if the ISBN exists, then modify, otherwise, do nothing
                {
                    db.Libraries.Remove(result);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Option()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string Input, string Criterion)
        {
            if (Criterion == "ISBN")
            {
                var result = db.Libraries.Where(b => b.ISBN == Input);
                return View(result);

            }
            else if (Criterion == "Title")
            {
                var result = db.Libraries.Where(b => b.Title == Input);
                return View(result);
            }

            else if (Criterion == "Author")
            {
                var result = db.Libraries.Where(b => b.Author == Input);
                return View(result);
            }
            return View();
        }
    }
}
