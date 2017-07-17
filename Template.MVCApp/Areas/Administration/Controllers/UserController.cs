using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Template.Engine.Data.Models;
using Template.Engine.Helper;
using Template.Engine.Helper.Alert;
using Template.Engine.MVC;
using Template.Engine.Security;
using Template.Service.DAO;

namespace Template.MVCApp.Areas.Administration.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet, AuthorizeRole]
        public ActionResult MyProfile()
        {
            var User_ = UserService.GetById(User.Identity.Name);

            return User_ != null ? View(User_) : View();
        }

		[HttpPost, AuthorizeRole, ActionName("MyProfile")]
		public ActionResult MyProfilePost(int Id, string Nombre, string APaterno, string AMaterno)
	    {
			#region Profile Image
			byte[] ImageData = null;

			foreach (string FileName_ in Request.Files)
			{
				var File_ = Request.Files[FileName_];
				if (File_ != null && File_.ContentLength == 0) continue;

				if (File_ == null) continue;
				var BinaryReader_ = new BinaryReader(File_.InputStream);
				var ContentLength = File_.ContentLength;
				ImageData = new byte[ContentLength];

				BinaryReader_.Read(ImageData, 0, ContentLength);
				BinaryReader_.Close();
			}
			#endregion

			var Result = UserService.Edit(Id, Nombre, APaterno, AMaterno, ImageData);
			var Referrer = Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : "";

		    if (Result)
		    {
                this.Flash("Su <strong>Perfil</strong> se ha <strong>Actualizado Correctamente</strong>.");
		        return Redirect(Referrer);
		    }
		    this.Flash("Ha Ocurrido un <strong>Error</strong> al Actualizar el Perfil.", MessageType.Error);
		    return Redirect(Referrer);
	    }

        [HttpGet, AuthorizeRole]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, AuthorizeRole, ActionName("ChangePassword")]
        public ActionResult ChangePasswordPost(string OldPassword, string NewPassword)
        {
            var Referrer = Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : "";

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
                return Redirect(Referrer);
            }
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult GetUsers(string Email, string Nombre, string APaterno, string AMaterno, bool? Estado)
        {
            var Users_ = UserService.GetUsers(Email, Nombre, APaterno, AMaterno, Estado);

			var btnAssignRole = "<a href='javascript:void(0)' class='pl8 pr8 link-action assignrole' title='Gestionar Roles' data-id='{0}'><i class='icon-tasks'></i></a>";
            var btnRequestPassword = "<a href='javascript:void(0)' class='pl8 pr8 link-action requestpassword' title='Ver Contraseña' data-id='{0}'><i class='icon-key'></i></a>";
            var btnEdit = "<a href='javascript:void(0)' class='pl8 pr8 link-action edit' title='Editar' data-id='{0}'><i class='icon-edit'></i></a>";
            var btnRemove = "<a href='javascript:void(0)' class='pl8 pr8 link-action remove' title='Borrar' data-id='{0}'><i class='icon-remove'></i></a>";
            
            var _Users = Users_.Select(X => new
            {
                X.Id,
                X.Email,
                X.Nombre,
                X.Apellido_Paterno,
                X.Apellido_Materno,
                Ultimo_Acceso = string.Format("<span title='{0}'>{1}</span>", Utility.TimeAgo(X.Ultimo_Acceso), X.Ultimo_Acceso.HasValue ? X.Ultimo_Acceso.Value.ToString("dd-MM-yyyy HH:mm:ss") : "Nunca"),
                Debe_Cambiar_Password = X.Debe_Cambiar_Password ? "Si" : "No",
                Estado = X.Activo ? "Activo" : "Inactivo",
                Acciones = string.Format(btnAssignRole + btnRequestPassword + btnEdit + btnRemove, X.Id)
            });

            return JsonOutPut(ResultJson.MensajeDataExito("", _Users));
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int? Id = null)
        {
            if (!Id.HasValue) return View();
            var User_ = UserService.FindById(Id.Value);

            return User_ != null ? View(User_) : View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int Id, string Email, string Nombre, string APaterno, string AMaterno, bool? Activo = null)
        {
            try
            {
                if (Id > 0)
                {
                    var User_ = UserService.Edit(Id, Nombre, APaterno, AMaterno, null, Activo, Email);

                    return User_ ?
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Usuario <strong>{0} {1} {2}</strong> Editado Correctamente.", Nombre, APaterno, AMaterno))) :
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Editar el Usuario."));
                }
                else
                {
                    var Password = Membership.GeneratePassword(8, 0);
                    Password = Coder.Encode(Password);

                    var User_ = UserService.New(Email, Nombre, APaterno, AMaterno, Password);

                    return User_ == null ?
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Crear el nuevo Usuario.")) :
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Usuario <strong>{0}</strong> Creado Correctamente.", Nombre)));
                }
            }
            catch (Exception Ex)
            {
                return JsonOutPut(ResultJson.MensajeError(Ex.Message));
            }
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult RequestPassword(string Id)
        {
            var User_ = UserService.FindById(Convert.ToInt32(Id));

            return User_ != null ? View(User_) : View();
        }

	    [HttpGet, AuthorizeRole(Roles = "Administrador")]
	    public ActionResult AssignRole(int UserId)
	    {
		    var RolesUnAssigned = RoleService.GetUnAssignedUser(UserId);
            var RolesAssigned = RoleService.GetAssignedUser(UserId);

            ViewBag.RolesUnAssigned = RolesUnAssigned;
            ViewBag.RolesAssigned = RolesAssigned;

		    return View();
	    }

		[HttpPost, AuthorizeRole(Roles = "Administrador"), ActionName("AssignRole")]
		public ActionResult AssignRolePost(string Roles, int UserId)
		{
			var Result = UserService.AssignRole(UserId, Roles);

		    return JsonOutPut(!Result ? ResultJson.MensajeError("Ocurrió un Error al Asignar/Desasignar Rol(es).") : ResultJson.MensajeExito("Cambios Realizados Correctamente."));
		}

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Remove(int Id)
        {
            var Result = UserService.Remove(Id);

            return JsonOutPut(!Result ? ResultJson.MensajeError("Ocurrió un Error al Remover el Usuario.") : ResultJson.MensajeExito("Usuario ha sido Removido Correctamente."));
        }
    }
}
