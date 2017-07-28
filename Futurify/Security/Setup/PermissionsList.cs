using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Security.Models;

namespace Security.Setup
{
    public class PermissionsList
    {
        public const string MANAGER_PERMISSION = "MANAGER";
        public const string STAFF_PERMISSION = "MEMBER"; 
        public const string LEADER_PERMISSION = "LEADER";
        public const string HR_PERMISSION = "HR";

        static PermissionsList()
        {
            GroupsPermissions = new PermissionsGroup[]
            {
                new PermissionsGroup
                    {
                        Name = "Manage Account",
                        Permissions =
                            new Permission[]
                                {
                                    new Permission("ACCOUNT_CREATE", "Account Create"),
                                    new Permission("ACCOUNT_EDIT", "Account Edit"),
                                    new Permission("ACCOUNT_REMOVE", "Account Remove")
                                }
                    },
                 new PermissionsGroup
                    {
                        Name = "Manage Role",
                        Permissions =
                            new Permission[]
                                {
                                    new Permission("ROLE_CREATE", "Role Create"),
                                    new Permission("ROLE_EDIT", "Role Edit"),
                                    new Permission("ROLE_REMOVE", "Role Remove")
                                }
                    },
                  new PermissionsGroup
                    {
                        Name = "Manager",
                        Permissions =
                            new Permission[]
                                {
                                    new Permission(MANAGER_PERMISSION, "Manager")
                                }
                    },
                   new PermissionsGroup
                    {
                        Name = "Leader",
                        Permissions =
                            new Permission[]
                                {
                                    new Permission(LEADER_PERMISSION, "Leader")
                                }
                    },
                    new PermissionsGroup
                    {
                        Name = "Hr",
                        Permissions =
                            new Permission[]
                                {
                                    new Permission(HR_PERMISSION, "Human Resources")
                                }
                    },
                    new PermissionsGroup
                    {
                        Name = "Member",
                        Permissions =
                            new Permission[]
                                {
                                    new Permission(STAFF_PERMISSION, "Member")
                                }
                    }
            };
        }

        public static PermissionsGroup[] GroupsPermissions { get; }
    }
}
