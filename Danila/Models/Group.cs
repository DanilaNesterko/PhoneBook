using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Danila.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BooksList { get; set; }
        public User User { get; set; }
        public ICollection<Book> Books { get; set; }

        public Group() {
            Books = new List<Book>();
        }
    }
}