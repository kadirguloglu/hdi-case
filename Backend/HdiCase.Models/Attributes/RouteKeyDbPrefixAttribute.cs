using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Humanizer;

public class RouteKeyDbPrefixAttribute : RouteAttribute
{
    public RouteKeyDbPrefixAttribute(Enum_RoutingKeys prefix, string extraKey, [StringSyntax("Route")] string template) : base(prefix.ToString().Underscore() + extraKey + "/" + template)
    {
    }
}