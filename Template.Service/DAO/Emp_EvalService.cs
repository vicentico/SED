using System;
using System.Collections.Generic;
using System.Linq;
using Template.Domain.Model;

namespace Template.Service.DAO
{
    public static class Emp_EvalService
    {



        public static Emp_Eval FindById(short[] Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Evaluacion = Dc.Emp_Eval.Find(Id);

                return Evaluacion;
            }
        }

        

    public static IEnumerable<Evaluacion> GetByStateAndNameAndDescription(bool? Activo = true, string Nombre = "", string Descripcion = "", int tipo = 0)
        {
            using (var Dc = new TemplateEntities())
            {
                var Evaluacions = Dc.Evaluacion.AsQueryable();

                if (Activo.HasValue) Evaluacions = Evaluacions.Where(x => x.activo == Activo.Value);
                if (!string.IsNullOrEmpty(Nombre)) Evaluacions = Evaluacions.Where(x => x.nombre.Contains(Nombre));
                if (!string.IsNullOrEmpty(Descripcion)) Evaluacions = Evaluacions.Where(x => x.descripcion.Contains(Descripcion));
                if (0 != tipo) Evaluacions = Evaluacions.Where(x => x.tipoEval == tipo);

                var Evaluacions_ = Evaluacions.ToList();

                return Evaluacions_;
            }
        }

        public static Evaluacion New(string Nombre, string Descripcion, int TipoEvaluacion)
        {
            using (var Dc = new TemplateEntities())
            {
                var Evaluacion = Dc.Evaluacion.FirstOrDefault(x => x.nombre.Trim().ToLower() == Nombre.Trim().ToLower());

                if (Evaluacion != null) throw new Exception(string.Format("No se puede crear el Evaluacion con Nombre <strong>{0}</strong> porque ya existe.", Nombre));

                Evaluacion = new Evaluacion
                {
                    nombre = Nombre.Trim(),
                    descripcion = Descripcion.Trim(),
                    activo = true,
                    tipoEval = TipoEvaluacion,
                    fechaCreacion = DateTime.Now,
                    fechaModificacion = DateTime.Now
                };

                Dc.Evaluacion.Add(Evaluacion);
                var result = Dc.SaveChanges();

                return result > 0 ? Evaluacion : null;
            }
        }

        public static bool Edit(int Id, string Nombre, string Descripcion, int TipoEvaluacion, bool? Activo = null)
        {
            using (var Dc = new TemplateEntities())
            {
                var Evaluacion = Dc.Evaluacion.Find(Id);

                if (Evaluacion == null) throw new Exception("No se pudo <strong>Encontrar</strong> el Rol para su <strong>Edición</strong>.");
                var Evaluacion_ = Dc.Evaluacion.FirstOrDefault(X => X.nombre.Trim().ToLower() == Nombre.Trim().ToLower() && X.id != Id);
                if (Evaluacion_ != null) throw new Exception("No se puede <strong>Editar</strong> el Rol porque <strong>Ya Existe</strong> otro Rol con el mismo <strong>Nombre</strong>.");

                Evaluacion.nombre = Nombre.Trim();
                Evaluacion.descripcion = Descripcion.Trim();
                if (Activo.HasValue) Evaluacion.activo = Activo.Value;
                Evaluacion.tipoEval = TipoEvaluacion;
                Evaluacion.fechaModificacion = DateTime.Now;
                Dc.Evaluacion.Attach(Evaluacion);
                var role2 = Dc.Entry(Evaluacion);

                role2.Property(X => X.nombre).IsModified = true;
                role2.Property(X => X.descripcion).IsModified = true;
                role2.Property(X => X.activo).IsModified = true;
                role2.Property(X => X.tipoEval).IsModified = true;

                var result = Dc.SaveChanges();

                return result > 0;
            }
        }

        public static List<Evaluacion> GetEvaluacion(string Nombre_, string Descripcion_, int tipo, bool? Estado)
        {
            using (var Dc = new TemplateEntities())
            {

                var Evaluaciones_ = Dc.Evaluacion.AsQueryable();

                if (!string.IsNullOrEmpty(Nombre_)) Evaluaciones_ = Evaluaciones_.Where(X => X.nombre.Contains(Nombre_.Trim()));
                if (!string.IsNullOrEmpty(Descripcion_)) Evaluaciones_ = Evaluaciones_.Where(X => X.descripcion.Contains(Descripcion_.Trim()));
                if (0 != tipo) Evaluaciones_ = Evaluaciones_.Where(X => X.tipoEval == tipo);
                if (Estado.HasValue) Evaluaciones_ = Evaluaciones_.Where(X => X.activo == Estado.Value);

                return Evaluaciones_.ToList();
            }
        }


        public static bool Remove(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Evaluacion = Dc.Evaluacion.Include("Evaluacion").FirstOrDefault(X => X.id == Id);

                if (Evaluacion == null) throw new Exception("No se puede Borrar el Evaluacion porque <strong>no existe</strong>.");
                if (Evaluacion.Emp_Eval.Count > 0) throw new Exception("No se puede Borrar la Evaluación porque tiene <strong>Empleados Asignados</strong>.");
                if (Evaluacion.Eval_Pregunta.Count > 0) throw new Exception("No se puede Borrar el Rol porque tiene <strong>Preguntas Asociadas</strong>.");

                Dc.Evaluacion.Remove(Evaluacion);

                var result = Dc.SaveChanges();

                return result > 0;
            }
        }

        public static Evaluacion FindById(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Evaluacion = Dc.Evaluacion.Find(Id);

                return Evaluacion;
            }
        }

   

        public static List<Evaluacion> GetEvaluacionOthers(int? Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Evaluacion_ = Dc.Evaluacion.AsQueryable();
                if (Id.HasValue) Evaluacion_ = Evaluacion_.Where(x => x.id != Id.Value);

                return Evaluacion_.ToList();
            }
        }

        //public static bool AssignEmpleados(int Id, string Empleados)
        //{
        //    var empleadosIds = Empleados.Split(',');

        //    using (var Dc = new TemplateEntities())
        //    {

        //        var empEval = Dc
        //            .Emp_Eval
        //            .Where(x =>
        //                x.idEvaluacion == Id
        //            ).ToList();

        //        if (empEval.Count > 0)
        //        {
        //            Dc.Emp_Eval.RemoveRange(empEval);
        //            Dc.SaveChanges();
        //        }

        //        if (!empleadosIds.Any()) return true;
        //        var Emp_Eval_ = new List<Emp_Eval>();

        //        foreach (var Empleados_ in empleadosIds)
        //        {
        //            if (string.IsNullOrEmpty(Empleados_)) continue;

        //            var EmpEval = new Emp_Eval
        //            {
        //                idEmpleado = Convert.ToInt32(Empleados_),
        //                idEvaluacion = Id
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



        public static IEnumerable<Evaluacion> GetUnAssignedPregunta(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var evaluaciones = Dc
                    .Evaluacion
                    .Include("Eval_Pregunta")
                    .Where(X =>
                        X.activo
                        && X.Eval_Pregunta.All(y => y.idPregunta != Id)
                    )
                    .ToList();

                return evaluaciones;
            }
        }

        public static IEnumerable<Evaluacion> GetAssignedPregunta(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var evaluacion = Dc
                    .Evaluacion
                    .Include("Eval_Pregunta")
                    .Where(X =>
                        X.activo
                        && X.Eval_Pregunta.Any(y => y.idPregunta == Id)
                    )
                    .ToList();

                return evaluacion;
            }
        }


    }
}
