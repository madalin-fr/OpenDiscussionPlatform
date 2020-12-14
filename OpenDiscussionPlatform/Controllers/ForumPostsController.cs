using OpenDiscussionPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenDiscussionPlatform.Controllers
{
    public class ForumPostsController : Controller
    {

        // GET: Lista tuturor postarilor
        [ActionName("listare")]
        [OutputCache(Duration = 30)]
        public ActionResult Index()
        {
            ForumPost[] forumPosts = GetForumPosts();

            // Adaugam array-ul de postari in view
            ViewBag.ForumPosts = forumPosts;

            return View("Index");
        }



        // GET: Vizualizarea unei postari
        public ActionResult Show(int id)
        {
            ForumPost[] forumPosts = GetForumPosts();

            try
            {
                ViewBag.ForumPost = forumPosts[id];
                return View();
            }

            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }

        }

        // GET: Afisarea formularului de creare pentru o postare
        public ActionResult New()
        {
            return View();
        }

        // POST: Trimiterea datelor despre postare catre server pentru creare
        [HttpPost]
        public ActionResult New(ForumPost forumPost)
        {
            // ... cod creare postare ...
            return View("NewPostMethod");
        }

        // GET: Afisarea datelor unei postari pentru editare
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }


        // PUT: Trimiterea modificarilor facute catre server si sa le salvam
        [HttpPut]
        public ActionResult Edit(ForumPost forumPost)
        {
            // ... cod update postare...
            return View("EditPutMethod");
        }

        // DELETE: Functionalitate de stergere a unei postari
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            // ... cod stergere postare ...
            return View("DeleteMethod");
        }


        [NonAction]
        public ForumPost[] GetForumPosts()
        {
            // Instantiem un array de postari
            ForumPost[] forumPosts = new ForumPost[3];

            // Cream postarile
            for (int i = 0; i < 3; i++)
            {
                ForumPost forumPost = new ForumPost();
                forumPost.Id = i;
                forumPost.Title = "Postare " + (i + 1).ToString();
                forumPost.Content = "Continut postare " + (i + 1).ToString();
                forumPost.Date = DateTime.Now;

                // Adaugam postarea in array
                forumPosts[i] = forumPost;
            }
            return forumPosts;
        }





    }
}