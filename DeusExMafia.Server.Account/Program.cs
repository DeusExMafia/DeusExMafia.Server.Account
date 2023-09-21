using DeusExMafia.Server.Account.Context;
using DeusExMafia.Server.Account.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace DeusExMafia.Server.Account;

public class Program {
    public const string CLIENT_POLICY_NAME = "ClientPolicy";
    public const string GAME_POLICY_NAME = "GamePolicy";

    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        CorsPolicyAddressSet addresses = builder.Configuration.GetSection("CorsPolicyAddresses").Get<CorsPolicyAddressSet>()!;
        builder.Services.AddCors(options => {
            options.AddPolicy(CLIENT_POLICY_NAME, policy => policy.WithOrigins(addresses.Client)
                .WithMethods("post")
                .WithHeaders(HeaderNames.AccessControlAllowOrigin)
                .WithHeaders(HeaderNames.AccessControlAllowHeaders)
            );
            options.AddPolicy(GAME_POLICY_NAME, policy => policy.WithOrigins(addresses.Game));
        });
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDbContext<DeusExMafiaContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DeusExMafia")));
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IAccountService, AccountService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(CLIENT_POLICY_NAME);
        app.UseCors(GAME_POLICY_NAME);

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
