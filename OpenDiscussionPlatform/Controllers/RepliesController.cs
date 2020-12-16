using Microsoft.AspNet.Identity;
using OpenDiscussionPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenDiscussionPlatform.Controllers
{
    public class RepliesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Replies
        public ActionResult Index()
        {
            return View();
        }


        [HttpDelete]
        [Authorize(Roles = "User,Moderator,Admin")]
        public ActionResult Delete(int id)
        {
            Reply repl= db.Replies.Find(id);
            if (repl.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            { 
                db.Replies.Remove(repl);
                db.SaveChanges();
                return Redirect("/ForumPosts/Show/" + repl.ForumPostId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "ForumPosts");
            }

        }



        [Authorize(Roles = "User,Moderator,Admin")]
        public ActionResult Edit(int id)
        {
            Reply repl = db.Replies.Find(id);

            if (repl.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(repl);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "ForumPosts");
            }
        }


        [HttpPut]
        [Authorize(Roles = "User,Moderator,Admin")]
        public ActionResult Edit(int id, Reply requestReply)
        {
            try
            {
                Reply repl = db.Replies.Find(id);

                if (repl.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    if (TryUpdateModel(repl))
                    {
                        repl.Content = requestReply.Content;
                        db.SaveChanges();
                    }
                    return Redirect("/ForumPosts/Show/" + repl.ForumPostId);
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                    return RedirectToAction("Index", "ForumPosts");
                }
            }
            catch (Exception e)
            {
                return View(requestReply);
            }
        }



    }
}