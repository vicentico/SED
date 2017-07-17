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
    public class AreaController : BaseController
    {
        // GET: Evaluaciones/Area
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ObtenerAreas(bool Activo, string Nombre, string Descripcion)
        {
            var Areas = AreaService.GetByStateAndNameAndDescription(Activo, Nombre, Descripcion);
            var btnEdit = "<a href='javascript:void(0)' class='pl8 pr8 link-action edit' title='Editar' data-id='{0}'><i class='icon-edit'></i></a>";
            var btnRemove = "<a href='javascript:void(0)' class='pl8 pr8 link-action remove' title='Borrar' data-id='{0}'><i class='icon-remove'></i></a>";

            var Areas_ = Areas.Select(X => new
            {
                Id = X.id,
                Nombre = X.nombre,
                Descripcion = X.descripcion,
                Estado = X.activo ? "Activo" : "Inactivo",
                Acciones = string.Format(btnEdit + btnRemove, X.id)
            });

            return JsonOutPut(ResultJson.MensajeDataExito("", Areas_));
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int? Id = null)
        {
            if (!Id.HasValue) return View();
            var Area = AreaService.FindById(Id.Value);

            return Area != null ? View(Area) : View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int Id, string Nombre, string Descripcion, bool? Activo = null)
        {
            try
            {
                if (Id > 0)
                {
                    var Area = AreaService.Edit(Id, Nombre, Descripcion, Activo);

                    return Area ?
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Area <strong>{0}</strong> Editado Correctamente.", Nombre))) :
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Editar el Area."));
                }
                else
                {
                    var Area = AreaService.New(Nombre, Descripcion);

                    return Area == null ?
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Crear el nuevo Area.")) :
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Area <strong>{0}</strong> Creado Correctamente.", Nombre)));
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
                var Area = AreaService.Remove(Id);

                return Area ?
                    JsonOutPut(ResultJson.MensajeExito("Area <strong>Borrado</strong> Correctamente.")) :
                    JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar <strong>Borrar</strong> el Area."));
            }
            catch (Exception Ex)
            {
                return JsonOutPut(ResultJson.MensajeError(Ex.Message));
            }
        }



    }
}