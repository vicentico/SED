using System;
using System.Collections.Generic;
using System.Linq;
using Template.Domain.Model;

namespace Template.Service.DAO
{
    public static class PreguntaService
    {
        public static IEnumerable<Pregunta> GetByStateAndNameAndDescription(bool? Activo = true, string Titulo = "", string Descripcion = "")
        {
            using (var Dc = new TemplateEntities())
            {
                var Preguntas = Dc.Pregunta.AsQueryable();

                if (Activo.HasValue) Preguntas = Preguntas.Where(x => x.activo == Activo.Value);
                if (!string.IsNullOrEmpty(Titulo)) Preguntas = Preguntas.Where(x => x.titulo.Contains(Titulo));
                if (!string.IsNullOrEmpty(Descripcion)) Preguntas = Preguntas.Where(x => x.descripcion.Contains(Descripcion));

                var Preguntas_ = Preguntas.ToList();

                return Preguntas_;
            }
        }

        public static Pregunta New(string Nombre, string Descripcion, int tipo)
        {
            using (var Dc = new TemplateEntities())
            {
                var Pregunta = Dc.Pregunta.FirstOrDefault(x => x.titulo.Trim().ToLower() == Nombre.Trim().ToLower());

                if (Pregunta != null) throw new Exception(string.Format("No se puede crear el Pregunta con Nombre <strong>{0}</strong> porque ya existe.", Nombre));

                Pregunta = new Pregunta
                {
                    titulo = Nombre.Trim(),
                    descripcion = Descripcion.Trim(),
                    activo = true,
                    fechaCreacion = DateTime.Now,
                    fechaModificacion = DateTime.Now,
                    tipoPregunta = tipo
                };

                Dc.Pregunta.Add(Pregunta);
                var result = Dc.SaveChanges();

                return result > 0 ? Pregunta : null;
            }
        }

        public static bool Edit(int Id, int tipo, string Titulo, string Descripcion, int? Orden, bool? Activo = null)
        {
            using (var Dc = new TemplateEntities())
            {
                var Pregunta = Dc.Pregunta.Find(Id);

                if (Pregunta == null) throw new Exception("No se pudo <strong>Encontrar</strong> la pregunta para su <strong>Edición</strong>.");
                var Pregunta_ = Dc.Pregunta.FirstOrDefault(X => X.titulo.Trim().ToLower() == Titulo.Trim().ToLower() && X.id != Id);
                if (Pregunta_ != null) throw new Exception("No se puede <strong>Editar</strong> la Pregunta porque <strong>Ya Existe</strong> otra Pregunta con el mismo <strong>Titulo</strong>.");

                Pregunta.titulo = Titulo.Trim();
                Pregunta.descripcion = Descripcion.Trim();
                if (Activo.HasValue) Pregunta.activo = Activo.Value;
                Pregunta.tipoPregunta = tipo;
                Pregunta.fechaModificacion = DateTime.Now;

                Dc.Pregunta.Attach(Pregunta);
                var role2 = Dc.Entry(Pregunta);

                role2.Property(X => X.titulo).IsModified = true;
                role2.Property(X => X.descripcion).IsModified = true;
                role2.Property(X => X.activo).IsModified = true;
                role2.Property(X => X.tipoPregunta).IsModified = true;
                role2.Property(X => X.fechaModificacion).IsModified = true;
                var result = Dc.SaveChanges();

                return result > 0;
            }
        }
                               

        public static bool Remove(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Pregunta = Dc.Pregunta.Include("Evaluacion").FirstOrDefault(X => X.id == Id);

                if (Pregunta == null) throw new Exception("No se puede Borrar la Pregunta porque <strong>no existe</strong>.");
                if (Pregunta.Eval_Pregunta.Count > 0) throw new Exception("No se puede Borrar la Pregunta porque tiene <strong>Evaluaciones Asociados</strong>.");
                

               // if (Pregunta.tipoPregunta.Count > 0) throw new Exception("No se puede Borrar el Rol porque tiene <strong>Cuentas de Usuarios Asociados</strong>.");

                Dc.Pregunta.Remove(Pregunta);

                var result = Dc.SaveChanges();

                return result > 0;
            }
        }




        public static Pregunta FindById(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Pregunta_ = Dc.Pregunta.Find(Id);

                return Pregunta_;
            }
        }

        public static List<Pregunta> GetPregunta( string Titulo, string Descripcion, bool? Estado)
        {
            using (var Dc = new TemplateEntities())
            {
                //var Pregunta_ = Dc.Pregunta.Include("Pregunta2").AsQueryable();
                var Pregunta_ = Dc.Pregunta.AsQueryable();
                //if (0!= id) Pregunta_ = Pregunta_.Where(X => X.id== id);
                if (!string.IsNullOrEmpty(Titulo)) Pregunta_ = Pregunta_.Where(X => X.descripcion.Contains(Titulo.Trim()));
                if (!string.IsNullOrEmpty(Descripcion)) Pregunta_ = Pregunta_.Where(X => X.descripcion.Contains(Descripcion.Trim()));
                //if (0 != Tipo) Pregunta_ = Pregunta_.Where(X => X.tipoPregunta == Tipo);
                if (Estado.HasValue) Pregunta_ = Pregunta_.Where(X => X.activo == Estado.Value);

                return Pregunta_.ToList();
            }
        }

        

       public static List<Pregunta> VerPreguntasEvaluacion(int Id,  bool? Estado=null)
        {
          
            using (var Dc = new TemplateEntities())
            {
                //var Preguntas;
                //if (Estado!=null)
                //{
                //    var Preguntas = Dc
                //  .Pregunta
                //  .Include("Eval_Pregunta")
                //  .Where(X =>
                //      X.Eval_Pregunta.Any(Y => Y.idEvaluacion == Id) && X.activo ==Estado
                //  )
                //  .ToList();
                //}
                //else
                //{}
                    var Preguntas = Dc
                       .Pregunta
                       .Include("Eval_Pregunta")
                       .Where(X =>
                           X.Eval_Pregunta.Any(Y => Y.idEvaluacion == Id) //&& Y.activo ==Estado
                       )
                       .ToList();

                return Preguntas.ToList();
            }

        }


        public static List<Pregunta> GetAll()
        {
            using (var Dc = new TemplateEntities())
            {
                var Pregunta = Dc.Pregunta.AsQueryable();

                Pregunta = Pregunta.Where(x => x.activo == true);
               

                var Pregunta_ = Pregunta.ToList();

                return Pregunta_;

            }
        }



        public static bool AssignEvaluacion(int Id, string Evaluaciones)
        {
            var EvaluacionesIds = Evaluaciones.Split(',');

            using (var Dc = new TemplateEntities())
            {

                var Eval_Preguntas_ = Dc
                    .Eval_Pregunta
                    .Where(X =>
                        X.idPregunta == Id
                    ).ToList();

                if (Eval_Preguntas_.Count > 0)
                {
                    Dc.Eval_Pregunta.RemoveRange(Eval_Preguntas_);
                    Dc.SaveChanges();
                }

                if (!EvaluacionesIds.Any()) return true;
                var Eval_Pregunta_ = new List<Eval_Pregunta>();

                foreach (var evaluacion_ in EvaluacionesIds)
                {
                    if (string.IsNullOrEmpty(evaluacion_)) continue;

                    var Eval_Preguntas = new Eval_Pregunta
                    {
                        idEvaluacion = Convert.ToInt32(evaluacion_),
                        idPregunta = Id
                    };

                    Eval_Pregunta_.Add(Eval_Preguntas);
                }

                if (Eval_Pregunta_.Count > 0)
                {
                    Dc.Eval_Pregunta.AddRange(Eval_Pregunta_);
                    var Result = Dc.SaveChanges();

                    return Result > 0;
                }

                return true;
            }
        }




    }
}
