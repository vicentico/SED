using System;
using System.Collections.Generic;
using System.Linq;
using Template.Domain.Model;

namespace Template.Service.DAO
{
	public static class UserService
    {
        public static Usuario GetByEMail(string CorreoElectronico)
        {
            if (string.IsNullOrEmpty(CorreoElectronico)) throw new Exception("Debe especificar el Correo Electrónico del Usuario.");

            using (var Dc = new TemplateEntities())
            {
                var Usuario_ = Dc
                    .Usuario
                    .FirstOrDefault(X => 
                        X.Email == CorreoElectronico
                        && X.Activo
                    );

                return Usuario_;
            }
        }

        public static List<RolUsuario> GetRoles(string PrincipalIdentity)
        {
            if (string.IsNullOrEmpty(PrincipalIdentity)) throw new Exception("Debe especificar la Identidad del Usuario.");

            using (var Dc = new TemplateEntities())
            {
                var Usuario_Id = Convert.ToInt32(PrincipalIdentity);

                var RolesUsuario = Dc
                    .RolUsuario
                    .Include("Rol")
                    .Where(X => 
                        X.Usuario_Id == Usuario_Id
                        && X.Usuario.Activo
                    )
                    .ToList();

                return RolesUsuario;
            }
        }

        public static List<Menu> GetMenuItems(string PrincipalIdentity)
        {
            if (string.IsNullOrEmpty(PrincipalIdentity)) throw new Exception("Debe especificar la Identidad del Usuario.");

            using (var Dc = new TemplateEntities())
            {
                var Usuario_Id = Convert.ToInt32(PrincipalIdentity);
                var RolesUsuario = GetRoles(PrincipalIdentity);

                if (RolesUsuario == null || RolesUsuario.Count <= 0) return null;
                var Menu = new List<Menu>();

                if (RolesUsuario.Count <= 0) return Menu;

                Menu = Dc
                    .Menu
                    .Where(X => X.RolMenu.Any(Y => Y.Rol.Activo && Y.Rol.RolUsuario.Any(Z => Z.Usuario_Id == Usuario_Id))
                    ).ToList();
                    
                return Menu;
            }
        }

        public static string GetFullName(string PrincipalIdentity)
        {
            if (string.IsNullOrEmpty(PrincipalIdentity)) throw new Exception("Debe especificar la Identidad del Usuario.");

            using (var Dc = new TemplateEntities())
            {
                var Usuario_Id = Convert.ToInt32(PrincipalIdentity);

                var Usuario_ = Dc
                    .Usuario
                    .FirstOrDefault(X => 
                        X.Id == Usuario_Id
                        && X.Activo
                    );

                if (Usuario_ == null) return null;
                var Nombre = string.Format("{0} {1} {2}", Usuario_.Nombre, Usuario_.Apellido_Paterno, Usuario_.Apellido_Materno);
                    
                return Nombre;
            }
        }

        public static bool Edit(int Id, string Nombre, string APaterno, string AMaterno, byte[] ImageData, bool? Activo = null, string Email = null)
        {
            using (var Dc = new TemplateEntities())
            {
                var Usuario_ = Dc
                    .Usuario
                    .FirstOrDefault(X =>
                        X.Id == Id
                        && X.Activo
                    );

                if (Usuario_ == null) throw new Exception("No se pudo <strong>Encontrar</strong> el Usuario para su <strong>Edición</strong>.");

                Usuario_.Nombre = Nombre.Trim();
                Usuario_.Apellido_Paterno = APaterno.Trim();
                Usuario_.Apellido_Materno = AMaterno.Trim();
                if (ImageData != null) Usuario_.Foto = ImageData;
                if (!string.IsNullOrEmpty(Email)) Usuario_.Email = Email;
                if (Activo.HasValue) Usuario_.Activo = Activo.Value;

                Dc.Usuario.Attach(Usuario_);
                var Usuario2_ = Dc.Entry(Usuario_);

                Usuario2_.Property(X => X.Nombre).IsModified = true;
                Usuario2_.Property(X => X.Apellido_Paterno).IsModified = true;
                Usuario2_.Property(X => X.Apellido_Materno).IsModified = true;
                if (ImageData != null) Usuario2_.Property(X => X.Foto).IsModified = true;
                if (!string.IsNullOrEmpty(Email)) Usuario2_.Property(X => X.Email).IsModified = true;
                if (Activo.HasValue) Usuario2_.Property(X => X.Activo).IsModified = true;

                var Result = Dc.SaveChanges();

                return Result > 0;
            }
        }

        public static Usuario New(string Email, string Nombre, string APaterno, string AMaterno, string Password)
        {
            using (var Dc = new TemplateEntities())
            {
                var User_ = Dc.Usuario.FirstOrDefault(X => X.Email.Trim().ToLower() == Email.Trim().ToLower());

                if (User_ != null) throw new Exception(string.Format("No se puede crear el Usuario con Correo Electrónico <strong>{0}</strong> porque ya existe.", Email));

                User_ = new Usuario
                {
                    Email = Email.Trim().ToLower(),
                    Nombre = Nombre.Trim(),
                    Apellido_Paterno = APaterno.Trim(),
                    Apellido_Materno = AMaterno.Trim(),
                    Debe_Cambiar_Password = true,
                    Password = Password,
                    Activo = true
                };

                Dc.Usuario.Add(User_);
                var Result = Dc.SaveChanges();

                return Result > 0 ? User_ : null;
            }
        }

        public static bool ChangePassword(int Id, string NewPassword, bool DebeCambiarPassword = false)
        {
            using (var Dc = new TemplateEntities())
            {
                var Usuario_ = Dc
                    .Usuario
                    .FirstOrDefault(X =>
                        X.Id == Id
                        && X.Activo
                    );

                if (Usuario_ == null) throw new Exception("No se pudo <strong>Encontrar</strong> el Usuario para cambiar su <strong>Contraseña</strong>.");

                Usuario_.Password = NewPassword;
                Usuario_.Debe_Cambiar_Password = DebeCambiarPassword;

                Dc.Usuario.Attach(Usuario_);
                var Usuario2_ = Dc.Entry(Usuario_);

                Usuario2_.Property(X => X.Password).IsModified = true;
                Usuario2_.Property(X => X.Debe_Cambiar_Password).IsModified = true;
                
                var Result = Dc.SaveChanges();

                return Result > 0;
            }
        }

        public static Usuario GetById(string PrincipalIdentity)
        {
            if (string.IsNullOrEmpty(PrincipalIdentity)) throw new Exception("Debe especificar la Identidad del Usuario.");

            using (var Dc = new TemplateEntities())
            {
                var Usuario_Id = Convert.ToInt32(PrincipalIdentity);

                var Usuario_ = Dc
                    .Usuario
                    .FirstOrDefault(X =>
                        X.Id == Usuario_Id
                        && X.Activo
                    );

                return Usuario_;
            }
        }

        public static IEnumerable<Usuario> GetUsers(string Email, string Nombre, string APaterno, string AMaterno, bool? Estado)
        {
            using (var Dc = new TemplateEntities())
            {
                var Usuarios = Dc.Usuario.AsQueryable();

                if (Estado.HasValue) Usuarios = Usuarios.Where(X => X.Activo == Estado.Value);
                if (!string.IsNullOrEmpty(Email)) Usuarios = Usuarios.Where(X => X.Email.Contains(Email));
                if (!string.IsNullOrEmpty(Nombre)) Usuarios = Usuarios.Where(X => X.Nombre.Contains(Nombre));
                if (!string.IsNullOrEmpty(APaterno)) Usuarios = Usuarios.Where(X => X.Apellido_Paterno.Contains(APaterno));
                if (!string.IsNullOrEmpty(AMaterno)) Usuarios = Usuarios.Where(X => X.Apellido_Materno.Contains(AMaterno));
                
                var Usuarios_ = Usuarios.ToList();

                return Usuarios_;
            }
        }

        public static void SetLastAccess(int Id)
        {
            using (var Dc = new TemplateEntities())
            {
                var Usuario_ = Dc
                    .Usuario
                    .FirstOrDefault(X =>
                        X.Id == Id
                        && X.Activo
                    );

                if (Usuario_ == null) throw new Exception("No se pudo <strong>Encontrar</strong> el Usuario para establecer su <strong>Último Acceso</strong>.");

                Usuario_.Ultimo_Acceso = DateTime.Now;

                Dc.Usuario.Attach(Usuario_);
                var Usuario2_ = Dc.Entry(Usuario_);

                Usuario2_.Property(X => X.Ultimo_Acceso).IsModified = true;
                Dc.SaveChanges();
            }
        }

        public static string GetProfilePicture(string PrincipalIdentity)
        {
            if (string.IsNullOrEmpty(PrincipalIdentity)) throw new Exception("Debe especificar la Identidad del Usuario.");

            using (var Dc = new TemplateEntities())
            {
                var Usuario_Id = Convert.ToInt32(PrincipalIdentity);

                var Usuario_ = Dc
                    .Usuario
                    .FirstOrDefault(X => 
                        X.Id == Usuario_Id
                        && X.Activo
                    );

	            string Foto;

                if (Usuario_ == null || Usuario_.Foto == null) Foto = Resources.NoProfilePicture;
                else Foto = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(Usuario_.Foto));

                return Foto;
            }
        }

	    public static Usuario FindById(int Id)
	    {
            using (var Dc = new TemplateEntities())
            {
                var User_ = Dc.Usuario.Find(Id);

                return User_;
            }
	    }

		public static bool AssignRole(int UserId, string Roles)
		{
		    var RolesIds = Roles.Split(',');
            
            using (var Dc = new TemplateEntities())
			{

				var UserRoles = Dc
					.RolUsuario
					.Where(X => 
						X.Usuario_Id == UserId
					).ToList();

			    if (UserRoles.Count > 0)
			    {
			        Dc.RolUsuario.RemoveRange(UserRoles);
			        Dc.SaveChanges();
			    }

                if (!RolesIds.Any()) return true;
                var UserRoles_ = new List<RolUsuario>();

                foreach (var Role_ in RolesIds)
                {
                    if (string.IsNullOrEmpty(Role_)) continue;

                    var UserRole = new RolUsuario
                    {
                        Rol_Id = Convert.ToInt32(Role_),
                        Usuario_Id = UserId
                    };

                    UserRoles_.Add(UserRole);
                }

			    if (UserRoles_.Count > 0)
			    {
			        Dc.RolUsuario.AddRange(UserRoles_);
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
                var User_ = Dc.Usuario.Include("RolUsuario").FirstOrDefault(X => X.Id == Id);

                if (User_ == null) throw new Exception("No se puede Borrar el Usuario porque <strong>no existe</strong>.");
                if (User_.RolUsuario.Count > 0) throw new Exception("No se puede Borrar el Usuario porque tiene <strong>Roles Asociados</strong>.");

                Dc.Usuario.Remove(User_);

                var Result = Dc.SaveChanges();

                return Result > 0;
            }
	    }

        public static List<Usuario> GetAll()
        {
            using (var Dc = new TemplateEntities())
            {
                var Usuarios = Dc.Usuario.AsQueryable();

                Usuarios = Usuarios.Where(x => x.Activo == true);

                var Usuarios_ = Usuarios.ToList();

                return Usuarios_;

            }
        }


    }
}
