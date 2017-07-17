using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Template.Domain.Model;
using Template.MVCApp.Model;
using Template.Service.DAO;

namespace Template.MVCApp.Engine
{
    public static class WebHelper
    {
        public static List<MenuVM> CreateMenuViewModel(int? Padre_Id, List<Menu> Origen)
        {
            var Menu_ = Origen.Where(X => X.Menu_Id == Padre_Id).Select(X => new MenuVM
            {
                Id = X.Id,
                Padre_Id = X.Menu_Id,
                Url = X.Url,
                Texto = X.Texto,
                Icono = X.Icono,
                Ayuda = X.Ayuda,
                SubMenu = CreateMenuViewModel(X.Id, Origen)
            }).ToList();

            return Menu_;
        }

        public static string GetFullName(this IPrincipal IPrincipal_)
        {
            var Nombre = UserService.GetFullName(IPrincipal_.Identity.Name);
            
            return Nombre;
        }

        public static string GetProfilePicture(this IPrincipal IPrincipal_)
        {
            var FotoPerfilBase64 = UserService.GetProfilePicture(IPrincipal_.Identity.Name);

            return FotoPerfilBase64;
        }
    }
}
