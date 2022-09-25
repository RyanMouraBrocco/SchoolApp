using SchoolApp.Activity.Ioc.Repositories;
using SchoolApp.Activity.Ioc.Services;
using SchoolApp.Activity.Ioc.Settings;
using SchoolApp.Shared.Utils.Authentication;
using SchoolApp.Shared.Utils.HttpApi.Extensions;
using SchoolApp.Shared.Utils.HttpApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().HandleDataAnnotationExceptions();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddActivityRepositories();
builder.Services.AddActivityServices();
builder.Services.AddActivitySettings(builder.Configuration);
builder.Services.Configure<CustomAuthenticationSettings>(builder.Configuration.GetSection(nameof(CustomAuthenticationSettings)));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<CustomAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
