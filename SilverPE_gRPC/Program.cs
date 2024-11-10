using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SilverPE_gRPC.Services;
using SilverPE_Repository;
using SilverPE_Repository.Interfaces;
using System.Text;

namespace SilverPE_gRPC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddGrpcReflection();

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IJewelryRepository, JewelryRepository>();

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var secretKey = jwtSettings["Key"] ?? "56075EA56027AA2172CA5003FF7F3305E3E665B9ABEDD9AC42577D25D79BE594";
            var issuer = jwtSettings["Issuer"] ?? "JewerlyStore";
            var audience = jwtSettings["Audience"] ?? "JewelryStoreUser";
            var durationInMinutes = int.Parse(jwtSettings["DurationInMinutes"] ?? "60");

            builder.Services.AddScoped<ITokenRepository>(provider =>
                new TokenRepository(secretKey, issuer , audience, durationInMinutes));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
            builder.Services.AddAuthorization();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<GreeterService>();
            app.MapGrpcService<AccountService>();
            app.MapGrpcService<SilverJewelryService>(); 
            app.MapGrpcReflectionService();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}