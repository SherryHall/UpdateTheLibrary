using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UpdateTheLibrary.Models
{
	public class Book
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public string Genre { get; set; }
		public int? Year_Published { get; set; }
		public bool IsCheckedOut { get; set; }
		public DateTime? LastCheckedOutDate { get; set; }
		public DateTime? DueBackDate { get; set; }
	}
}

