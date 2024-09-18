using System.ComponentModel;

public enum Enum_Permission
{
    [PermissionCategory(Enum_PermissionCategory.Logging)]
    LoggingList = 1
}

public enum Enum_PermissionCategory
{
    Logging,
}