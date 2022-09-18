using SchoolApp.Classroom.Ioc.Database;
using SchoolApp.Classroom.Ioc.Repositories;
using SchoolApp.Classroom.Ioc.Services;
using SchoolApp.Classroom.Ioc.Settings;
using SchoolApp.Shared.Utils.HttpApi.Extensions;
using SchoolApp.Shared.Utils.HttpApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().HandleDataAnnotationExceptions();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddClassroomDatabase();
builder.Services.AddClassroomRepositories();
builder.Services.AddClassroomServices();
builder.Services.AddClassroomSettings(builder.Configuration);


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
