using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SchoolApp.File.Application.Interfaces.Repositories;
using SchoolApp.File.Application.Interfaces.Services;
using SchoolApp.File.Application.Services;
using SchoolApp.File.Blob.Repositories;
using SchoolApp.File.Blob.Settings;
using SchoolApp.File.Http.Repositories;
using SchoolApp.File.Http.Settings;
using SchoolApp.Shared.Utils.HttpApi.Extensions;
using SchoolApp.Shared.Utils.HttpApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().HandleDataAnnotationExceptions();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUserFileService, UserFileService>();
builder.Services.AddScoped<IStudentFileService, StudentFileService>();
builder.Services.AddScoped<IActivityFileService, ActivityFileService>();
builder.Services.AddScoped<IActivityAnswerVersionFileService, ActivityAnswerVersionFileService>();

builder.Services.AddScoped(typeof(IFileRepository<>), typeof(FileRepository<>));
builder.Services.AddHttpClient<IStudentRepository, StudentRepository>();
builder.Services.AddHttpClient<IClassroomRepository, ClassroomRepository>();
builder.Services.AddHttpClient<IActivityAnswerVersionRepository, ActivityAnswerVersionRepository>();
builder.Services.AddHttpClient<IActivityRepository, ActivityRepository>();

builder.Services.Configure<AzureBlobStorageSettings>(builder.Configuration.GetSection(nameof(AzureBlobStorageSettings)));
builder.Services.Configure<ActivityServiceApiSettings>(builder.Configuration.GetSection(nameof(ActivityServiceApiSettings)));
builder.Services.Configure<ClassroomServiceApiSettings>(builder.Configuration.GetSection(nameof(ClassroomServiceApiSettings)));



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AuthenticationSettings:Issuer"],
            ValidAudience = builder.Configuration["AuthenticationSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthenticationSettings:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
