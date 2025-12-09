using Cendryon.Functions.Correlation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;

namespace Cendryon.Functions;

public static class FunctionWorkerApplicationBuilderExtensions
{
    public static IFunctionsWorkerApplicationBuilder UseFunctionsUtilities(
        this IFunctionsWorkerApplicationBuilder builder)
    {
        builder.UseMiddleware<CorrelationIdMiddleware>(); 
        builder.UseMiddleware<FunctionExceptionMiddleware>();

        return builder;
    }
}