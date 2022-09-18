using SchoolApp.IdentityProvider.Ioc.Database;
using SchoolApp.IdentityProvider.Ioc.Repositories;
using SchoolApp.IdentityProvider.Ioc.Services;
using SchoolApp.Shared.Utils.Authentication;
using SchoolApp.Shared.Utils.HttpApi.Extensions;
using SchoolApp.Shared.Utils.HttpApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().HandleDataAnnotationExceptions();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityProviderDatabase();
builder.Services.AddIdentityProviderRepositories();
builder.Services.AddIdentityProviderServices();
builder.Services.Configure<CustomAuthenticationSettings>(builder.Configuration.GetSection(nameof(CustomAuthenticationSettings)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<CustomAuthMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
