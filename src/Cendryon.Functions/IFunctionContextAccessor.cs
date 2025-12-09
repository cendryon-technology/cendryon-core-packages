using Microsoft.Azure.Functions.Worker;

namespace Cendryon.Functions;

public interface IFunctionContextAccessor
{
    FunctionContext? Current { get; set; }
}