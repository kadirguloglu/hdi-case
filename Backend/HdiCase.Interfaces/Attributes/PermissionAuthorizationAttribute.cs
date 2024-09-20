using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class PermissionAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    private readonly Enum_Permission _permission;
    public PermissionAuthorizationAttribute(
        Enum_Permission permission
    )
    {
        _permission = permission;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            if (context.HttpContext?.RequestServices is null)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }
            else
            {

                IClaimService? _claimService = context.HttpContext.RequestServices.GetService(typeof(IClaimService)) as IClaimService;
                if (_claimService == null)
                {
                    context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                }
                else
                {
                    var currentUserId = _claimService.GetUserId();
                    if (currentUserId == null)
                    {
                        context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                    }
                    else
                    {
                        IDatabaseContext<AdminLoginData>? _loginDataContext = context.HttpContext.RequestServices.GetService(typeof(IDatabaseContext<AdminLoginData>)) as IDatabaseContext<AdminLoginData>;
                        if (_loginDataContext == null)
                        {
                            context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                        }
                        else
                        {
                            var currentUser = _loginDataContext.FirstAsync(x => x.Id == currentUserId).Result;
                            if (currentUser == null)
                            {
                                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                            }
                            else
                            {
                                if (!currentUser.IsDeveloper)
                                {
                                    IDatabaseContext<Role>? _roleContext = context.HttpContext.RequestServices.GetService(typeof(IDatabaseContext<Role>)) as IDatabaseContext<Role>;
                                    if (_roleContext == null)
                                    {
                                        context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);

                                    }
                                    else
                                    {
                                        if (currentUser.RoleId == null)
                                        {
                                            context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                                        }
                                        else
                                        {
                                            var roles = _roleContext.GetAsync(x => currentUser.RoleId.Contains(x.Id)).Result;
                                            if (roles == null)
                                            {
                                                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                                            }
                                            else
                                            {
                                                int enumValue = (int)_permission;
                                                var isExist = roles.Any(x => x.PermissionKeys != null && x.PermissionKeys.Contains(enumValue));
                                                if (!isExist)
                                                {
                                                    context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
        }
    }
}