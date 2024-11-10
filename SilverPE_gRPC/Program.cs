using SilverPE_gRPC.Services;
using SilverPE_Repository;
using SilverPE_Repository.Interfaces;

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

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var secretKey = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var durationInMinutes = int.Parse(jwtSettings["DurationInMinutes"] ?? "60");

            builder.Services.AddScoped<ITokenRepository>(provider =>
                new TokenRepository(secretKey ?? "56075EA56027AA2172CA5003FF7F3305E3E665B9ABEDD9AC42577D25D79BE594", issuer ?? "JewerlyStore", audience ?? "JewelryStoreUser", durationInMinutes));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<GreeterService>();
            app.MapGrpcService<AccountService>();
            app.MapGrpcReflectionService();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}