public class ParentPermissionAttribute : System.Attribute
{
    public readonly Enum_Permission _category;
    public ParentPermissionAttribute(
        Enum_Permission category
    )
    {
        _category = category;
    }
}