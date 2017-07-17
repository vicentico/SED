using System.Web.Mvc;
using Template.Engine.MVC;
using Template.Service.DAO;

namespace Template.MVCApp.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet, AuthorizeRole]
        public ActionResult Index()
        {
            var Menu_ = UserService.GetMenuItems(User.Identity.Name);

            return View(Menu_);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Error() {
            return View();
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Error404(string aspxerrorpath)
        {
            ViewBag.Url_ = aspxerrorpath;

            return View();
        }
    }
}
