using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using UpdateTheLibrary.Models;

namespace UpdateTheLibrary.Controllers
{
    public class BooksController : ApiController
    {
        private LibraryContext db = new LibraryContext();

		// GET: api/Books  (Get all books)
		public IQueryable<Book> GetBooks()
        {
            return db.Books;
        }

        // GET: api/Books/5  (Get 1 book)
        [ResponseType(typeof(Book))]
        public IHttpActionResult GetBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

		[HttpGet]
		public IHttpActionResult GetBooksByStatus(bool ischeckedout)
		{
			var books = db.Books.Where(w => w.IsCheckedOut == ischeckedout);
			return Ok(books);
		}

		// PUT: api/Books/5 (update 1 book)
		[ResponseType(typeof(void))]
        public IHttpActionResult PutBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
			/*
            if (id != book.Id)
            {
                return BadRequest();
            }
			*/
            db.Entry(book).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

		[HttpPut]
		public IHttpActionResult CheckInOrOut(int id, string action)
		{
			var message = String.Empty;
			action = action.ToLower();

			Book book = db.Books.Find(id);
			if (book == null)
			{
				return NotFound();
			}
			if (action.Equals("out") && book.IsCheckedOut)
			{
				message = $"Status: {book.Title} is already checked Out, DueDate: {book.DueBackDate}";
			}
			else
			{
				book.IsCheckedOut = !book.IsCheckedOut;
				if (action.Equals("out"))
				{
					book.LastCheckedOutDate = DateTime.Today;
					book.DueBackDate = DateTime.Today.AddDays(10);
				}
				db.SaveChanges();
				message = $"Status: {book.Title} has been checked {action}";

			}
			return Ok(message);
		}


		// POST: api/Books  (Add 1 book)
		[ResponseType(typeof(Book))]
        public IHttpActionResult PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5 (Delete 1 book)
        [ResponseType(typeof(Book))]
        public IHttpActionResult DeleteBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.Id == id) > 0;
        }
    }
}