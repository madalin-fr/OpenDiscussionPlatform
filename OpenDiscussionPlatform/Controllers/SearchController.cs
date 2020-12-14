using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenDiscussionPlatform.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }


        [ActionName("query")]
        public ActionResult SearchSequence(string keywords)
        {
            if(GetArticleIdByKeyword(keywords) == -1)   // error
            {
                // error: string length must be >= 3
                return View();
            }
            return View();
        }
        [NonAction]
        public int GetArticleIdByKeyword(string keywords)
        {
            int id = 0;
            if (keywords.Length < 3)
                return -1;
            return id;
        }
    }
}