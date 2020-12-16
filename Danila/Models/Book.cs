using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Danila.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Mail { get; set; }
        public string Telefon { get; set; }
		public ICollection<Group> Groups { get; set; }

		public Book()
		{
			Groups = new List<Group>();
		}
	}
}