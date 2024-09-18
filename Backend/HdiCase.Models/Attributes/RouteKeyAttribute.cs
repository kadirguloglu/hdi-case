using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using Humanizer;

public class RouteKeyAttribute : RouteAttribute
{
    public RouteKeyAttribute(Enum_RoutingKeys prefix, [StringSyntax("Route")] string template) : base(prefix.ToString().Underscore() + "/" + template)
    {
    }
}