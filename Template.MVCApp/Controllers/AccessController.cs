using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Template.Domain.Enums;
using Template.Engine.Helper;
using Template.Engine.Helper.Alert;
using Template.Engine.MVC;
using Template.Engine.Security;
using Template.Service.DAO;

namespace Template.MVCApp.Controllers
{
    public class AccessController : BaseController
    {
        [HttpGet, AllowAnonymous]
        public ActionResult Index()


        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Administrador")) return RedirectToAction("Index", "Home");
            
            var ReturnUrl = Request.QueryString["ReturnUrl"] ?? "";

            ViewBag.ReturnUrl = ReturnUrl;

            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Login(string CorreoElectronico, string Password, string ReturnUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(CorreoElectronico)) throw new Exception("Debe ingresar su Correo Electrónico.");
                if (string.IsNullOrEmpty(Password)) throw new Exception("Debe ingresar su Contraseña.");
                
                var Usuario_ = UserService.GetByEMail(CorreoElectronico);

                if (Usuario_ == null) throw new Exception(string.Format("El Usuario con Correo Electrónico <strong>{0}</strong> no existe.", CorreoElectronico));
                var NombreUsuario = string.Format("{0} {1} {2}", Usuario_.Nombre, Usuario_.Apellido_Paterno, Usuario_.Apellido_Materno);
                if (Usuario_.Password != Coder.Encode(Password)) throw new Exception(string.Format("La Contraseña para el Usuario <strong>{0}</strong> es Incorrecta.", NombreUsuario));

                FormsAuthentication.SetAuthCookie(Usuario_.Id.ToString(), true);
                UserService.SetLastAccess(Usuario_.Id);

                if (Usuario_.Debe_Cambiar_Password) return RedirectToAction("ChangePassword");
                
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/")
                   && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                {
                    return Redirect(ReturnUrl);
                }
                //return RedirectToAction("Index", "Home", new { Area = "Administration" });
                return RedirectToAction("Index", "Home");
            }
            catch (Exception Ex)
            {
                this.Flash(Ex.GetBaseException().Message, MessageType.Error);
                return RedirectToAction("Index");
            }
        }

        [HttpGet, AllowAnonymous]
        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ActionName("RecoverPassword")]
        public ActionResult RecoverPasswordPost(string CorreoElectronico)
        {
            try
            {
                if (string.IsNullOrEmpty(CorreoElectronico)) throw new Exception("Debe ingresar su Correo Electrónico.");
                
                var Usuario_ = UserService.GetByEMail(CorreoElectronico);

                if (Usuario_ == null) throw new Exception(string.Format("El Usuario con Correo Electrónico <strong>{0}</strong> no existe.", CorreoElectronico));
                
                var Email_ = EmailService.GetById((int)EmailTemplate.PasswordRecovery);
                if (Email_ == null) throw new Exception("No se encontró una <strong>Plantilla de Correo<strong> para esta Solicitud.");
                var NewPassword = Membership.GeneratePassword(8, 0);
                
                var NombreReceptor = string.Format("{0} {1} {2}", Usuario_.Nombre.Trim(), Usuario_.Apellido_Paterno.Trim(), Usuario_.Apellido_Materno.Trim());
                var DireccionReceptor = new List<string> { Usuario_.Email };
                var Mensaje = Email_.Cuerpo;
                Mensaje = Mensaje.Replace("{NombreDestinatario}", NombreReceptor);
                Mensaje = Mensaje.Replace("{NombreAplicacion}", "Organiza - Evaluación de Desempeño");
                Mensaje = Mensaje.Replace("{NuevaPassword}", NewPassword);

                var EmailSent = Email.EnviarEmail(Email_.NombreRemitente, Email_.DireccionRemitente, Email_.Asunto, NombreReceptor, DireccionReceptor, Mensaje);

                if (!EmailSent) throw new Exception("Ocurrió un <strong>Error</strong> al Enviar por Correo su <strong>Nueva Contraseña</strong>, por favor contacte a su <strong>Adminsitrador</strong>.");

                NewPassword = Coder.Encode(NewPassword);
                var Result = UserService.ChangePassword(Usuario_.Id, NewPassword, true);

                if (Result) this.Flash("Su <strong>Nueva Contraseña</strong> ha sido enviada a su <strong>Correo Electrónico</strong><br><span>Recuerde por seguridad Actualizarla lo antes posible</span>.");
                else throw new Exception("Ocurrió un <strong>Error</strong> al restablecer su <strong>Contraseña</strong>, por favor contacte a su <strong>Adminsitrador</strong>.");

                return RedirectToAction("RecoverPassword");
            }
            catch (Exception Ex)
            {
                this.Flash(Ex.GetBaseException().Message, MessageType.Error);
                return RedirectToAction("RecoverPassword");
            }
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Denied() {
            return View();
        }

        [HttpGet, AllowAnonymous]
        public ActionResult SignOut() {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index");
        }

        [HttpGet, AuthorizeRole]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, AuthorizeRole, ActionName("ChangePassword")]
        public ActionResult ChangePasswordPost(string OldPassword, string NewPassword)
        {
            var Referrer = Url.Action("Index");

            try
            {
                var User_ = UserService.GetById(User.Identity.Name);

                if (User_ == null) throw new Exception("No se pudo encontrar el usuario.");
                if (User_.Password != Coder.Encode(OldPassword)) throw new Exception("Su <strong>Contraseña Actual</strong> no es correcta.");

                var Result = UserService.ChangePassword(User_.Id, Coder.Encode(NewPassword));

                if (Result)
                {
                    this.Flash("Su <strong>Contraseña</strong> se ha <strong>Modificado Correctamente</strong>.");
                    return Redirect(Referrer);
                }
                this.Flash("Ha Ocurrido un <strong>Error</strong> al Cambiar su Contraseña.", MessageType.Error);
                return Redirect(Referrer);
            }
            catch (Exception Ex)
            {
                this.Flash(Ex.Message, MessageType.Error);
                return View();
            }
        }
    }
}
