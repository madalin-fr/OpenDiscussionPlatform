using Microsoft.AspNet.Identity;
using OpenDiscussionPlatform.Models;
using System;
using System.Collections.Generic;
using Microsoft.Security.Application;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenDiscussionPlatform.Controllers
{
    public class ForumPostsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        private int _perPage = 3;

        /// GET: ForumPost
        [Authorize(Roles = "User,E-ditor,Admin")]
        public ActionResult Index()
        {
            var forumposts = db.ForumPosts.Include("Category").Include("User").OrderBy(a => a.Date);
            var totalItems = forumposts.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }

            var paginatedForumPosts= forumposts.Skip(offset).Take(this._perPage);

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            //ViewBag.perPage = this._perPage;
            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.ForumPosts = paginatedForumPosts;

            if (User.IsInRole("User"))
                ViewBag.AfisareProfiluri = false;
            else
                ViewBag.AfisareProfiluri = true;

            return View();
        }



        [Authorize(Roles = "User,E-ditor,Admin")]
        public ActionResult Show(int id)
        {
            ForumPost forumPost = db.ForumPosts.Find(id);

            SetAccessRights();

            /*
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Editor") || User.IsInRole("Admin"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            */

            if (User.IsInRole("User"))
                ViewBag.AfisareProfiluri = false;
            else
                ViewBag.AfisareProfiluri = true;


            return View(forumPost);

        }

        [HttpPost]
        [Authorize(Roles = "User,E-ditor,Admin")]
        public ActionResult Show(Reply repl)
        {
            repl.Date = DateTime.Now;
            repl.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Replies.Add(repl);
                    db.SaveChanges();
                    return Redirect("/ForumPosts/Show/" + repl.ForumPostId);
                }

                else
                {
                    ForumPost a = db.ForumPosts.Find(repl.ForumPostId);

                    SetAccessRights();

                    return View(a);
                }

            }

            catch (Exception e)
            {
                ForumPost a = db.ForumPosts.Find(repl.ForumPostId);

                SetAccessRights();

                return View(a);
            }

        }

        [Authorize(Roles = "User,E-ditor,Admin")]
        public ActionResult New()
        {
            ForumPost forumPost = new ForumPost();
            

            // preluam lista de categorii din metoda GetAllCategories()
            forumPost.Categ = GetAllCategories();

            // Preluam ID-ul utilizatorului curent
            forumPost.UserId = User.Identity.GetUserId();

            return View(forumPost);
        }


        [HttpPost]
        [Authorize(Roles = "User,E-ditor,Admin")]
        [ValidateInput(false)]
        public ActionResult New(ForumPost forumPost)
        {
            forumPost.Date = DateTime.Now;
            forumPost.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    forumPost.Content = Sanitizer.GetSafeHtmlFragment(forumPost.Content);

                    db.ForumPosts.Add(forumPost);
                    db.SaveChanges();
                    TempData["message"] = "Postarea a fost adaugata";
                    return RedirectToAction("Index");
                }
                else
                {
                    forumPost.Categ = GetAllCategories();
                    return View(forumPost);
                }
            }
            catch (Exception e)
            {
                forumPost.Categ = GetAllCategories();
                return View(forumPost);
            }
        }


        [Authorize(Roles = "E-ditor,Admin,User")]
        public ActionResult Edit(int id)
        {
            ForumPost forumPost = db.ForumPosts.Find(id);
            forumPost.Categ = GetAllCategories();

            if (forumPost.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(forumPost);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei postari care nu va apartine";
                return RedirectToAction("Index");
            }
        }


        [HttpPut]
        [Authorize(Roles = "E-ditor,Admin,User")]
        [ValidateInput(false)]
        public ActionResult Edit(int id, ForumPost requestForumPost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ForumPost forumPost = db.ForumPosts.Find(id);

                    if (forumPost.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(forumPost))
                        {
                            forumPost.Title = requestForumPost.Title;

                            requestForumPost.Content = Sanitizer.GetSafeHtmlFragment(requestForumPost.Content);

                            forumPost.Content = requestForumPost.Content;
                            forumPost.CategoryId = requestForumPost.CategoryId;
                            db.SaveChanges();
                            TempData["message"] = "Postarea a fost modificata";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei postari care nu va apartine";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    requestForumPost.Categ = GetAllCategories();
                    return View(requestForumPost);
                }
            }
            catch (Exception e)
            {
                requestForumPost.Categ = GetAllCategories();
                return View(requestForumPost);
            }
        }



        [HttpDelete]
        [Authorize(Roles = "E-ditor,Admin,User")]
        public ActionResult Delete(int id)
        {
            ForumPost forumPost = db.ForumPosts.Find(id);

            if (forumPost.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.ForumPosts.Remove(forumPost);
                db.SaveChanges();
                TempData["message"] = "Postarea a fost stearsa";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti o postarecare nu va apartine";
                return RedirectToAction("Index");
            }
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







        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            /*
            foreach (var category in categories)
            {
                var listItem = new SelectListItem();
                listItem.Value = category.CategoryId.ToString();
                listItem.Text = category.CategoryName.ToString();

                selectList.Add(listItem);
            }*/

            // returnam lista de categorii
            return selectList;
        }




        private void SetAccessRights()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("E-ditor") || User.IsInRole("Admin") || User.IsInRole("User"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }


    }
}