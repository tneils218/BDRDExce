using BDRDExce.Extensions;
using BDRDExce.Infrastuctures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastructures.Services;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DB")));
services.AddCustomIdentity(builder.Configuration)
.AddEntityFrameworkStores<AppDbContext>();
services.TryAddScoped<IRoleValidator<IdentityRole>, RoleValidator<IdentityRole>>();
services.TryAddScoped<RoleManager<IdentityRole>>();
services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(
    c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] { }
    }
    });
    }
);
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IRoleService, RoleService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<ICourseService, CourseService>();
services.AddScoped<ISubmissionService, SubmissionService>();
services.AddScoped<IMediaService, MediaService>();
services.AddScoped<IEmailSender, EmailSender>();
services.AddScoped<IExamService, ExamService>();

var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
});
// }

app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAnyOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();