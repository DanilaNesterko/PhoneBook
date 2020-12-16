using Danila.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Danila.Controllers
{
    public class HomeController : Controller
    {
        private Context _db = new Context();
        ///////////////////////////////////Books///////////////////////////////////
        [HttpGet]
        public ActionResult Index(int? Id, string Name, string Lastname, string Mail, string Telefon)
        {
            bool starte = false;
            var books = "";
            if (Id != null)
            {
                books += "Id LIKE '%"+Id+"%' ";
                starte = true;
            }
            if (Name != null)
            {
                books += "Name LIKE '%" + Name + "%' ";
                starte = true;
            }
            if (Lastname != null)
            {
                books += "Lastname LIKE '%" + Lastname + "%' ";
                starte = true;
            }
            if (Mail != null)
            {
                books += "Mail LIKE '%" + Mail + "%' ";
                starte = true;
            }
            if (Telefon != null)
            {
                books += "Telefon LIKE '%" + Telefon + "%' ";
                starte = true;
            }
            if (starte)
            {
                books = "WHERE " + books;
            }
            ViewBag.Books = _db.Books.SqlQuery("SELECT * FROM [Books] " + books).ToList();
            return View();
        }
        public ActionResult CreateBook()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBook(Book objProject)
        {
            var project = new Book
            {
                Name = objProject.Name,
                Lastname = objProject.Lastname,
                Mail = objProject.Mail,
                Telefon = objProject.Telefon
            };
            _db.Books.Add(project);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditBook(int id)
        {
            ViewBag.Book = _db.Books.Where(a => a.Id == id).FirstOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook(Book objProject)
        {
            Book project = _db.Books.Where(a => a.Id == objProject.Id).FirstOrDefault();
            if (project != null)
            {
                project.Name = objProject.Name;
                project.Lastname = objProject.Lastname;
                project.Telefon = objProject.Telefon;
                project.Mail = objProject.Mail;
            }
            _db.Entry(project).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteBook(int? id)
        {
            Book book = _db.Books.Where(a => a.Id == id).FirstOrDefault();
            if (book != null)
            {
                _db.Books.Remove(book);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        ///////////////////////////////////Groups///////////////////////////////////
        public ActionResult Groups()
        {
            if (Session["token"] == null) { return RedirectToAction("Login"); }
            else if (Session["admin"].ToString() == "1")
            {
                ViewBag.Groups = _db.Groups.Include("Books").ToList();
            }
            else
            {
                User user = _db.Users.Find(Convert.ToInt32(Session["token"] + ""));
                ViewBag.Groups = _db.Groups.Include("Books").Where(u => u.User.Id == user.Id).ToList();
            }
            return View();
        }
        public ActionResult CreateGroup()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGroup(Group objProject)
        {
            if (Session["token"] == null) { return RedirectToAction("Login"); }
            User user = _db.Users.Find(Convert.ToInt32(Session["token"] + ""));

            var project = new Group
            {
                Title = objProject.Title,
                User = user
            };
            _db.Groups.Add(project);
            _db.SaveChanges();
            return RedirectToAction("Groups");
        }
        [HttpGet]
        public ActionResult EditGroup(int id)
        {
            ViewBag.Books = _db.Books.ToList();
            ViewBag.Group = _db.Groups.Where(a => a.Id == id).FirstOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroup(Group objProject)
        {
            Group project = _db.Groups.Where(a => a.Id == objProject.Id).FirstOrDefault();
            if (project != null)
            {
                project.Title = objProject.Title;
                project.Books.Clear();
                foreach (var item in objProject.BooksList.Trim().Split(','))
                {
                    project.Books.Add(_db.Books.Where(a => a.Name == item).FirstOrDefault());
                }
            }
            _db.Entry(project).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Groups");
        }
        [HttpGet]
        public ActionResult DeleteGroup(int? id)
        {
            Group group = _db.Groups.Include("Books").Where(a => a.Id == id).FirstOrDefault();
            if (group != null)
            {
				group.Books.Clear();
				_db.Groups.Remove(group);
                _db.SaveChanges();
            }
            return RedirectToAction("Groups");
        }
        ///////////////////////////////////Users///////////////////////////////////
        public ActionResult Login()
        {
            if (Session["token"] != null) { return RedirectToAction("Index"); }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User objUser)
        {
            var user = _db.Users.Where(a => a.Username.Equals(objUser.Username) && a.Password.Equals(objUser.Password)).FirstOrDefault();
            if (user != null)
            {
                Session["token"] = user.Id.ToString();
                Session["username"] = user.Username.ToString();
                Session["admin"] = user.Administator.ToString();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Register()
        {
            if (Session["token"] != null) { return RedirectToAction("Index"); }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User objUser)
        {
            var user = _db.Users.Where(a => a.Username.Equals(objUser.Username)).FirstOrDefault();
            if (user == null)
            {
                var newUser = new User { Username = objUser.Username, Password = objUser.Password };
                Session["token"] = newUser.Id.ToString();
                Session["username"] = newUser.Username.ToString();
                _db.Users.Add(newUser);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Logout()
        {
            if (Session["token"] == null) { return RedirectToAction("Login"); }
            else { Session["token"] = null; return RedirectToAction("Login"); }
        }
    }
}