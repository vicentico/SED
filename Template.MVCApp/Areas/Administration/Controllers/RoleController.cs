using System;
using System.Linq;
using System.Web.Mvc;
using Template.Engine.Data.Models;
using Template.Engine.MVC;
using Template.Service.DAO;

namespace Template.MVCApp.Areas.Administration.Controllers
{
    public class RoleController : BaseController
    {
        [AuthorizeRole(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult ObtenerRoles(bool Activo, string Nombre, string Descripcion)
        {
            var Roles = RoleService.GetByStateAndNameAndDescription(Activo, Nombre, Descripcion);
            var btnEdit = "<a href='javascript:void(0)' class='pl8 pr8 link-action edit' title='Editar' data-id='{0}'><i class='icon-edit'></i></a>";
            var btnRemove = "<a href='javascript:void(0)' class='pl8 pr8 link-action remove' title='Borrar' data-id='{0}'><i class='icon-remove'></i></a>";

            var Roles_ = Roles.Select(X => new
            {
                X.Id,
                X.Nombre,
                X.Descripcion,
                Estado = X.Activo ? "Activo" : "Inactivo",
                Acciones = string.Format(btnEdit + btnRemove, X.Id)
            });

            return JsonOutPut(ResultJson.MensajeDataExito("", Roles_));
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int? Id = null)
        {
            if (!Id.HasValue) return View();
            var Role = RoleService.FindById(Id.Value);

            return Role != null ? View(Role) : View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int Id, string Nombre, string Descripcion, bool? Activo = null)
        {
            try
            {
                if (Id > 0)
                {
                    var Role = RoleService.Edit(Id, Nombre, Descripcion, Activo);

                    return Role ? 
						JsonOutPut(ResultJson.MensajeExito(string.Format("Rol <strong>{0}</strong> Editado Correctamente.", Nombre))) : 
						JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Editar el Rol."));
                }
                else
                {
                    var Role = RoleService.New(Nombre, Descripcion);

                    return Role == null ?
						JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Crear el nuevo Rol.")) :
						JsonOutPut(ResultJson.MensajeExito(string.Format("Rol <strong>{0}</strong> Creado Correctamente.", Nombre)));   
                }
            }
            catch (Exception Ex)
            {
                return JsonOutPut(ResultJson.MensajeError(Ex.Message));
            }
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Remove(int Id)
        {
            try
            {
                var Role = RoleService.Remove(Id);

                return Role ? 
					JsonOutPut(ResultJson.MensajeExito("Rol <strong>Borrado</strong> Correctamente.")) :
					JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar <strong>Borrar</strong> el Rol."));
            }
            catch (Exception Ex)
            {
                return JsonOutPut(ResultJson.MensajeError(Ex.Message));
            }
        }
	}
}
