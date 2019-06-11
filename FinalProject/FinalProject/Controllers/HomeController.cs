using FinalProject.Beans;
using FinalProject.Models;
using FinalProject.Services;
using FinalProject.Services.Impl;
using log4net;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            log.Info("Index page is opened.");
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            VkService service = new VKServiseImpl();
            user.Token = service.GetVKToken(user);

            Session["User"] = user;
            if (user.Token != null)
            {
                log.Info("User redirected to Search page.");
                return View("Search");
            }
            else
            {
                log.Error("Not valid User.");
                return View("NotValidUser");
            }
        }

        [HttpPost]
        public ActionResult Search(SearchConfig searchConfig)
        {
            User user = (User) Session["User"];
            VkService service = new VKServiseImpl();
            if (searchConfig.GroupDomain != null)
            {
                List<VKPost> posts = service.GetVkPostByDomain(searchConfig.GroupDomain, searchConfig.Size, user);
                ViewBag.VKposts = posts;
                log.Info("User gets posts  by GroupDomain.");
                return View("SearchResults");
            }
            else if (searchConfig.GroupId != 0)
            {
                List<VKPost> posts = service.GetVkPostById(searchConfig.GroupId, searchConfig.Size, user);
                ViewBag.VKposts = posts;
                log.Info("User gets posts  by GroupId.");
                return View("SearchResults");
            }
            log.Error("Not valid SearchResults.");
            return View("NotValidSearch");
        }
    }
}