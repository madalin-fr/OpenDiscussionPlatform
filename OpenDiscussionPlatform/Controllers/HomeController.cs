using OpenDiscussionPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenDiscussionPlatform.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "ForumPosts");
            }

            var forumposts= from forumpost in db.ForumPosts
                           select forumpost;

            ViewBag.FirstForumPost = forumposts.First();
            ViewBag.ForumPosts = forumposts.OrderBy(o => o.Date).Skip(1).Take(2);

            return View();
        }


    }
}