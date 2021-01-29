using Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.Entity.Migrations;
using System.Net.Mail;
using System.Net;

namespace Final.Controllers
{
     public class HomeController : Controller
     {
          PageContext pageContextDB = new PageContext();

          SoccerContext soccerDB = new SoccerContext();
          public ActionResult Index(int page = 1)
          {
               int pageSize = 5;
               IEnumerable<New> newsPerPages = pageContextDB.News.
                    Include(n => n.picture).
                    OrderBy(x => x.Id).
                    Skip((page - 1) * pageSize).
                    Take(pageSize).ToList();

               PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = pageContextDB.News.Count() };
               IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, News = newsPerPages };

               return View(ivm);
          }

          public ActionResult Clasament()
          {

               return PartialView();
          }

          [HttpGet]
          public ActionResult SendEmail()
          {
               return View();
          }

          [HttpPost]
          public string SendEMail()
          {
               string from = Request["from"];
               string subject = Request["subject"];
               string body = Request["body"].ToString();
               

               using (MailMessage mail = new MailMessage())
               {
                    mail.From = new MailAddress("daniel.verhovetchi@ati.utm.md");
                    mail.To.Add("info_barca@mail.ru");
                    mail.Subject = subject;
                    mail.Body = $"<b>From:</b>{from}<br><b>Message:</b>{body}";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587))
                    {
                         smtp.Credentials = new NetworkCredential("daniel.verhovetchi@ati.utm.md", "1111Nuttertools.");
                         smtp.EnableSsl = true;
                         smtp.Send(mail);
                    }
               }

               return "<b style='color: green; font-size: 40px'>Message was sent successfull!!!<b>";
          }

          public ActionResult About()
          {
               return View();
          }

          public ActionResult Privacy()
          {
               return View();
          }

          public ActionResult Team(int id)
          {
               var players = soccerDB.Database.SqlQuery<Player>("SELECT * FROM Players where TeamId=" + id);

               ViewBag.Players = players;

               return View();
          }




          //public ActionResult Test()
          //{
          //     SoccerContext soccer = new SoccerContext();
          //}

          //SoccerContext db2 = new SoccerContext();

          //public ActionResult Index2()
          //{
          //     var teams = db2.Teams.Include(t => t.picture);

          //     return View(teams.ToList());
          //}

          //protected override void Dispose(bool disposing)
          //{
          //     db.Dispose();
          //     base.Dispose(disposing);
          //}
     }
}