using FCG.Application;
using FCG.Infrastructure;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Data.Migrations;
using FCG.Infrastructure.Logging;
using FCG.API.Middlewares;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Datadog.Trace;
using Datadog.Trace.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add Logging
builder.Logging.ClearProviders();
builder.Services.AddCustomLogging(builder.Configuration);

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter());
});

// Configs Datadog APM
var settings = TracerSettings.FromDefaultSources();
Tracer.Configure(settings);

// Add services to the container.
builder.Services.AddControllers();

// Add Infrastructure Services
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Fiap Cloud Games API",
        Version = "v1",
        Description = "API do projeto [Fiap Cloud Games (FCG)](https://github.com/hagadashi/FCG) da FIAP.",
        Contact = new OpenApiContact
        {
            Name = "Equipe FCG",
            Url = new Uri("https://github.com/hagadashi/FCG"),
        },
    });
    // Define o esquema de segurança (ex: JWT Bearer)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Aplica o esquema para todos os endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});

var app = builder.Build();

app.UseGlobalExceptionHandling();
app.UseRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Aplica migrations no ambiente de desenvolvimento
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();

        // Aplica as migrações automaticamente
        context.Database.Migrate();

        // Insere dados iniciais
        await DbInitializer.SeedDataAsync(context);
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllers();

try
{
    Log.Information("Iniciando FCG API...");

    // Log das URLs que a aplicação está ouvindo
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        var addresses = app.Services.GetRequiredService<IServer>().Features
            .Get<IServerAddressesFeature>()?.Addresses;

        if (addresses != null && addresses.Any())
        {
            logger.LogInformation("FCG API iniciada em {Environment} - Ouvindo em: {Addresses}",
                app.Environment.EnvironmentName, string.Join(", ", addresses));
            logger.LogInformation("Swagger: {Addresses}", string.Join(", ", addresses.Select(x => string.Concat(x, "/swagger"))));
        }
        else
        {
            logger.LogInformation("FCG API iniciada em {Environment}", app.Environment.EnvironmentName);
        }
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou ao inicializar");
}
finally
{
    Log.CloseAndFlush();
}