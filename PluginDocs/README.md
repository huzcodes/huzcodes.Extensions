# huzcodes.Extensions 


huzcodes.Extensions is a C# .NET 8 package that provides a global exception handler to enhance error handling in your applications. It offers two main classes, '**ResultException**' and '**CustomResultException**', for managing Extensions and errors in a unified manner. '**ResultException**' allows you to throw Extensions with a specified error message and status code, while '**CustomResultException**' is designed for structured responses in case of failures.

Additionally, the package includes '**fluent validation support**', enabling you to handle validation errors seamlessly. By centralizing error management, huzcodes.Extensions simplifies error handling across different layers of your application.

**huzcodes.Extensions** now includes an **identity extension** for token generation and authorization using **JWT tokens** or **access keys** and it supports authorization for both together as well at the same time. This allows client applications, including mobile or websites, to access APIs using JWT tokens. Additionally, APIs can access other APIs using an access key.

### Installation

To install huzcodes.Extensions, use the following command in the Package Manager Console:

```bash

dotnet add package huzcodes.Extensions --version 1.0.0
```
### Exception and Validation Extension
----------------------------------------

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

### Identity Extension
---------------------------

#### Registering the Identity Extension

To use the identity extension, register the necessary services in your application's program file:

```csharp
// Add huzcodes identity extension registration
var oSigningKey = builder.Configuration["SigningKey"];
builder.Services.AddAuthZ(oSigningKey!);

// Add the huzcodes identity extension middleware
app.AddAuthZMiddleWare();
```
Add the '**[Authorize]**' attribute on top of controllers or specific action methods/endpoints to enable authorization.

### Usage
#### Generating a Token

Inject the '**IIdentityManager**' interface and use the '**GenerateJwtToken**' method to generate a token:
```csharp
var oIdentity = new IdentityModel()
{
    Email = "huz@huzcodes.com",
    FirstName = "huz",
    LastName = "codes"
};
var oSigningKey = _configuration["SigningKey"];
var oToken = _identityManager.GenerateJwtToken(oIdentity,
                                               oSigningKey!,
                                               DateTime.UtcNow.AddHours(1));

```

#### Decoding a Token
By Claims Generic Class, you need to pass same generic class type that used while generating the token and with same prapeters.
```csharp
var oTokenContent = _identityManager.DecodeToken<IdentityModel>();

```
By Passing the Token to Function Parameters,
First, use the '**GetAuthorizationHeader**' extension method "it is part of the package", for '**IHttpContextAccessor**' to get the token from the header:
```csharp
var oJwtToken = _httpContextAccessor.GetAuthorizationHeader();
var oTokenContent = _identityManager.DecodeToken<IdentityModel>(jwtToken: oJwtToken);

```

### Access Key Authorization

To use access key authorization, add the access key in the app settings JSON file as **"X-Api-Key"** and the key name in app settings JSON must by like this name **"X-Api-Key"** for the middleware authorization by api key to work correct.



For more information on how to use huzcodes.Extensions, please refer to the
[API Package Tests](https://github.com/huzcodes/huzcodes.Extensions/tree/main/huzcodes.Extensions.API).

### Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

### License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/huzcodes/huzcodes.Extensions/blob/main/LICENSE) file for details.
