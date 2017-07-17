using System.Web.Mvc;

namespace Template.Engine.MVC
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext AuthorizationContext_)
        {
            base.OnAuthorization(AuthorizationContext_);

            var Request_ = AuthorizationContext_.RequestContext.HttpContext.Request;
            var Response_ = AuthorizationContext_.RequestContext.HttpContext.Response;

            var SesionValida = Request_.IsAuthenticated;
            var EsAjax = Request_.IsAjaxRequest();

            if (SesionValida && (AuthorizationContext_.Result is HttpUnauthorizedResult) && !EsAjax) AuthorizationContext_.Result = new RedirectResult("~/Access/Denied");
            else if (SesionValida && (AuthorizationContext_.Result is HttpUnauthorizedResult) && EsAjax)
            {
                Response_.Clear();
                Response_.SuppressFormsAuthenticationRedirect = true;
                Response_.StatusCode = 600; //Custom Code for Ajax Not Authorization.
                Response_.StatusDescription = "Not Authorized";
                Response_.End();
            }
            else if (SesionValida && !EsAjax) return;
            else if (!SesionValida && EsAjax) {
                Response_.Clear();
                Response_.SuppressFormsAuthenticationRedirect = true;
                Response_.StatusCode = 601; //Custom Code for Ajax Login Redirection.
                Response_.StatusDescription = "Not Authenticated";

                Response_.End();
            }
        }
    }
}
