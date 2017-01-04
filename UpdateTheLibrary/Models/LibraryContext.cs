using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UpdateTheLibrary.Models
{
	public class LibraryContext : DbContext
	{
		public LibraryContext() : base("name=DefaultConnection")
		{

		}

		public DbSet<Book> Books { get; set; }
	}
}