using System.Text;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastructures.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DB")));
services.AddIdentity<AppUser, IdentityRole>(
options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
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
services.AddSwaggerGen();
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
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAnyOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();