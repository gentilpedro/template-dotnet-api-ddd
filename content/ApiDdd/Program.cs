using System.Text;
using DddApiTemplate.Application.Commands.CreateOrder;
using DddApiTemplate.Application.Common;
using DddApiTemplate.Application.DTOs;
using DddApiTemplate.Application.Interfaces;
using DddApiTemplate.Application.Mappings;
using DddApiTemplate.Application.Queries.GetOrderById;
using DddApiTemplate.Domain.Repositories;
using DddApiTemplate.Infrastructure.Auth;
using DddApiTemplate.Infrastructure.Data;
using DddApiTemplate.Infrastructure.Repositories;
using DddApiTemplate.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando DddApiTemplate");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

    var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
    var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key não configurada.");

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });
    builder.Services.AddAuthorization();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddAutoMapper(cfg => cfg.AddProfile<OrderMappingProfile>());

    builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();

    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
    builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

    builder.Services.AddScoped<ICommandHandler<CreateOrderCommand, OrderDto>, CreateOrderCommandHandler>();
    builder.Services.AddScoped<IQueryHandler<GetOrderByIdQuery, OrderDto?>, GetOrderByIdQueryHandler>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "DddApiTemplate encerrado de forma inesperada");
}
finally
{
    Log.CloseAndFlush();
}
