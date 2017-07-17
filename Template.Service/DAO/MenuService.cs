using System;
using System.Collections.Generic;
using System.Linq;
using Template.Domain.Model;

namespace Template.Service.DAO
{
	public static class MenuService
	{
		public static Menu GetByUrl(string Url_)
		{
			if (string.IsNullOrEmpty(Url_)) throw new Exception("Debe especificar la Url del Menú.");

            using (var Dc = new TemplateEntities())
			{
				var Menu_ = Dc
					.Menu
					.FirstOrDefault(X =>
						X.Url == Url_
						&& X.Activo
					);

				return Menu_;
			}
		}

		public static Menu GetByParent(int IdMenuPadre)
		{
			if (IdMenuPadre == 0) throw new Exception("Debe especificar el Identificador del Menú Padre.");

			using (var Dc = new TemplateEntities())
			{
				var Menu_ = Dc
					.Menu
					.FirstOrDefault(X =>
						X.Id == IdMenuPadre
						&& X.Activo
					);

				return Menu_;
			}
		}

        public static List<Menu> GetParents()
        {
            using (var Dc = new TemplateEntities())
            {
                var Menu_ = Dc
                    .Menu
                    .Where(X => 
                        X.Menu_Id == null
                        && X.Activo
                    )
                    .ToList();

                return Menu_;
            }
        }

        public static List<Menu> GetChildren(int Menu_Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Menu_ = Dc
                    .Menu
                    .Where(X =>
                        X.Menu_Id == Menu_Id
                        && X.Activo
                    )
                    .ToList();

                return Menu_;
            }
        }

	    public static List<Menu> GetMenu(string Url_, string Texto, string Ayuda, bool? Estado)
	    {
            using (var Dc = new TemplateEntities())
            {
                var Menu_ = Dc.Menu.Include("Menu2").AsQueryable();

                if (!string.IsNullOrEmpty(Url_)) Menu_ = Menu_.Where(X => X.Url.Contains(Url_.Trim()));
                if (!string.IsNullOrEmpty(Texto)) Menu_ = Menu_.Where(X => X.Texto.Contains(Texto.Trim()));
                if (!string.IsNullOrEmpty(Ayuda)) Menu_ = Menu_.Where(X => X.Ayuda.Contains(Ayuda.Trim()));
                if (Estado.HasValue) Menu_ = Menu_.Where(X => X.Activo == Estado.Value);

                return Menu_.ToList();
            }
	    }

	    public static Menu FindById(int Id)
	    {
            using (var Dc = new TemplateEntities())
            {
                var Menu_ = Dc.Menu.Find(Id);

                return Menu_;
            }
	    }

	    public static List<Menu> GetMenuOthers(int? Id)
	    {
            using (var Dc = new TemplateEntities())
            {
                var Menu_ = Dc.Menu.AsQueryable();
                if (Id.HasValue) Menu_ = Menu_.Where(X => X.Id != Id.Value);
                
                return Menu_.ToList();
            }
	    }

	    public static Menu New(string Url_, string Icono, string Texto, string Ayuda, int? MenuId, int? Orden)
	    {
            using (var Dc = new TemplateEntities())
            {
                var Menu_ = Dc.Menu.AsQueryable();
                
                Menu_ = Menu_.Where(X => X.Texto.Trim().ToLower() == Texto.Trim().ToLower());
                if (MenuId.HasValue) Menu_ = Menu_.Where(X => X.Menu_Id == MenuId);
                
                var Menu__ = Menu_.FirstOrDefault();

                if (Menu__ != null) throw new Exception(string.Format("No se puede crear el Menú con el Texto <strong>{0}</strong> porque ya existe.", Texto));

                Menu__ = new Menu
                {
                    Url = Url_.Trim(),
                    Icono = Icono.Trim(),
                    Texto = Texto.Trim(),
                    Ayuda = Ayuda.Trim(),
                    Activo = true
                };

                if (MenuId.HasValue) Menu__.Menu_Id = MenuId.Value;
                if (Orden.HasValue) Menu__.Orden = Orden.Value;

                Dc.Menu.Add(Menu__);
                var Result = Dc.SaveChanges();

                return Result > 0 ? Menu__ : null;
            }
	    }

	    public static bool Edit(string Url, string Icono, string Texto, string Ayuda, int Id, int? MenuId, int? Orden, bool? Estado)
	    {
            using (var Dc = new TemplateEntities())
            {
                var Menu_ = Dc.Menu.Find(Id);

                if (Menu_ == null) throw new Exception("No se pudo <strong>Encontrar</strong> el Menú para su <strong>Edición</strong>.");

                var Menu__ = Dc.Menu.AsQueryable();
                Menu__ = Menu__.Where(X => X.Texto.Trim().ToLower() == Texto.Trim().ToLower() && X.Id != Id);
                Menu__ = Menu__.Where(X => X.Menu_Id == MenuId);

                var _Menu = Menu__.FirstOrDefault();

                if (_Menu != null) throw new Exception("No se puede <strong>Editar</strong> el Menú porque <strong>Ya Existe</strong> otro Menú con el mismo <strong>Texto</strong>.");

                Menu_.Url = Url.Trim();
                Menu_.Icono = Icono.Trim();
                Menu_.Texto = Texto.Trim();
                Menu_.Ayuda = Ayuda.Trim();
                Menu_.Menu_Id = MenuId;
                if (Estado.HasValue) Menu_.Activo = Estado.Value;

                Dc.Menu.Attach(Menu_);
                var Menu2 = Dc.Entry(Menu_);

                Menu2.Property(X => X.Url).IsModified = true;
                Menu2.Property(X => X.Icono).IsModified = true;
                Menu2.Property(X => X.Texto).IsModified = true;
                Menu2.Property(X => X.Ayuda).IsModified = true;
                Menu2.Property(X => X.Menu_Id).IsModified = true; 
                if (Estado.HasValue) Menu2.Property(X => X.Activo).IsModified = true;
                
                var Result = Dc.SaveChanges();

                return Result > 0;
            }
	    }

	    public static bool AssignRole(int Id, string Roles)
	    {
            var RolesIds = Roles.Split(',');

            using (var Dc = new TemplateEntities())
            {

                var MenuRoles = Dc
                    .RolMenu
                    .Where(X =>
                        X.Menu_Id == Id
                    ).ToList();

                if (MenuRoles.Count > 0)
                {
                    Dc.RolMenu.RemoveRange(MenuRoles);
                    Dc.SaveChanges();
                }

                if (!RolesIds.Any()) return true;
                var MenuRoles_ = new List<RolMenu>();

                foreach (var Role_ in RolesIds)
                {
                    if (string.IsNullOrEmpty(Role_)) continue;

                    var MenuRole = new RolMenu
                    {
                        Rol_Id = Convert.ToInt32(Role_),
                        Menu_Id = Id
                    };

                    MenuRoles_.Add(MenuRole);
                }

                if (MenuRoles_.Count > 0)
                {
                    Dc.RolMenu.AddRange(MenuRoles_);
                    var Result = Dc.SaveChanges();

                    return Result > 0;
                }

                return true;
            }
	    }

        public static bool Remove(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Menu_ = Dc.Menu.Include("RolMenu").FirstOrDefault(X => X.Id == Id);

                if (Menu_ == null) throw new Exception("No se puede Borrar el Menú porque <strong>no existe</strong>.");
                if (Menu_.RolMenu.Count > 0) throw new Exception("No se puede Borrar el Menú porque tiene <strong>Roles Asociados</strong>.");

                Dc.Menu.Remove(Menu_);

                var Result = Dc.SaveChanges();

                return Result > 0;
            }
        }
	}
}
