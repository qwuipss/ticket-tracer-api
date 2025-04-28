using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace TicketTracer.Api.Configuration;

internal static class ControllersConfigurator
{
    public static void AddControllers(this IServiceCollection services)
    {
        services
            .AddControllers(
                options => { options.Conventions.Add(new LowercaseControllerModelConvention()); }
            )
            .ConfigureApplicationPartManager(manager => { manager.FeatureProviders.Add(new ControllerFeatureProvider()); });
    }

    private class ControllerFeatureProvider : Microsoft.AspNetCore.Mvc.Controllers.ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            return typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) && typeInfo.IsDefined(typeof(ApiControllerAttribute));
        }
    }

    private class LowercaseControllerModelConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.ControllerName = controller.ControllerName.ToLowerInvariant();

            foreach (var action in controller.Actions)
            {
                action.ActionName = action.ActionName.ToLowerInvariant();
            }
        }
    }
}