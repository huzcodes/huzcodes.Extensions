# huzcodes.Extensions 
<br>

huzcodes.Extensions is a C# .NET 8 package that provides a global exception handler to enhance error handling in your applications. It offers two main classes, '**ResultException**' and '**CustomResultException**', for managing Extensions and errors in a unified manner. '**ResultException**' allows you to throw Extensions with a specified error message and status code, while '**CustomResultException**' is designed for structured responses in case of failures.

Additionally, the package includes '**fluent validation support**', enabling you to handle validation errors seamlessly. By centralizing error management, huzcodes.Extensions simplifies error handling across different layers of your application.


### Installation

To install huzcodes.Extensions, use the following command in the Package Manager Console:

```bash

dotnet add package huzcodes.Extensions --version 1.0.0
```

### Usage

To use huzcodes.Extensions, register the exception handler extension and, if needed, fluent validation in your application's program file:

```csharp

// Register the exception handler extension
app.AddExceptionHandlerExtension();

// Register fluent validation (if needed)
builder.Services.AddFluentValidation(typeof(Program));
```
### Example

Here's an example of how you can use '**ResultException**':

```csharp

throw new ResultException("response error message", (int)HttpStatusCode.BadRequest);
```

And here's an example of how you can use '**CustomResultException**':

```csharp
throw new CustomResultException(new CustomExceptionResponse()
{
    Message = "response error message",
    StatusCode = (int)HttpStatusCode.BadRequest,
    ClassName = nameof(ExtensionsController),
    FunctionName = nameof(Get),
});
```

For more information on how to use huzcodes.Extensions, please refer to the
[API Package Tests](https://github.com/huzcodes/huzcodes.Extensions/tree/main/huzcodes.Extensions.API).

### Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

### License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/huzcodes/huzcodes.Extensions/blob/main/LICENSE) file for details.
