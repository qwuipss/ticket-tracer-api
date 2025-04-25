using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TicketTracer.Api.Models.Binders;

internal class LowerCaseModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;
        var result = bindingContext.ValueProvider.GetValue(modelName);
        var value = string.Join(string.Empty, result.Values.Select(s => s?.ToLowerInvariant()));
        
        bindingContext.Result = ModelBindingResult.Success(value.ToLowerInvariant());
        
        return Task.CompletedTask;
    }
}