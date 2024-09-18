public class PermissionCategoryAttribute : System.Attribute
{
    public readonly Enum_PermissionCategory _category;
    public PermissionCategoryAttribute(
        Enum_PermissionCategory category
    )
    {
        _category = category;
    }
}