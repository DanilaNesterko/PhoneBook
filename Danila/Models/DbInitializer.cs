using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Danila.Models
{
    public class DbInitializer : CreateDatabaseIfNotExists<Context>
    {
        protected override void Seed(Context db)
        {
            var user1 = new User
            {
                Username = "Danila",
                Password = "danilaPass",
                Administator = 1
            };
            var user2 = new User
            {
                Username = "Ivan",
                Password = "ivanPass",
                Administator = 0
            };

            db.Users.Add(user1);
            db.Users.Add(user2);

            var book1 = new Book
            {
                Name = "Danila",
                Lastname = "Nesterko",
                Telefon = "+372 23489813",
                Mail = "Danila@gmail.com"
            };
            var book2 = new Book
            {
                Name = "Ivan",
                Lastname = "Timofeev",
                Telefon = "+372 02978247",
                Mail = "Danila@gmail.com"
            };

            db.Books.Add(book1);
            db.Books.Add(book2);

            var group1 = new Group
            {
                Title = "Friends Danila",
                User = user1
            };
            group1.Books.Add(book2);
            var group2 = new Group
            {
                Title = "Friends Ivan",
                User = user2
            };
            group2.Books.Add(book1);

            db.Groups.Add(group1);
            db.Groups.Add(group2);

            base.Seed(db);
        }
    }
}