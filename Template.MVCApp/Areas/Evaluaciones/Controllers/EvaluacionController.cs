using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Template.Domain.Enums;
using Template.Engine.Data.Models;
using Template.Engine.Helper.Alert;
using Template.Engine.MVC;
using Template.Engine.Security;
using Template.Service.DAO;

namespace Template.MVCApp.Areas.Evaluaciones.Controllers
{
    public class EvaluacionController : BaseController
    {
        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Index()
        {
            var Tipos = TipoService.GetAll("Eval");
            ViewBag.Tipos = Tipos;
            return View();
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult EnviarEvaluacion(int Id)
        {
            var Area = AreaService.GetAll();
            ViewBag.Areas = Area;
            ViewBag.idEvaluacion = Id;
            //ViewBag.id=
            //var Usuario = UserService.GetAll();
            //ViewBag.Usuarios = Usuario;

            //var Empleados = EmpleadoService.GetAll();
            //ViewBag.Empleados = Empleados;
            return View();
        }
       

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult ObtenerPreguntasEvaluacion(int Id )
        {
            var Pregunta_ = PreguntaService.VerPreguntasEvaluacion(Id);

            var btnAssignEvaluacion = "<a href='javascript:void(0)' class='pl8 pr8 link-action assignevaluacion' title='Gestionar Evaluaciones' data-id='{0}'><i class='icon-tasks'></i></a>";
            //var btnEdit = "<a href='javascript:void(0)' class='pl8 pr8 link-action edit' title='Editar' data-id='{0}'><i class='icon-edit'></i></a>";
            //var btnRemove = "<a href='javascript:void(0)' class='pl8 pr8 link-action remove' title='Borrar' data-id='{0}'><i class='icon-remove'></i></a>";

            var Pregunta__ = Pregunta_.Select(X => new
            {
                Id = X.id,
                X.titulo,
                X.descripcion,
                Acciones = string.Format(btnAssignEvaluacion , X.id)
            }).ToList();

            return JsonOutPut(ResultJson.MensajeDataExito("", Pregunta__));
        }


        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult VerPreguntasEvaluacion(int? Id = null)
        {
            //var MenusPadre_ = MenuService.GetMenuOthers(Id);
            var Tipos = TipoService.GetAll("Eval");
            //ViewBag.MenusPadre = MenusPadre_;
            ViewBag.Tipos = Tipos;

            if (!Id.HasValue) return View();
            var Eval_ = EvaluacionService.FindById(Id.Value);

            return Eval_ != null ? View(Eval_) : View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult ObtenerEmpleados(string Nombre, string Cargo, int AreaId, int JefeId, bool Activo)
        {
            var Empleados = EmpleadoService.GetByStateAndNameAndDescription(Nombre, Cargo, AreaId, JefeId, Activo);
            //var btnEdit = "<a href='javascript:void(0)' class='pl8 pr8 link-action edit' title='Editar' data-id='{0}'><i class='icon-edit'></i></a>";
            //var btnRemove = "<a href='javascript:void(0)' class='pl8 pr8 link-action remove' title='Borrar' data-id='{0}'><i class='icon-remove'></i></a>";
            var CheckSelect= "<a  href='javascript:void(0)'class='pl8 pr8 link-action check' title='Check' data-id='{0}'><input type='checkbox' name='Seleccionar' value='{0}'></a>";

            var Empleados_ = Empleados.Select(X => new
            {
                Id = X.id,
                X.nombre,
                id_Aera = X.Area.nombre,
                X.cargo,
                //X.id_Empleado_Jefe,
                //id_Empleado_Jefe = EmpleadoService.FindById(X.id_Empleado_Jefe)!=null? EmpleadoService.FindById(X.id_Empleado_Jefe).nombre:string.Empty,//EmpleadoService.FindById(X.id_Empleado_Jefe).nombre ? EmpleadoService.FindById(X.id_Empleado_Jefe).nombre:"",
                id_Empleado_Jefe = X.Empleado2 != null ? X.Empleado2.nombre : "Sin Jefe",
                X.id_Usuario,
                X.Usuario.Email,
                Estado = X.activo ? "Activo" : "Inactivo",
                Acciones = string.Format(CheckSelect, X.id)
            }).ToList();

            return JsonOutPut(ResultJson.MensajeDataExito("", Empleados_));
        }


        


        //[HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult GetEvaluacion(string Nombre, string Descripcion, int Tipo, bool? Estado)
        {
            var Evaluacion_ = EvaluacionService.GetEvaluacion(Nombre, Descripcion, Tipo, Estado);

            var btnAssignPersona = "<a href='javascript:void(0)' class='pl8 pr8 link-action assignpersona' title='Gestionar Personas' data-id='{0}'><i class='icon-tasks'></i></a>";
            var btnVerPreguntasEvaluacion = "<a href='javascript:void(0)' class='pl8 pr8 link-action verpreguntasevaluacion' title='Ver Evaluación' data-id='{0}'><i class='icon-zoom-in'></i></a>";
            var btnEnviarEvaluacion = "<a href='javascript:void(0)' class='pl8 pr8 link-action enviarevaluacion' title='Enviar Evaluación' data-id='{0}'><i class='icon-search'></i></a>";
            var btnEdit = "<a href='javascript:void(0)' class='pl8 pr8 link-action edit' title='Editar' data-id='{0}'><i class='icon-edit'></i></a>";
            var btnRemove = "<a href='javascript:void(0)' class='pl8 pr8 link-action remove' title='Borrar' data-id='{0}'><i class='icon-remove'></i></a>";

         

            var Evaluacion__ = Evaluacion_.Select(X => new
            {
                Id=X.id,
                X.id,
                //MenuPadre = X.Menu2 != null ? X.Menu2.Texto : "",
                X.nombre,
                //Icono = string.Format("<i class='{0} fs18 link-action'></i>", X.Icono),
                X.descripcion,
                //.FirstOrDefault(X => X.nombre.Trim().ToLower() == Nombre.Trim()
                tipoEval = TipoService.FindById(X.tipoEval).nombre.Trim(),
                fechaCreacion= X.fechaCreacion.ToString(),
                fechaModificacion= X.fechaModificacion.ToString(),
                Acciones = string.Format(btnAssignPersona + btnEnviarEvaluacion + btnVerPreguntasEvaluacion + btnEdit + btnRemove, X.id)
            }).ToList();

            return JsonOutPut(ResultJson.MensajeDataExito("", Evaluacion__));
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int? Id = null)
        {
            //var MenusPadre_ = MenuService.GetMenuOthers(Id);
            var Tipos = TipoService.GetAll("Eval");
            //ViewBag.MenusPadre = MenusPadre_;
            ViewBag.Tipos = Tipos;
           
            if (!Id.HasValue) return View();
            var Eval_ = EvaluacionService.FindById(Id.Value);

            return Eval_ != null ? View(Eval_) : View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Form(int Id, string Nombre, string Descripcion, int TipoEvaluacion, bool? Estado = null)
        {
            
            try
            {
                if (Id > 0)
                {
                    var Eval_ = EvaluacionService.Edit(Id, Nombre, Descripcion, TipoEvaluacion,   Estado);

                    return Eval_ ?
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Evaluación <strong>{0}</strong> Editada Correctamente.", Nombre  ))) :
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Editar la evaluación."));
                }
                else
                {
                    var Eval_ = EvaluacionService.New(Nombre, Descripcion, TipoEvaluacion);

                    return Eval_ == null ?
                        JsonOutPut(ResultJson.MensajeError("Ocurrió un error al intentar Crear una nueva Evaluación.")) :
                        JsonOutPut(ResultJson.MensajeExito(string.Format("Evaluación <strong>{0}</strong> Creado Correctamente.", Nombre)));
                }
            }
            catch (Exception Ex)
            {
                return JsonOutPut(ResultJson.MensajeError(Ex.Message));
            }
        }

        [HttpGet, AuthorizeRole(Roles = "Administrador")]
        public ActionResult AssignPersona(int Id)
        {
            var personasUnAssigned = EmpleadoService.GetUnAssignedEvaluacion(Id);
            var personasAssigned = EmpleadoService.GetAssignedEvaluacion(Id);

            ViewBag.EmpleadosUnAssigned = personasUnAssigned;
            ViewBag.EmpleadosAssigned = personasAssigned;

            return View();
        }

        [HttpPost, AuthorizeRole(Roles = "Administrador"), ActionName("AssignPersona")]
        public ActionResult AssignPersonaPost(string Personas, int idEvaluacion)
        {
            var Result = EvaluacionService.AssignEmpleados(idEvaluacion, Personas);

            return JsonOutPut(!Result ? ResultJson.MensajeError("Ocurrió un Error al Asignar/Desasignar Persona(s).") : ResultJson.MensajeExito("Cambios Realizados Correctamente."));
        }

        public ActionResult EnviarCorreo(string Personas, int idEvaluacion)
        {
            var empleadosIds = Personas.Split(',');
            if (!empleadosIds.Any()) return JsonOutPut( ResultJson.MensajeError("Ocurrió un error al seleccionar los invitados.") ); 

          var Evaluadores= EvaluacionService.RegistrarEvaluacion(idEvaluacion, Personas);

            //var Result = true;

            var Result = true;

            foreach (var EvaluadorUser in Evaluadores)
            {
                 Result = EnviarEmail(EvaluadorUser.Email);
            }
      


          

            return JsonOutPut(!Result ? ResultJson.MensajeError("Ocurrió un error al enviar email(s).") : ResultJson.MensajeExito("Cambios Realizados Correctamente."));
        }


        public bool sendMailUsers()
        {
            return true;

        }


        public bool EnviarEmail(string CorreoElectronico)
        {
            try
            {
                if (string.IsNullOrEmpty(CorreoElectronico)) throw new Exception("Debe ingresar su Correo Electrónico.");

                var Usuario_ = UserService.GetByEMail(CorreoElectronico);

                if (Usuario_ == null) throw new Exception(string.Format("El Usuario con Correo Electrónico <strong>{0}</strong> no existe.", CorreoElectronico));

                var Email_ = EmailService.GetById((int)EmailTemplate.NotificacionEvaluador);
                if (Email_ == null) throw new Exception("No se encontró una <strong>Plantilla de Correo<strong> para esta Solicitud.");
                var NombreEvaluacion = "";//Membership.GeneratePassword(8, 0);

                var NombreReceptor = string.Format("{0} {1} {2}", Usuario_.Nombre.Trim(), Usuario_.Apellido_Paterno.Trim(), Usuario_.Apellido_Materno.Trim());
                var DireccionReceptor = new List<string> { Usuario_.Email };
                var Mensaje = Email_.Cuerpo;
                Mensaje = Mensaje.Replace("{NombreDestinatario}", NombreReceptor);
                Mensaje = Mensaje.Replace("{NombreEvaluado}", "NombreReceptor"); 
                Mensaje = Mensaje.Replace("{LinkAplicacion}", "http://memnershiprh.gear.host/");
                Mensaje = Mensaje.Replace("{NombreEvaluacion}", NombreEvaluacion);

                var EmailSent = Template.Engine.Helper.Email.EnviarEmail(Email_.NombreRemitente, Email_.DireccionRemitente, Email_.Asunto, NombreReceptor, DireccionReceptor, Mensaje);

                if (!EmailSent) return false;

                //NewPassword = Coder.Encode(NewPassword);
                //var Result = UserService.ChangePassword(Usuario_.Id, NewPassword, true);

                    //if (Result) this.Flash("Su <strong>Nueva Contraseña</strong> ha sido enviada a su <strong>Correo Electrónico</strong><br><span>Recuerde por seguridad Actualizarla lo antes posible</span>.");
                    //else throw new Exception("Ocurrió un <strong>Error</strong> al restablecer su <strong>Contraseña</strong>, por favor contacte a su <strong>Adminsitrador</strong>.");

                return true ;
            }
            catch (Exception Ex)
            {
                this.Flash(Ex.GetBaseException().Message, MessageType.Error);
                return true;
            }
        }



        [HttpPost, AuthorizeRole(Roles = "Administrador")]
        public ActionResult Remove(int Id)
        {
            var Result = EvaluacionService.Remove(Id);

            return JsonOutPut(!Result ? ResultJson.MensajeError("Ocurrió un Error al Remover la Evaluación.") : ResultJson.MensajeExito("Evaluación ha sido Removida Correctamente."));
        }
    }
}
