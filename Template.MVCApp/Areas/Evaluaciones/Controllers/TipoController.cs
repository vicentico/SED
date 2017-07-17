using System;
using System.Linq;
using System.Web.Mvc;
using Template.Engine.Data.Models;
using Template.Engine.MVC;
using Template.Service.DAO;

using Template.Engine.Helper;
using Template.Service;

namespace Template.MVCApp.Areas.Evaluaciones.Controllers
{
    public class TipoController : BaseController
    {
        // GET: Evaluaciones/Tipo
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ObtenerTipos(bool Activo, string Nombre, string Descripcion)
        {
            var Tipos = TipoService.GetByStateAndNameAndDescription(Activo, Nombre, Descripcion);
            var btnEdit = "<a href='javascript:void(0)' class='pl8 pr8 link-action edit' title='Editar' data-id='{0}'><i class='icon-edit'></i></a>";
            var btnRemove = "<a href='javascript:void(0)' class='pl8 pr8 link-action remove' title='Borrar' data-id='{0}'><i class='icon-remove'></i></a>";

            var Tipos_ = Tipos.Select(X => new
            {
                Id=X.id,
                Nombre=X.nombre,
                Descripcion=X.descripcion,
                Estado = X.activo ? "Activo" : "Inactivo",
                Acciones = string.Format(btnEdit + btnRemove, X.id)
            });

            return JsonOutPut(ResultJson.MensajeDataExito("", Tipos_));
        }

                [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int? Id = null)
        {
            if (!Id.HasValue) return View();
            var Tipo = TipoService.FindById(Id.Value);

            return Tipo != null ? View(Tipo) : View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int Id, string Nombre, string Descripcion, string Categoria, bool? Activo = null)
        {
            try
            {
                if (Id > 0)
                {
                    var Tipo = TipoService.Edit(Id, Nombre, Descripcion, Activo);

                    return Tipo ?
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Tipo <strong>{0}</strong> Editado Correctamente.", Nombre))) :
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Editar el Tipo."));
                }
                else
                {
                    var Tipo = TipoService.New(Nombre, Descripcion,Categoria);

                    return Tipo == null ?
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Crear el nuevo Tipo.")) :
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Tipo <strong>{0}</strong> Creado Correctamente.", Nombre)));
                }
            }
            catch (Exception Ex)
            {
                return JsonOutPut(ResultJson.MensajeError(Ex.Message));
            }
        }

        //[HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Remove(int Id)
        {
            try
            {
                var Tipo = TipoService.Remove(Id);

                return Tipo ?
                    JsonOutPut(ResultJson.MensajeExito("Tipo <strong>Borrado</strong> Correctamente.")) :
                    JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar <strong>Borrar</strong> el Tipo."));
            }
            catch (Exception Ex)
            {
                return JsonOutPut(ResultJson.MensajeError(Ex.Message));
            }
        }



    }
}