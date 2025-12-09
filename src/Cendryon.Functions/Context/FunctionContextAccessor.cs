using Microsoft.Azure.Functions.Worker;

namespace Cendryon.Functions.Context;
 
internal sealed class FunctionContextAccessor : IFunctionContextAccessor
{
    private static readonly AsyncLocal<FunctionContext?> _current = new();
    
    public FunctionContext? Current { get => _current.Value; set => _current.Value = value; }
}