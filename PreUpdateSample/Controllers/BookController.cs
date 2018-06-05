using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using PreUpdateSample.Models;

namespace PreUpdateSample.Controllers
{
    public class BookController : Controller
    {
        private readonly ISessionFactory _sessionFactory;

        public BookController(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        // GET: Book
        public ActionResult Index()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var books = session.Query<Book>().ToList();
               return View(books);
            }
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            try
            {
                using(var session = _sessionFactory.OpenSession())
                {
                    session.Save(book);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var book = session.Get<Book>(id);
               return View(book);
            }
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Book bookData)
        {
            try
            {
                using (var session = _sessionFactory.OpenSession())
                {
                    var book = session.Get<Book>(id);
                    book.Name = bookData.Name;
                    session.Flush();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}