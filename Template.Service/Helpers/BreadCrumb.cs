using System.Web;
using System.Web.Mvc;
using Template.Service.DAO;

namespace Template.Service.Helpers
{
    public static class BreadCrumb
    {
        public static string GenerarBreadCrumb(string Url_)
        {
            var _Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var Menu_ = MenuService.GetByUrl(Url_);

            if (Menu_ == null) return null;

            var BreadCrumb = string.Format("<li class='active' title='{0}'>{1}", Menu_.Ayuda, Menu_.Texto);
            var Salida = Menu_.Menu_Id.HasValue ? string.Format("{0}<span class='divider'>/</span></li>{1}", GenerarBreadCrumb(Menu_.Menu_Id.Value), BreadCrumb) : BreadCrumb;
            Salida = string.Format("<li><a href='{0}' title='Ir al Inicio'><i class='icon-home mr8'></i>Inicio</a><span class='divider'>/</span></li>{1}", _Url.Action("Index", "Home", new { Area = "" }), Salida);

            return Salida;
        }

        public static string GenerarBreadCrumb(int IdMenuPadre)
        {
            var Menu_ = MenuService.GetByParent(IdMenuPadre);

            if (Menu_ == null) return null;

            var BreadCrumb = string.Format("<li><a href='{0}' title='{1}'>{2}</a>", Menu_.Url, Menu_.Ayuda, Menu_.Texto);
            var Salida = Menu_.Menu_Id.HasValue ? string.Format("{0}<span class='divider'>/</span></li>{1}", GenerarBreadCrumb(Menu_.Menu_Id.Value), BreadCrumb) : BreadCrumb;

            return Salida;
        }
    }
}
