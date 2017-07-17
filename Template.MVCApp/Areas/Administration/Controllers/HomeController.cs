using System.Collections.Generic;
using System.Web.Mvc;
using Template.Domain.Model;
using Template.Engine.MVC;
using Template.Service.DAO;

namespace Template.MVCApp.Areas.Administration.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Index()
        {
            if (Request.Url == null) return View();
            var Menu_ = MenuService.GetByUrl(Request.Url.AbsolutePath);
            var Menu__ = new List<Menu>();

            if (Menu_ != null)
            {
                //Menu__ = MenuService.GetChildren(Menu_.Id);
            }

            return View(Menu__);
        }
    }
}
