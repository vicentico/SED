using System;
using System.Collections.Generic;
using System.Linq;
using Template.Domain.Model;

namespace Template.Service.DAO
{
    public static class AreaService
    {
        public static IEnumerable<Area> GetByStateAndNameAndDescription(bool? Activo = true, string Nombre = "", string Descripcion = "")
        {
            using (var Dc = new TemplateEntities())
            {
                var Areas = Dc.Area.AsQueryable();

                if (Activo.HasValue) Areas = Areas.Where(x => x.activo == Activo.Value);
                if (!string.IsNullOrEmpty(Nombre)) Areas = Areas.Where(x => x.nombre.Contains(Nombre));
                if (!string.IsNullOrEmpty(Descripcion)) Areas = Areas.Where(x => x.descripcion.Contains(Descripcion));

                var Areas_ = Areas.ToList();

                return Areas_;
            }
        }

        public static Area New(string Nombre, string Descripcion)
        {
            using (var Dc = new TemplateEntities())
            {
                var Area = Dc.Area.FirstOrDefault(x => x.nombre.Trim().ToLower() == Nombre.Trim().ToLower());

                if (Area != null) throw new Exception(string.Format("No se puede crear el Area con Nombre <strong>{0}</strong> porque ya existe.", Nombre));

                Area = new Area
                {
                    nombre = Nombre.Trim(),
                    descripcion = Descripcion.Trim(),
                    activo = true
                   
                };

                Dc.Area.Add(Area);
                var result = Dc.SaveChanges();

                return result > 0 ? Area : null;
            }
        }

        public static bool Edit(int Id, string Nombre, string Descripcion, bool? Activo = null)
        {
            using (var Dc = new TemplateEntities())
            {
                var Area = Dc.Area.Find(Id);

                if (Area == null) throw new Exception("No se pudo <strong>Encontrar</strong> el Rol para su <strong>Edición</strong>.");
                var Area_ = Dc.Area.FirstOrDefault(X => X.nombre.Trim().ToLower() == Nombre.Trim().ToLower() && X.id != Id);
                if (Area_ != null) throw new Exception("No se puede <strong>Editar</strong> el Rol porque <strong>Ya Existe</strong> otro Rol con el mismo <strong>Nombre</strong>.");

                Area.nombre = Nombre.Trim();
                Area.descripcion = Descripcion.Trim();
                if (Activo.HasValue) Area.activo = Activo.Value;

                Dc.Area.Attach(Area);
                var role2 = Dc.Entry(Area);

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
                var Area = Dc.Area.Include("Evaluacion").FirstOrDefault(X => X.id == Id);

                if (Area == null) throw new Exception("No se puede Borrar el Area porque <strong>no existe</strong>.");
                if (Area.Empleado.Count > 0) throw new Exception("No se puede Borrar el Area porque tiene <strong>Empleados Asociados</strong>.");
                //if (Area.Pregunta.Count > 0) throw new Exception("No se puede Borrar el Rol porque tiene <strong>Cuentas de Usuarios Asociados</strong>.");

                Dc.Area.Remove(Area);

                var result = Dc.SaveChanges();

                return result > 0;
            }
        }

        public static Area FindById(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Area = Dc.Area.Find(Id);

                return Area;
            }
        }

        public static List<Area> GetAll()
        {
            using (var Dc = new TemplateEntities())
            {
                var Areas = Dc.Area.AsQueryable();

                Areas = Areas.Where(x => x.activo == true);
               
                var Areas_ = Areas.ToList();

                return Areas_;

            }
        }
    }
}
