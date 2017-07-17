using System;
using System.Linq;
using System.Web.Security;
using Template.Service.DAO;

namespace Template.Engine.Security
{
    public class BaseRoleProvider : RoleProvider
    {
        public override void AddUsersToRoles(string[] Usernames, string[] RoleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string RoleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string RoleName, bool ThrowOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string RoleName, string UsernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string Username)
        {
            var RolesUsuario = UserService.GetRoles(Username);

            if (RolesUsuario == null || RolesUsuario.Count <= 0) return null;
            var Roles = RolesUsuario.Select(X => X.Rol.Nombre).ToArray();

            return Roles;
        }

        public override string[] GetUsersInRole(string RoleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string Username, string RoleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] Usernames, string[] RoleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string RoleName)
        {
            throw new NotImplementedException();
        }
    }
}
