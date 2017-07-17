using System;
using System.Linq;
using System.Web.Mvc;
using Template.Engine.Data.Models;
using Template.Engine.MVC;
using Template.Service.DAO;

namespace Template.MVCApp.Areas.Administration.Controllers
{
    public class MenuController : BaseController
    {
        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult GetMenu(string Url_, string Texto, string Ayuda, bool? Estado)
        {
            var Menu_ = MenuService.GetMenu(Url_, Texto, Ayuda, Estado);

            var btnAssignRole = "<a href='javascript:void(0)' class='pl8 pr8 link-action assignrole' title='Gestionar Roles' data-id='{0}'><i class='icon-tasks'></i></a>";
            var btnEdit = "<a href='javascript:void(0)' class='pl8 pr8 link-action edit' title='Editar' data-id='{0}'><i class='icon-edit'></i></a>";
            var btnRemove = "<a href='javascript:void(0)' class='pl8 pr8 link-action remove' title='Borrar' data-id='{0}'><i class='icon-remove'></i></a>";

            var Menu__ = Menu_.Select(X => new
            {
                X.Id,
                MenuPadre = X.Menu2 != null ? X.Menu2.Texto : "",
                Url_ = X.Url,
                Icono = string.Format("<i class='{0} fs18 link-action'></i>", X.Icono),
                X.Texto,
                X.Ayuda,
                X.Orden,
                Acciones = string.Format(btnAssignRole + btnEdit + btnRemove, X.Id)
            }).ToList();

            return JsonOutPut(ResultJson.MensajeDataExito("", Menu__));
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int? Id = null)
        {
            var MenusPadre_ = MenuService.GetMenuOthers(Id);
            var Iconos = IconoService.GetAll();
            ViewBag.MenusPadre = MenusPadre_;
            ViewBag.Iconos = Iconos;

            if (!Id.HasValue) return View();
            var Menu_ = MenuService.FindById(Id.Value);

            return Menu_ != null ? View(Menu_) : View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int Id, int? Menu_Id, string Url_, string Icono, string Texto, string Ayuda, int? Orden, bool? Estado = null)
        {
            try
            {
                if (Id > 0)
                {
                    var Menu_ = MenuService.Edit(Url_, Icono, Texto, Ayuda, Id, Menu_Id, Orden, Estado);

                    return Menu_ ?
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Menú <strong>{0}</strong> Editado Correctamente.", Texto))) :
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Editar el Menú."));
                }
                else
                {
                    var Menu_ = MenuService.New(Url_, Icono, Texto, Ayuda, Menu_Id, Orden);

                    return Menu_ == null ?
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Crear el nuevo Menú.")) :
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Menú <strong>{0}</strong> Creado Correctamente.", Texto)));
                }
            }
            catch (Exception Ex)
            {
                return JsonOutPut(ResultJson.MensajeError(Ex.Message));
            }
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult AssignRole(int Id)
        {
            var RolesUnAssigned = RoleService.GetUnAssignedMenu(Id);
            var RolesAssigned = RoleService.GetAssignedMenu(Id);

            ViewBag.RolesUnAssigned = RolesUnAssigned;
            ViewBag.RolesAssigned = RolesAssigned;

            return View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador"), ActionName("AssignRole")]
        public ActionResult AssignRolePost(string Roles, int MenuId)
        {
            var Result = MenuService.AssignRole(MenuId, Roles);

            return JsonOutPut(!Result ? ResultJson.MensajeError("Ocurrió un Error al Asignar/Desasignar Rol(es).") : ResultJson.MensajeExito("Cambios Realizados Correctamente."));
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Remove(int Id)
        {
            var Result = MenuService.Remove(Id);

            return JsonOutPut(!Result ? ResultJson.MensajeError("Ocurrió un Error al Remover el Menú.") : ResultJson.MensajeExito("Menú ha sido Removido Correctamente."));
        }
	}
}
