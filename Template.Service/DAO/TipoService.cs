using System;
using System.Collections.Generic;
using System.Linq;
using Template.Domain.Model;

namespace Template.Service.DAO
{
    public static class TipoService
    {
        public static IEnumerable<Tipo> GetByStateAndNameAndDescription(bool? Activo = true, string Nombre = "", string Descripcion = "")
        {
            using (var Dc = new TemplateEntities())
            {
                var Tipos = Dc.Tipo.AsQueryable();

                if (Activo.HasValue) Tipos = Tipos.Where(x => x.activo == Activo.Value);
                if (!string.IsNullOrEmpty(Nombre)) Tipos = Tipos.Where(x => x.nombre.Contains(Nombre));
                if (!string.IsNullOrEmpty(Descripcion)) Tipos = Tipos.Where(x => x.descripcion.Contains(Descripcion));

                var Tipos_ = Tipos.ToList();

                return Tipos_;
            }
        }

        public static Tipo New(string Nombre, string Descripcion, string Categoria)
        {
            using (var Dc = new TemplateEntities())
            {
                var Tipo = Dc.Tipo.FirstOrDefault(x => x.nombre.Trim().ToLower() == Nombre.Trim().ToLower());

                if (Tipo != null) throw new Exception(string.Format("No se puede crear el Tipo con Nombre <strong>{0}</strong> porque ya existe.", Nombre));

                Tipo = new Tipo
                {
                    nombre = Nombre.Trim(),
                    descripcion = Descripcion.Trim(),
                    activo = true,
                    categoria= Categoria
                };

                Dc.Tipo.Add(Tipo);
                var result = Dc.SaveChanges();

                return result > 0 ? Tipo : null;
            }
        }

        public static bool Edit(int Id, string Nombre, string Descripcion, bool? Activo = null)
        {
            using (var Dc = new TemplateEntities())
            {
                var Tipo = Dc.Tipo.Find(Id);

                if (Tipo == null) throw new Exception("No se pudo <strong>Encontrar</strong> el Rol para su <strong>Edición</strong>.");
                var Tipo_ = Dc.Tipo.FirstOrDefault(X => X.nombre.Trim().ToLower() == Nombre.Trim().ToLower() && X.id != Id);
                if (Tipo_ != null) throw new Exception("No se puede <strong>Editar</strong> el Rol porque <strong>Ya Existe</strong> otro Rol con el mismo <strong>Nombre</strong>.");

                Tipo.nombre = Nombre.Trim();
                Tipo.descripcion = Descripcion.Trim();
                if (Activo.HasValue) Tipo.activo = Activo.Value;

                Dc.Tipo.Attach(Tipo);
                var role2 = Dc.Entry(Tipo);

                role2.Property(X => X.nombre).IsModified = true;
                role2.Property(X => X.descripcion).IsModified = true;
                role2.Property(X => X.activo).IsModified = true;

                var result = Dc.SaveChanges();

                return result > 0;
            }
        }


        public static bool Remove(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Tipo = Dc.Tipo.Include("Evaluacion").FirstOrDefault(X => X.id == Id);

                if (Tipo == null) throw new Exception("No se puede Borrar el Tipo porque <strong>no existe</strong>.");
                if (Tipo.Evaluacion.Count > 0) throw new Exception("No se puede Borrar el Tipo porque tiene <strong>Opciones de Evaluacion Asociados</strong>.");
                //if (Tipo.Pregunta.Count > 0) throw new Exception("No se puede Borrar el Rol porque tiene <strong>Cuentas de Usuarios Asociados</strong>.");

                Dc.Tipo.Remove(Tipo);

                var result = Dc.SaveChanges();

                return result > 0;
            }
        }

        public static Tipo FindById(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var tipo = Dc.Tipo.Find(Id);

                return tipo;
            }
        }

        public static List<Tipo> GetAll(string categoria)
        {
            using (var Dc = new TemplateEntities())
            {
                var Tipos = Dc.Tipo.AsQueryable();

                 Tipos = Tipos.Where(x => x.activo == true);
                if (!string.IsNullOrEmpty(categoria)) Tipos = Tipos.Where(x => x.categoria.Contains(categoria));
               
                var Tipos_ = Tipos.ToList();

                return Tipos_;
                
            }
        }
    }
}
