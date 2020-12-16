using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Danila.Models
{
    public class Context : DbContext
    {
        public Context() : base("Db") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}