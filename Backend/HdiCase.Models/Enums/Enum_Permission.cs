using System.ComponentModel;

public enum Enum_Permission
{
    [PermissionCategory(Enum_PermissionCategory.Role)]
    RoleList = 1000,
    [ParentPermission(RoleList)]
    RoleInsert,
    [ParentPermission(RoleList)]
    RoleUpdate,
    [ParentPermission(RoleList)]
    RoleDelete,

    [PermissionCategory(Enum_PermissionCategory.AdminLoginData)]
    AdminLoginDataList = 2000,
    [ParentPermission(AdminLoginDataList)]
    AdminLoginDataInsert,
    [ParentPermission(AdminLoginDataList)]
    AdminLoginDataUpdate,
    [ParentPermission(AdminLoginDataList)]
    AdminLoginDataDelete,

    [PermissionCategory(Enum_PermissionCategory.Logging)]
    LoggingList = 17000
}

public enum Enum_PermissionCategory
{
    Logging,
    AdminLoginData,
    Role
}