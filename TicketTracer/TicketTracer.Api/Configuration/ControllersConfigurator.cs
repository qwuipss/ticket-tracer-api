using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace TicketTracer.Api.Configuration;

internal static class ControllersConfigurator
{
    public static void AddControllers(this IServiceCollection services)
    {
        MvcServiceCollectionExtensions
            .AddControllers(services)
            .ConfigureApplicationPartManager(manager => { manager.FeatureProviders.Add(new ControllerFeatureProvider()); });
    }

    private class ControllerFeatureProvider : Microsoft.AspNetCore.Mvc.Controllers.ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            return typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) && typeInfo.IsDefined(typeof(ApiControllerAttribute));
        }
    }
}