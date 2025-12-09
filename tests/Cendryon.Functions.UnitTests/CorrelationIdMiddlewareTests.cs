using Cendryon.Functions.Correlation;
using FluentAssertions;
using FQ.Functions.Testing;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;

namespace Cendryon.Functions.UnitTests;

public class CorrelationMiddlewareTests
{
    [Fact]
    public async Task Uses_Incoming_Header()
    {
        var ctx = FunctionTestingHelper.Ctx();
        var (req, _) = FunctionTestingHelper.Http(ctx);
        req.Headers.Add("X-Correlation-Id", "CID-abc");

        var mw = new CorrelationIdMiddleware(Options.Create(new CorrelationIdOptions()),
            resolve: _ => Task.FromResult<HttpRequestData?>(req));

        await mw.Invoke(ctx, _ => Task.CompletedTask);

        ctx.Items.TryGetValue(typeof(ICorrelationContext), out var obj).Should().BeTrue();
        (obj as ICorrelationContext)!.CorrelationId.Should().Be("CID-abc");
    }

    [Fact]
    public async Task Generates_When_Missing()
    {
        var ctx = FunctionTestingHelper.Ctx();
        var (req, _) = FunctionTestingHelper.Http(ctx);

        var mw = new CorrelationIdMiddleware(
            Options.Create(new CorrelationIdOptions { GenerateWhenMissing = true }),
            resolve: _ => Task.FromResult<HttpRequestData?>(req));

        await mw.Invoke(ctx, _ => Task.CompletedTask);

        ctx.Items.TryGetValue(typeof(ICorrelationContext), out var obj).Should().BeTrue();
        (obj as ICorrelationContext)!.CorrelationId.Should().NotBeNullOrWhiteSpace();
    }
}