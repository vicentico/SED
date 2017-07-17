using System.Collections.Generic;
using System.Web.Mvc;
using Template.Domain.Model;
using Template.Engine.MVC;
using Template.Service.DAO;

namespace Template.MVCApp.Areas.Evaluaciones.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Evaluaciones/Home
        public ActionResult Index()
        {
            if (Request.Url == null) return View();
            var Menu_ = MenuService.GetByUrl(Request.Url.AbsolutePath);
            var Menu__ = new List<Menu>();

            if (Menu_ != null)
            {
                Menu__ = MenuService.GetChildren(Menu_.Id);
            }

            return View(Menu__);
        }
    }
}