using System;
using System.Collections.Generic;
using System.Linq;
using Template.Domain;

namespace Template.Service
{
    public class UsuarioService
    {
        public static Usuario ObtenerUsuario(int ApplicationId, string CorreoElectronico)
        {
            if (string.IsNullOrEmpty(CorreoElectronico)) throw new Exception("Debe especificar el Correo Electrónico del Usuario.");

            using (var Dc = new TemplateEntities())
            {
                var Usuario_ = Dc.Usuario.FirstOrDefault(X => X.Aplicacion_Id == ApplicationId && X.Email == CorreoElectronico);

                return Usuario_;
            }
        }

        public static List<RolUsuario> ObtenerRoles(string PrincipalIdentity)
        {
            if (string.IsNullOrEmpty(PrincipalIdentity)) throw new Exception("Debe especificar la Identidad del Usuario.");

            using (var Dc = new TemplateEntities())
            {
                var Ids = PrincipalIdentity.Split('_');

                if (Ids.Length == 0) throw new Exception("La Identidad del Usuario no contiene información.");

                var Usuario_Id = Convert.ToInt32(Ids[1]);

                var RolesUsuario = Dc.RolUsuario.Include("Rol").Where(X => X.Usuario_Id == Usuario_Id).ToList();

                return RolesUsuario;
            }
        }

        public static List<Menu> ObtenerMenu(string PrincipalIdentity)
        {
            if (string.IsNullOrEmpty(PrincipalIdentity)) throw new Exception("Debe especificar la Identidad del Usuario.");

            using (var Dc = new TemplateEntities())
            {
                var Ids = PrincipalIdentity.Split('_');

                if (Ids.Length == 0) throw new Exception("La Identidad del Usuario no contiene información.");

                var Usuario_Id = Convert.ToInt32(Ids[1]);

                var RolesUsuario = Dc.RolUsuario.Include("Rol").Where(X => X.Usuario_Id == Usuario_Id).ToList();
                var Menu = new List<Menu>();

                if (RolesUsuario.Count <= 0) return Menu;
                var Roles_Id = RolesUsuario.Select(X => X.Rol_Id).ToList();

                Dc.Configuration.LazyLoadingEnabled = true;
                Dc.Database.Log = Console.WriteLine;

                Menu = Dc.Menu.Where(X => Roles_Id.Contains(X.Rol_Id)).OrderBy(X => X.Orden).ToList();

                return Menu;
            }
        }
    }
}
