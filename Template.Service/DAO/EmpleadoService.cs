
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Template.Domain.Model;

namespace Template.Service.DAO
{
    public static class EmpleadoService
    {
        public static IEnumerable<Empleado> GetByStateAndNameAndDescription( string   Nombre , string Cargo , int AreaId, int JefeId, bool? Activo = true)
        {
            using (var Dc = new TemplateEntities())
            {
              

                var Empleados = Dc.Empleado.Include("Empleado2").Include("Area").Include("Usuario").AsQueryable();
                


                if (Activo.HasValue) Empleados = Empleados.Where(x => x.activo == Activo.Value);
                if (!string.IsNullOrEmpty(Nombre)) Empleados = Empleados.Where(x => x.nombre.Contains(Nombre));
                if (!string.IsNullOrEmpty(Cargo)) Empleados = Empleados.Where(x => x.cargo.Contains(Cargo));
                if (0 != AreaId) Empleados = Empleados.Where(x => x.id_Aera== AreaId);
                if (0 != JefeId) Empleados = Empleados.Where(x => x.id_Empleado_Jefe == JefeId);

                //var Empleados_ = Empleados.ToList();

                return Empleados.ToList(); ;
            }
        }

        public static Empleado New(string Nombre, string Cargo, int idAera, int idUsuario, int? idEmpleadoJefe = null)
        {
            using (var Dc = new TemplateEntities())
            {
                var Empleado = Dc.Empleado.FirstOrDefault(x => x.nombre.Trim().ToLower() == Nombre.Trim().ToLower());

                if (Empleado != null) throw new Exception(string.Format("No se puede crear el Empleado con Nombre <strong>{0}</strong> porque ya existe.", Nombre));

                Empleado = new Empleado
                {
                    nombre = Nombre.Trim(),
                    cargo = Cargo.Trim(),
                    activo = true,
                    id_Aera = idAera,
                    id_Empleado_Jefe = idEmpleadoJefe,
                    id_Usuario = idUsuario
                };

                Dc.Empleado.Add(Empleado);
                var result = Dc.SaveChanges();

                return result > 0 ? Empleado : null;
            }
        }

        public static bool Edit(int Id, string Nombre, string Cargo ,int idAera, int idUsuario, int? idEmpleadoJefe = null, bool? Activo = null)
        {
            using (var Dc = new TemplateEntities())
            {
                var Empleado = Dc.Empleado.Find(Id);

                if (Empleado == null) throw new Exception("No se pudo <strong>Encontrar</strong> el Empleado para su <strong>Edición</strong>.");
                var Empleado_ = Dc.Empleado.FirstOrDefault(X => X.nombre.Trim().ToLower() == Nombre.Trim().ToLower() && X.id != Id);
                if (Empleado_ != null) throw new Exception("No se puede <strong>Editar</strong> el Empleado porque <strong>Ya Existe</strong> otro Rol con el mismo <strong>Nombre</strong>.");
                if (idEmpleadoJefe != null) Empleado.id_Empleado_Jefe = idEmpleadoJefe;
                Empleado.nombre = Nombre.Trim();
                Empleado.cargo = Cargo.Trim();
                if (Activo.HasValue) Empleado.activo = Activo.Value;

                Dc.Empleado.Attach(Empleado);
                var role2 = Dc.Entry(Empleado);

                role2.Property(X => X.nombre).IsModified = true;
                role2.Property(X => X.cargo).IsModified = true;
                role2.Property(X => X.activo).IsModified = true;

                var result = Dc.SaveChanges();

                return result > 0;
            }
        }


        public static bool Remove(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Empleado = Dc.Empleado.Include("Evaluacion").FirstOrDefault(X => X.id == Id);

                if (Empleado == null) throw new Exception("No se puede Borrar el Empleado porque <strong>no existe</strong>.");
                if (Empleado.Emp_Eval.Count > 0) throw new Exception("No se puede Borrar el Empleado porque tiene <strong>Opciones de Evaluacion Asociados</strong>.");
                if (Empleado.Respuesta.Count > 0) throw new Exception("No se puede Borrar el Empleado porque tiene <strong>Respuestas Asociadas en alguna Evaluacion</strong>.");
                if (Empleado.Empleado1.Count > 0) throw new Exception("No se puede Borrar el Empleado porque es jefe de otros <strong>Empleados</strong>.");

                Dc.Empleado.Remove(Empleado);

                var result = Dc.SaveChanges();

                return result > 0;
            }
        }

        public static Empleado FindById(int? Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Empleado = Dc.Empleado.Find(Id);

                return Empleado;
            }
        }
        public static Empleado ObtenerJefatura(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var empleado = Dc
                .Empleado
                .FirstOrDefault(x =>
                    x.id == Id
                    && x.activo
                );

                Empleado empleadoJ = null;
                if (empleado != null && 0 != empleado.id_Empleado_Jefe)
                {
                     empleadoJ = Dc
                        .Empleado
                        .FirstOrDefault(x =>
                            x.id == empleado.id_Empleado_Jefe
                            && x.activo
                        );

                   
                }
                //var Empleados__ = Dc.Empleado.Include("Empleado2").AsQueryable();

                if (empleadoJ != null) return empleadoJ;
                
                return empleadoJ;

          
            }
        }


        public static Empleado GetByParent(int idEmpleadoJefe)
        {
            if (idEmpleadoJefe == 0) throw new Exception("Debe especificar el Identificador del Menú Padre.");

            using (var Dc = new TemplateEntities())
            {
                var empleado = Dc
                    .Empleado
                    .FirstOrDefault(x =>
                        x.id == idEmpleadoJefe
                        && x.activo
                    );

                return empleado;
            }
        }


        public static List<Empleado> GetParents()
        {
            using (var Dc = new TemplateEntities())
            {
                var Empleado_ = Dc
                    .Empleado
                    .Where(X =>
                        X.id_Empleado_Jefe == null
                        && X.activo
                    )
                    .ToList();

                return Empleado_;
            }
        }


        public static List<Empleado> GetChildren(int IdEmpleadoJefe)
        {
            using (var Dc = new TemplateEntities())
            {
                var Empleado_ = Dc
                    .Empleado
                    .Where(X =>
                        X.id_Empleado_Jefe == IdEmpleadoJefe
                        && X.activo
                    )
                    .ToList();

                return Empleado_;
            }
        }

        public static List<Empleado> GetMenuOthers(int? Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Empleado_ = Dc.Empleado.AsQueryable();
                if (Id.HasValue) Empleado_ = Empleado_.Where(X => X.id != Id.Value);

                return Empleado_.ToList();
            }
        }

        //public static bool AssignEvaluacion(int Id, string Evaluaciones)
        //{
        //    var EvaluacionesIds = Evaluaciones.Split(',');

        //    using (var Dc = new TemplateEntities())
        //    {

        //        var Emp_Eval = Dc
        //            .Emp_Eval
        //            .Where(X =>
        //                X.idEmpleado == Id
        //            ).ToList();

        //        if (Emp_Eval.Count > 0)
        //        {
        //            Dc.Emp_Eval.RemoveRange(Emp_Eval);
        //            Dc.SaveChanges();
        //        }

        //        if (!EvaluacionesIds.Any()) return true;
        //        var Emp_Eval_ = new List<Emp_Eval>();

        //        foreach (var Eval_ in EvaluacionesIds)
        //        {
        //            if (string.IsNullOrEmpty(Eval_)) continue;

        //            var EmpEval = new Emp_Eval
        //            {
        //                idEvaluacion = Convert.ToInt32(Eval_),
        //                idEmpleado = Id
        //            };

        //            Emp_Eval_.Add(EmpEval);
        //        }

        //        if (Emp_Eval_.Count > 0)
        //        {
        //            Dc.Emp_Eval.AddRange(Emp_Eval_);
        //            var result = Dc.SaveChanges();

        //            return result > 0;
        //        }

        //        return true;
        //    }
        //}


        public static IEnumerable<Empleado> GetUnAssignedEvaluacion(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Empleado = Dc
                    .Empleado
                    .Include("Emp_Eval")
                    .Where(X =>
                        X.activo
                        && X.Emp_Eval.All(Y => Y.idEvaluacion != Id)
                    )
                    .ToList();

                return Empleado;
            }
        }
        public static IEnumerable<Empleado> GetAssignedEvaluacion(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Personas = Dc
                    .Empleado
                    .Include("Emp_Eval")
                    .Where(X =>
                        X.activo
                        && X.Emp_Eval.Any(Y => Y.idEvaluacion == Id)
                    )
                    .ToList();

                return Personas;
            }
        }

        public static List<Empleado> GetAll()
        {
            using (var Dc = new TemplateEntities())
            {
                var Empleados = Dc.Empleado.AsQueryable();

                Empleados = Empleados.Where(x => x.activo == true);

                var Empleados_ = Empleados.ToList();

                return Empleados_;

            }
        }


    }
}
