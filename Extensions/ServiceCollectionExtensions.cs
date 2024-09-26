using System.Text;
using BDRDExce.AuthenticationHandler;
using BDRDExce.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IdentityBuilder AddCustomIdentity(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Services used by identity
        services.AddAuthentication(IdentityConstants.BearerScheme)
        .AddScheme<JwtBearerOptions, JwtHandler>(IdentityConstants.BearerScheme, null,
        options =>
{
    options.RequireHttpsMetadata = false;
    options.UseSecurityTokenValidators = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {


        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

        services.AddIdentityCore<AppUser>()
            .AddApiEndpoints();

        // Hosting doesn't add IHttpContextAccessor by default
        services.AddHttpContextAccessor();
        // Identity services
        services.TryAddScoped<IUserValidator<AppUser>, UserValidator<AppUser>>();
        services.TryAddScoped<IPasswordValidator<AppUser>, PasswordValidator<AppUser>>();
        services.TryAddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
        services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
        services.TryAddScoped<IRoleValidator<IdentityRole>, RoleValidator<IdentityRole>>();
        // No interface for the error describer so we can add errors without rev'ing the interface
        services.TryAddScoped<IdentityErrorDescriber>();
        services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<AppUser>>();

        services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<AppUser>>();
        services.TryAddScoped<IUserClaimsPrincipalFactory<AppUser>, UserClaimsPrincipalFactory<AppUser, IdentityRole>>();
        services.TryAddScoped<UserManager<AppUser>>();
        services.TryAddScoped<SignInManager<AppUser>>();
        services.TryAddScoped<RoleManager<IdentityRole>>();
        return new IdentityBuilder(typeof(AppUser), typeof(IdentityRole), services);
    }
}