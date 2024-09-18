public class GetCurrentUserResponse
{
    public required AdminLoginData AdminLoginData { get; set; }
    public required List<Role> Roles { get; set; }
}