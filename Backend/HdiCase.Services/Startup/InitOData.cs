using Humanizer;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

public static class InitOData
{
    static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EntitySet<AdminLoginData>("AdminLoginData");
        builder.EntitySet<Logging>("Logging");
        builder.EntitySet<Role>("Role");
        builder.EnableLowerCamelCase();
        return builder.GetEdmModel();
    }
    public static void Init(IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddOData(opt =>
        {
            opt.AddRouteComponents(Enum_RoutingKeys.Api.ToString().Underscore() + "/odata/v1", GetEdmModel()).Filter().Select().Expand().Count().OrderBy().SetMaxTop(100);
        });
    }
}