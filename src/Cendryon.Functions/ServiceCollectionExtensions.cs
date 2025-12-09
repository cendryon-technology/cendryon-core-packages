using Cendryon.Functions.Context;
using Cendryon.Functions.Correlation;
using Microsoft.Extensions.DependencyInjection;

namespace Cendryon.Functions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFunctionsUtilities(
        this IServiceCollection services,
        Action<CorrelationIdOptions>? correlation = null)
    {
        var c = new CorrelationIdOptions(); correlation?.Invoke(c); 

        services.AddSingleton(c); 
        
        services.AddSingleton<IFunctionContextAccessor, FunctionContextAccessor>();
        
        services.AddSingleton<FunctionExceptionMiddleware>();
        services.AddSingleton<CorrelationIdMiddleware>(); 
        services.AddSingleton<FunctionContextAccessorMiddleware>();
 
        services.AddScoped<ICorrelationContext>(_ => new CorrelationContext(null));
        
        return services;
    }
}