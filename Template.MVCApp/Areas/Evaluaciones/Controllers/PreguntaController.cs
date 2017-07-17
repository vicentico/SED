using System;
using System.Linq;
using System.Web.Mvc;
using Template.Engine.Data.Models;
using Template.Engine.MVC;
using Template.Service.DAO;


namespace Template.MVCApp.Areas.Evaluaciones.Controllers
{
    public class PreguntaController : BaseController
    {
        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult GetPregunta(string titulo, string descripcion, bool? Estado)
        {
            var Pregunta_ = PreguntaService.GetPregunta(titulo, descripcion,  Estado);

            var btnAssignEvaluacion = "<a href='javascript:void(0)' class='pl8 pr8 link-action assignevaluacion' title='Gestionar Evaluaciones' data-id='{0}'><i class='icon-tasks'></i></a>";
            var btnEdit = "<a href='javascript:void(0)' class='pl8 pr8 link-action edit' title='Editar' data-id='{0}'><i class='icon-edit'></i></a>";
            var btnRemove = "<a href='javascript:void(0)' class='pl8 pr8 link-action remove' title='Borrar' data-id='{0}'><i class='icon-remove'></i></a>";

            var Pregunta__ = Pregunta_.Select(X => new
            {
               Id= X.id,
                 X.titulo,
                X.descripcion,
                Acciones = string.Format(btnAssignEvaluacion + btnEdit + btnRemove, X.id)
            }).ToList();

            return JsonOutPut(ResultJson.MensajeDataExito("", Pregunta__));
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int? Id = null)
        {
            //var EvaluacionOtras_ = EvaluacionService.GetEvaluacionOthers(Id);
            var Tipos = TipoService.GetAll("Pregunta");
            //ViewBag.EvaluacionOtras = EvaluacionOtras_;
            ViewBag.Tipos = Tipos;

            if (!Id.HasValue) return View();
            var Pregunta_ = PreguntaService.FindById(Id.Value);

            return Pregunta_ != null ? View(Pregunta_) : View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int Id,  int Tipo, string Titulo, string Descripcion, int? Orden, bool? Estado = null)
        {
            try
            {
                if (Id > 0)
                {
                    var Pregunta_ = PreguntaService.Edit(Id, Tipo, Titulo, Descripcion,  Orden, Estado);

                    return Pregunta_ ?
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Menú <strong>{0}</strong> Editado Correctamente.", Titulo))) :
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Editar el Menú."));
                }
                else
                {
                    var Pregunta_ = PreguntaService.New(Titulo, Descripcion, Tipo);

                    return Pregunta_ == null ?
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Crear la nueva Pregunta.")) :
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Pregunta <strong>{0}</strong> Creada Correctamente.", Titulo)));
                }
            }
            catch (Exception Ex)
            {
                return JsonOutPut(ResultJson.MensajeError(Ex.Message));
            }
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult AssignEvaluacion(int Id)
        {
            var EvaluacionUnAssigned = EvaluacionService.GetUnAssignedPregunta(Id);
            var EvaluacionAssigned = EvaluacionService.GetAssignedPregunta(Id);

            ViewBag.EvaluacionesUnAssigned = EvaluacionUnAssigned;
            ViewBag.EvaluacionesAssigned = EvaluacionAssigned;

            return View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador"), ActionName("AssignEvaluacion")]
        public ActionResult AssignEvaluacionPost(string Evaluaciones, int PreguntaId)
        {
            var Result = PreguntaService.AssignEvaluacion(PreguntaId, Evaluaciones);

            return JsonOutPut(!Result ? ResultJson.MensajeError("Ocurrió un Error al Asignar/Desasignar Evaluacion(es).") : ResultJson.MensajeExito("Cambios Realizados Correctamente."));
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Remove(int Id)
        {
            var Result = PreguntaService.Remove(Id);

            return JsonOutPut(!Result ? ResultJson.MensajeError("Ocurrió un Error al Remover la Pregunta.") : ResultJson.MensajeExito("Pregunta ha sido Removido Correctamente."));
        }
    }
}

