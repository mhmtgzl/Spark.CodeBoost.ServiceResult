# Service Result Pattern

A standardized result pattern for .NET API responses with rich error handling capabilities.

## Features

- ✅ Generic and non-generic result types
- ✅ Built-in support for common HTTP status codes
- ✅ Exception-to-result conversion
- ✅ Consistent error message formatting
- ✅ Success/Failure factory methods
- ✅ Custom error code support (for `CoreException`)

## Installation

1. Ensure you have the required namespace reference:
```csharp
using Spark.CodeBoost.ServiceResult;

```
## Basic Usage
### Successful Results
```csharp
// Non-generic success
var result = Result.Success("Operation completed");

// Generic success with data
var dataResult = Result.Success(new Product { Id = 1, Name = "Laptop" });

```
## Error Results
```csharp
// Basic error
var error = Result.Failure("Validation failed");

// With status code
var notFound = Result.NotFound("User not found");

// From exception
try {
    // ...
} 
catch (AppException ex) {
    return Result.Failure(ex);
}

```
## Common Status Codes
```bash
Method	HTTP Status	Typical Use Case
Success()	200 OK	Successful operations
Failure()	400 BadReq	Validation errors
NotFound()	404	Missing resources
Unauthorized()	401	Authentication failures
Forbidden()	403	Authorization failures
InternalError()	500	Server errors

```
## Advanced Usage
### Custom Error Codes
### When using CoreException:

```csharp
// Exception automatically converts to code "MODULE-1001"
throw new CoreException(new Module("MODULE", "Module Name"), 1001, "Error message");

```
### Message Formatting
```csharp
// With parameters
var error = Result.Failure("Invalid {0} value", "email");

// Output: "Invalid email value"

```
### Generic Results
```csharp
// Successful data response
return Result.Success(product);

// Error data response
return Result.Failure<Product>("Product not found");

// Typed status responses
return Result.NotFound<Product>("Product ID 123 not found");

```
## Best Practices
### 1.Consistent Error Handling

```csharp
public Result<Order> ProcessOrder(OrderRequest request)
{
    if (!IsValid(request))
        return Result.Failure<Order>("Invalid request");
    
    // ... processing
    
    return Result.Success(order);
}

```
### Controller Integration

```csharp
[HttpGet("{id}")]
public IActionResult GetProduct(int id)
{
    var result = _service.GetProduct(id);
    
    if (!result.Success)
        return StatusCode((int)result.StatusCode, result.Message);
    
    return Ok(result.Data);
}

```
### Exception Mapping

```csharp
// Global exception handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception is AppException appEx)
        {
            var result = Result.Failure(appEx);
            context.Response.StatusCode = (int)result.StatusCode;
            await context.Response.WriteAsync(result.Message);
        }
    });
});

```
## FAQ
### Q: How to add custom status codes?
A: Use the base constructor:

```csharp
return new Result(false, "Conflict", HttpStatusCode.Conflict);
Q: How to extend with additional properties?
A: Inherit from Result<T>:

```
```csharp
public class ExtendedResult<T> : Result<T>
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

```
## Contribution
```bash
1- Fork the repository
2- Create your feature branch (git checkout -b feature/improvement)
3- Commit your changes (git commit -m 'Add some feature')
4- Push to the branch (git push origin feature/improvement)
5- Open a Pull Request
