using OpenDiscussionPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenDiscussionPlatform.Controllers
{
    public class UsersController : Controller
    {
        // GET: Lista users
        public ActionResult Index()
        {
            return View();
        }

        // GET: Vizualizarea unui user
        public ActionResult Show(int id)
        {
            return View();
        }

        // GET: Formularul de creare a unui user nou
        public ActionResult New()
        {
            return View();
        }

        // POST: Se trimit datele user-ului catre server pentru creare
        [HttpPost]
        public ActionResult New(User user)
        {
            int id = 123;
            return Redirect("/users/" + id);
        }

        //[NonAction]
        //public User GetUser(int id)
        //{
        //    return;
        //}



        // PUT: Editare user
        [HttpPut]
        public ActionResult Edit(User ID)
        {
            // modify user code here


            return Redirect("/users/" + ID);
            //return RedirectToRoute("users_show", new { id = ID });
        }


        [HttpDelete]
        public ActionResult Delete(int id)
        {
            // cod stergere user din baza de date
            // redirectionam browserul la pagina index a user-ilor
            return RedirectToRoute("users_index");
        }

    }
}