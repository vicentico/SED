using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Engine.Helper;
using Template.MVCApp.Engine;

namespace Template.MVCApp
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object Sender, EventArgs E)
        {
            #region Regional Configuration.
            var CultureInfo = new CultureInfo("es")
            {
                NumberFormat =
                {
                    NumberDecimalSeparator = ",",
                    CurrencySymbol = "$",
                    CurrencyGroupSeparator = ".",
                    CurrencyPositivePattern = 2,
                    CurrencyNegativePattern = 2
                }
            };

            Thread.CurrentThread.CurrentCulture = CultureInfo;
            Thread.CurrentThread.CurrentUICulture = CultureInfo;
            #endregion

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        //protected void Application_Error(object Sender, EventArgs E)
        //{
        //    var EsAjax = HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        //    if (EsAjax) return;

        //    var Exception_ = Server.GetLastError().GetBaseExcepcion();
        //    Response.Clear();

        //    var HttpError = Exception_ as HttpException;
        //    var Error_ = new ExcepcionWeb();
        //    var UrlOrigen = HttpContext.Current.Request.Url.AbsoluteUri;

        //    if (HttpError != null)
        //    {
        //        var Codigo = HttpError.GetHttpCode();
        //        var Mensaje = HttpError.Message;
		       
        //        Error_.Codigo = Codigo;
        //        Error_.Url = UrlOrigen;
        //        Error_.Mensaje = Mensaje;
        //    }
        //    else
        //    {
        //        Error_.Codigo = 0;
        //        Error_.Url = UrlOrigen;
        //        if (Exception_ != null) Error_.Mensaje = Exception_.Message;
        //    }

        //    Session["ErrorException"] = Error_;
        //    Server.ClearError();

        //    if (!Response.IsRequestBeingRedirected) Response.Redirect(@"/Home/Error");
        //}
    }
}
