using System;
using System.Collections.Generic;
using System.Linq;
using Template.Domain.Model;

namespace Template.Service.DAO
{
    public static class RoleService
    {
        public static IEnumerable<Rol> GetByStateAndNameAndDescription(bool? Activo = true, string Nombre = "", string Descripcion = "")
        {
            using (var Dc = new TemplateEntities())
            {
                var Roles = Dc.Rol.AsQueryable();

                if (Activo.HasValue) Roles = Roles.Where(X => X.Activo == Activo.Value);
                if (!string.IsNullOrEmpty(Nombre)) Roles = Roles.Where(X => X.Nombre.Contains(Nombre));
                if (!string.IsNullOrEmpty(Descripcion)) Roles = Roles.Where(X => X.Descripcion.Contains(Descripcion));

                var Roles_ = Roles.ToList();
                
                return Roles_;
            }
        }

        public static Rol New(string Nombre, string Descripcion)
        {
            using (var Dc = new TemplateEntities())
            {
                var Rol = Dc.Rol.FirstOrDefault(X => X.Nombre.Trim().ToLower() == Nombre.Trim().ToLower());

                if (Rol != null) throw new Exception(string.Format("No se puede crear el Rol con Nombre <strong>{0}</strong> porque ya existe.", Nombre));

                Rol = new Rol
                {
                    Nombre = Nombre.Trim(),
                    Descripcion = Descripcion.Trim(),
                    Activo = true
                };

                Dc.Rol.Add(Rol);
                var Result = Dc.SaveChanges();

                return Result > 0 ? Rol : null;
            }
        }

        public static bool Remove(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Rol = Dc.Rol.Include("RolUsuario").Include("RolMenu").FirstOrDefault(X => X.Id == Id);

                if (Rol == null) throw new Exception("No se puede Borrar el Rol porque <strong>no existe</strong>.");
                if (Rol.RolMenu.Count > 0) throw new Exception("No se puede Borrar el Rol porque tiene <strong>Opciones de Menú Asociados</strong>.");
                if (Rol.RolUsuario.Count > 0) throw new Exception("No se puede Borrar el Rol porque tiene <strong>Cuentas de Usuarios Asociados</strong>.");

                Dc.Rol.Remove(Rol);

                var Result = Dc.SaveChanges();

                return Result > 0;
            }
        }

        public static Rol FindById(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Role = Dc.Rol.Find(Id);

                return Role;
            }
        }

        public static bool Edit(int Id, string Nombre, string Descripcion, bool? Activo = null)
        {
	        using (var Dc = new TemplateEntities())
            {
                var Role = Dc.Rol.Find(Id);

                if (Role == null) throw new Exception("No se pudo <strong>Encontrar</strong> el Rol para su <strong>Edición</strong>.");
                var Role_ = Dc.Rol.FirstOrDefault(X => X.Nombre.Trim().ToLower() == Nombre.Trim().ToLower() && X.Id != Id);
                if (Role_ != null) throw new Exception("No se puede <strong>Editar</strong> el Rol porque <strong>Ya Existe</strong> otro Rol con el mismo <strong>Nombre</strong>.");

                Role.Nombre = Nombre.Trim();
                Role.Descripcion = Descripcion.Trim();
                if (Activo.HasValue) Role.Activo = Activo.Value;

                Dc.Rol.Attach(Role);
                var Role2_ = Dc.Entry(Role);

                Role2_.Property(X => X.Nombre).IsModified = true;
                Role2_.Property(X => X.Descripcion).IsModified = true;
                Role2_.Property(X => X.Activo).IsModified = true;
                
                var Result = Dc.SaveChanges();

                return Result > 0;
            }
        }

	    public static IEnumerable<Rol> GetUnAssignedUser(int UserId)
	    {
			using (var Dc = new TemplateEntities())
			{
				var Roles = Dc
					.Rol
					.Include("RolUsuario")
					.Where(X => 
						X.Activo
						&& X.RolUsuario.All(Y => Y.Usuario_Id != UserId)
					)
					.ToList();
				
				return Roles;
			}
	    }

        public static IEnumerable<Rol> GetAssignedUser(int UserId)
        {
            using (var Dc = new TemplateEntities())
            {
                var Roles = Dc
                    .Rol
                    .Include("RolUsuario")
                    .Where(X =>
                        X.Activo
                        && X.RolUsuario.Any(Y => Y.Usuario_Id == UserId)
                    )
                    .ToList();

                return Roles;
            }
        }

        public static IEnumerable<Rol> GetUnAssignedMenu(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Roles = Dc
                    .Rol
                    .Include("RolMenu")
                    .Where(X =>
                        X.Activo
                        && X.RolMenu.All(Y => Y.Menu_Id != Id)
                    )
                    .ToList();

                return Roles;
            }
        }

        public static IEnumerable<Rol> GetAssignedMenu(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Roles = Dc
                    .Rol
                    .Include("RolMenu")
                    .Where(X =>
                        X.Activo
                        && X.RolMenu.Any(Y => Y.Menu_Id == Id)
                    )
                    .ToList();

                return Roles;
            }
        }
    }
}
