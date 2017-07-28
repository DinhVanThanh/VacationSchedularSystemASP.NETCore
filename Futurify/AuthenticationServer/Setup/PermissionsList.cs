using AuthenticationServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Setup
{
    public static class PermissionsList
    {

        public const string RECRUITER_PERMISSION = "RECRUITER";
        public const string JOBSEEKER_PERMISSION = "JOBSEEKER";

        static PermissionsList()
        {
            GroupsPermissions = new PermissionsGroup[]
            {
                new PermissionsGroup
                    {
                        Name = "Manage accounts",
                        Permissions =
                            new Permission[]
                                {
                                    new Permission("VIEW_ACCOUNTS", "View accounts"),
                                    new Permission("CREATE_ACCOUNT", "Create account"),
                                    new Permission("EDIT_ACCOUNT", "Edit account"),
                                    new Permission("ADMIN_EDIT_ACCOUNT", "Admin can edit account"),
                                    new Permission("ADMIN_REMOVE_AVATAR", "Admin can delete avatar"),
                                    new Permission("ADMIN_REMOVE_VIDEO", "Admin can delete video"),
                                    new Permission("DELETE_ACCOUNT", "Delete account")
                                }
                    },
                new PermissionsGroup
                    {
                        Name = "Manage_Roles",
                        Permissions =
                            new Permission[]
                                {
                                    new Permission("VIEW_ROLES", "View roles"),
                                    new Permission("CREATE_ROLE", "Create role"),
                                    new Permission("EDIT_ROLE", "Edit role"),
                                    new Permission("DELETE_ROLE", "Delete role")
                                }
                    },
                new PermissionsGroup
                {
                    Name = "Recruiter",
                    Permissions =
                        new Permission[]
                        {
                            new Permission(RECRUITER_PERMISSION, "Recruiter")
                        }
                },
                new PermissionsGroup
                {
                    Name = "Jobseeker",
                    Permissions =
                        new Permission[]
                        {
                            new Permission(JOBSEEKER_PERMISSION, "Jobseeker")
                        }
                },
                new PermissionsGroup
                {
                    Name= "Manage data",
                    Permissions =
                    new Permission[]
                    {
                        new Permission("UPDATE_DATA", "Update data")
                    }
                }
            };
        }

        public static PermissionsGroup[] GroupsPermissions { get; }

    }
}
