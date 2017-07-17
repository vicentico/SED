
using System;
using System.Linq;
using System.Web.Mvc;
using Template.Engine.Data.Models;
using Template.Engine.MVC;
using Template.Service.DAO;

namespace Template.MVCApp.Areas.Evaluaciones.Controllers
{
    public class EmpleadoController : BaseController
    {
        [AuthorizeRole(Roles = "Administrador")]
        public ActionResult Index()
        {
            var Area = AreaService.GetAll();
            ViewBag.Areas = Area;

            //var Usuario = UserService.GetAll();
            //ViewBag.Usuarios = Usuario;

            var Empleados = EmpleadoService.GetAll();
            ViewBag.Empleados = Empleados;
            return View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult ObtenerEmpleados(string Nombre, string Cargo ,int AreaId, int JefeId, bool Activo)
        {
            var Empleados = EmpleadoService.GetByStateAndNameAndDescription( Nombre, Cargo, AreaId, JefeId, Activo);
            var btnEdit = "<a href='javascript:void(0)' class='pl8 pr8 link-action edit' title='Editar' data-id='{0}'><i class='icon-edit'></i></a>";
            var btnRemove = "<a href='javascript:void(0)' class='pl8 pr8 link-action remove' title='Borrar' data-id='{0}'><i class='icon-remove'></i></a>";

            var Empleados_ = Empleados.Select(X => new
            {
                Id=X.id,
                X.nombre,
                id_Aera=X.Area.nombre,
                X.cargo,
                X.Usuario.Email,
                //X.id_Empleado_Jefe,
                //id_Empleado_Jefe = EmpleadoService.FindById(X.id_Empleado_Jefe)!=null? EmpleadoService.FindById(X.id_Empleado_Jefe).nombre:string.Empty,//EmpleadoService.FindById(X.id_Empleado_Jefe).nombre ? EmpleadoService.FindById(X.id_Empleado_Jefe).nombre:"",
                id_Empleado_Jefe =X.Empleado2!=null? X.Empleado2.nombre:"Sin Jefe",
                X.id_Usuario,
                Estado = X.activo ? "Activo" : "Inactivo",
                Acciones = string.Format(btnEdit + btnRemove, X.id)
            }).ToList(); 

            return JsonOutPut(ResultJson.MensajeDataExito("", Empleados_));
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int? Id = null)
        {
            var Area = AreaService.GetAll();
            ViewBag.Areas = Area;

            var Usuario = UserService.GetAll();
            ViewBag.Usuarios = Usuario;

            var Empleados = EmpleadoService.GetAll();
            ViewBag.Empleados = Empleados;


            if (!Id.HasValue) return View();
            var Empleado = EmpleadoService.FindById(Id.Value);

            return Empleado != null ? View(Empleado) : View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int Id, string Nombre, string Cargo, int AreaId, int UsuarioId, int? JefeId, bool? Activo = null)
        {
            

            try
            {
                if (Id > 0)
                {
                    var Empleado = EmpleadoService.Edit(Id, Nombre, Cargo, AreaId, UsuarioId, JefeId, Activo);

                    return Empleado ?
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Empleado <strong>{0}</strong> Editado Correctamente.", Nombre))) :
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Editar el Empleado."));
                }
                else
                {
                    var Empleado = EmpleadoService.New(Nombre, Cargo, AreaId, UsuarioId, JefeId);

                    return Empleado == null ?
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Crear el nuevo Empleado.")) :
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Empleado <strong>{0}</strong> Creado Correctamente.", Nombre)));
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
                var Empleado = EmpleadoService.Remove(Id);

                return Empleado ?
                    JsonOutPut(ResultJson.MensajeExito("Empleado <strong>Borrado</strong> Correctamente.")) :
                    JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar <strong>Borrar</strong> el Empleado."));
            }
            catch (Exception Ex)
            {
                return JsonOutPut(ResultJson.MensajeError(Ex.Message));
            }
        }
    }
}
