using System;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using Template.Engine.Helper;
using Template.Service.Helpers;

namespace Template.Engine.MVC
{
    [HandleError]
    public class BaseController : Controller
    {
		protected override void OnActionExecuting(ActionExecutingContext ActionExecutingContext_)
		{
			base.OnActionExecuting(ActionExecutingContext_);
		    
			if (ActionExecutingContext_.RequestContext.HttpContext.Request.Url != null)
				ViewBag.BreadCrumb = BreadCrumb.GenerarBreadCrumb(ActionExecutingContext_.RequestContext.HttpContext.Request.Url.AbsolutePath);
		}

		protected override void OnException(ExceptionContext ExceptionContext_)
	    {
			base.OnException(ExceptionContext_);

			var EsAjax = ExceptionContext_.RequestContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

			if (EsAjax) return;

            var Exception_ = ExceptionContext_.Exception.GetBaseExcepcion();
			Response.Clear();

			var HttpError = Exception_ as HttpException;
			var Error_ = new ExcepcionWeb();
			var UrlOrigen = ExceptionContext_.RequestContext.HttpContext.Request.Url != null ? ExceptionContext_.RequestContext.HttpContext.Request.Url.AbsolutePath : "";

			if (HttpError != null)
			{
				var Codigo = HttpError.GetHttpCode();
				var Mensaje = HttpError.Message;

				Error_.Codigo = Codigo;
				Error_.Url = UrlOrigen;
				Error_.Mensaje = Mensaje;
			}
			else
			{
				Error_.Codigo = 0;
				Error_.Url = UrlOrigen;
			    if (Exception_ != null) Error_.Mensaje = Exception_.Message;
			}

			Session["ErrorException"] = Error_;
			Server.ClearError();

            if (!Response.IsRequestBeingRedirected) Response.Redirect(@"/Home/Error"); 
	    }

	    public int SetAuthorizationCookie<T>(HttpResponseBase HttpResponseBase, string Nombre, bool Recordarme, T UserData)
        {
            var HttpCookie = FormsAuthentication.GetAuthCookie(Nombre, Recordarme);
            var FormsAuthenticationTicket = FormsAuthentication.Decrypt(HttpCookie.Value);

            if (FormsAuthenticationTicket == null) return 0;
            FormsAuthenticationTicket = new FormsAuthenticationTicket(
                FormsAuthenticationTicket.Version,
                FormsAuthenticationTicket.Name,
                FormsAuthenticationTicket.IssueDate,
                FormsAuthenticationTicket.Expiration,
                FormsAuthenticationTicket.IsPersistent,
                JsonConvert.SerializeObject(UserData),
                FormsAuthenticationTicket.CookiePath
                );

            var EncTicket = FormsAuthentication.Encrypt(FormsAuthenticationTicket);

            HttpCookie.Value = EncTicket;
            HttpResponseBase.Cookies.Add(HttpCookie);

            return EncTicket != null ? EncTicket.Length : 0;
        }

        public JsonResult JsonOutPut(bool Error_ = false, string Message = "", object OutPut = null)
        {
            try
            {
                return Json(new
                {
                    Error = Error_,
                    Mensaje = Message,
                    Datos = OutPut
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    Error = Error_,
                    Mensaje = Message,
                    Datos = OutPut
                }, JsonRequestBehavior.AllowGet);
            }
        }

		public JsonResult JsonOutPut(object ResultJson)
		{
			return Json(ResultJson, JsonRequestBehavior.AllowGet);
		}

        protected override JsonResult Json(object Data, string ContentType, Encoding ContentEncoding, JsonRequestBehavior Behavior)
        {
            return new JsonResult
            {
                Data = Data,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpGet]
        public FileResult DownloadFile(string NombreArchivo, string Directorio = "~/Temp", bool Borrar = true)
        {
            try
            {
                var DirectorioTemporal = Server.MapPath(Directorio);
                var Ruta = string.Format("{0}/{1}", DirectorioTemporal, NombreArchivo);
                byte[] Contenido = {};

                if (System.IO.File.Exists(Ruta)) Contenido = System.IO.File.ReadAllBytes(Ruta);
                if (System.IO.File.Exists(Ruta) && Borrar) System.IO.File.Delete(Ruta);

                return File(Contenido, MediaTypeNames.Application.Octet, NombreArchivo);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
