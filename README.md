# Cendryon Core Packages

A modern, minimal, and production-ready foundation for building **.NET 10** services and APIs —  
designed around **Clean Architecture**, **CQRS**, and **unified result handling**.

This ecosystem includes:

| Package | Purpose |
|----------|----------|
| **Cendryon.Results** | Uniform success/error result handling, HTTP mapping, and problem-details integration |
| **Cendryon.AspNetCore** | ASP.NET Core utilities (correlation, idempotency, versioning, result mapping) |
| **Cendryon.Functions** | Azure Functions utilities (correlation, idempotency, exception handling, result writers) |
| **Cendryon.Mapping** | Tiny abstraction over object mapping with first‑class `Result<T>` support |


All libraries target **.NET 10** and work with: 
- **Microsoft.Azure.Functions.Worker 2.2+**
- **ASP.NET Core 10.0+**


## Support the development
If you like what you see here, feel free to donate to contribute to our open-source efforts. All donations will be received by the contributing engineers!

<a href="https://www.buymeacoffee.com/futeq" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-green.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>

---

## 📦 Installation

```bash
dotnet add package Cendryon.Results
dotnet add package Cendryon.Cqrs
dotnet add package Cendryon.AspNetCore
dotnet add package Cendryon.Functions
dotnet add package Cendryon.Mapping
```

---

## 🧩 Cendryon.Results

Provides lightweight primitives for handling outcomes and errors across your entire stack.

### Result basics

```csharp
using Cendryon.Results;

var ok = Result.Ok();
var user = Result<User>.Ok(new User("alice"));
var notFound = Result.Fail(Error.NotFound("user_not_found", "User does not exist"));
```

### Error factory helpers

```csharp
var e1 = Error.Validation("email", "Invalid format");
var e2 = Error.Forbidden("no_access", "User has no permission");
var e3 = Error.Conflict("duplicate", "Email already exists");
```

### Mapping to HTTP Problem Details

```csharp
var shape = e1.ToProblemShape("/api/users/1");

Console.WriteLine(shape.Status);  // 400
Console.WriteLine(shape.Type);    // urn:problem-type:validation
```

### JSON-friendly model

All result and error types are fully serializable with `System.Text.Json`.

```csharp
var json = JsonSerializer.Serialize(Result.Fail(Error.NotFound("x", "missing")));
```

Due to immutability of the record classes, use `Cendryon.Results.JsonResultSerializer` for deserialization purposes.

```csharp
var result = JsonResultSerializer.Deserialize<ResultType>(json, _serializerOptions);
```


---

## ☁️ Cendryon.Functions

Azure Functions isolated worker helpers.

### Configure

```csharp
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.UseFunctionsUtilities();
    })
    .ConfigureServices(services =>
    {
        services.AddFunctionsUtilities();
    })
    .Build();

host.Run();
```

### Usage

```csharp
public class UsersFunctions
{
    [Function("GetUser")]
    public async Task<HttpResponseData> GetUser(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{id}")] HttpRequestData req, string id)
    {
        if (id == "404")
        {
                return await req.WriteResultAsync(Result.Fail(Error.NotFound("user", "Not found")));
        }

        return await req.WriteResultAsync(Result.Ok());
    }
}
```

---

## ✅ Testing

All packages have tests (xUnit + FluentAssertions + NSubstitute).

```bash
dotnet test -c Release
```

---

## 🧩 License

MIT © 2025 Cendryon
