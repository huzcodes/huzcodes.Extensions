using huzcodes.Extensions.Exceptions;
using huzcodes.Extensions.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding the registration of fluent validation from inside huzcodes exception extension plugin.
builder.Services.AddFluentValidation(typeof(Program));

// adding huzcodes identity extension registration
var oSigningKey = builder.Configuration["SigningKey"];
builder.Services.AddAuthZ(oSigningKey!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// adding the exception handler extension registration
app.AddExceptionHandlerExtension();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.AddAuthZMiddleWare();

app.Run();
