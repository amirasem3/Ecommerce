using System.Text;
using System.Text.Json.Serialization;
using Ecommerce.Application.Binder;
using Ecommerce.Application.Binder.Category;
using Ecommerce.Application.Binder.Invoice;
using Ecommerce.Application.Binder.Product;
using Ecommerce.Application.Binder.User;
using Ecommerce.Application.Services;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Interfaces.RelationRepoInterfaces;
using Ecommerce.Core.Log;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .MinimumLevel.Information() // Set global minimum log level
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) // Suppress framework logs
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)    
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level}] [Machine: {MachineName}] [ClassName: {ClassName}] [FunctionName: {FunctionName}] [Arguments: {Arguments}] [RetrieveData: {RetrieveData}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("Logs/log.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level}] [Machine: {MachineName}] [ClassName: {ClassName}] [FunctionName: {FunctionName}] [Arguments: {Arguments}] [RetrieveData: {RetrieveData}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();



try
{
    // Log.Information("Starting up the application");
    LoggerHelper.LogWithDetails("Starting up the application",logLevel:LoggerHelper.LogLevel.Information);
    builder.Services.AddControllersWithViews();
    builder.Services.AddMvcCore();
    builder.Services.AddMvc();
    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = "AmirHosseinIssuer",
                ValidAudience = "AmirHosseinAudience",
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => { });
    builder.Services.AddDbContext<EcommerceDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("EcommerceDB_Post"),
            npgsqlOptions => npgsqlOptions.UseNetTopologySuite())
    );
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp",
            builder =>
            {
                builder.WithOrigins("http://localhost:3000", "https://ahasemyosuefi.ir", "https://ahasemyousefi.ir")
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
            });
    });
// builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "V1",
            Title = "Ecommerce API",
            Description = "ASP.NET Core Web API"
        });
    });


    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddScoped(typeof(GenericRepository<>));
    builder.Services.AddScoped<LoggerHelper>();
    builder.Services.AddScoped<ProductService>();
    builder.Services.AddScoped<RoleService>();
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    builder.Services.AddScoped<ManufacturerService>();
    builder.Services.AddScoped<CategoryServices>();
    builder.Services.AddScoped<InvoiceServices>();
    builder.Services.AddScoped<RoleService>();
    builder.Services.AddScoped<IModelBinder, RoleModelBinder>();
    builder.Services.AddScoped<RoleModelBinderProvider>();

    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<IModelBinder, UserModelBinder>();
    builder.Services.AddScoped<UserModelBinderProvider>();


    builder.Services.AddScoped<ProductService>();
    builder.Services.AddScoped<IModelBinder, ProductModelBinder>();
    builder.Services.AddScoped<ProductModelBinderProvider>();

    builder.Services.AddScoped<ManufacturerService>();
    builder.Services.AddScoped<IModelBinder, ManufacturerModelBinder>();
    builder.Services.AddScoped<ManufacturerModelBinderProvider>();


    builder.Services.AddScoped<InvoiceServices>();
    builder.Services.AddScoped<IModelBinder, InvoiceModelBinder>();
    builder.Services.AddScoped<InvoiceModelBinderProvider>();

    builder.Services.AddScoped<CategoryServices>();
    builder.Services.AddScoped<IModelBinder, CategoryModelBinder>();
    builder.Services.AddScoped<CategoryModelBinderProvider>();

    builder.Services.AddControllers(options =>
    {
        options.ModelBinderProviders.Insert(0, new RoleModelBinderProvider(
            builder.Services.BuildServiceProvider().GetRequiredService<RoleService>()));

        options.ModelBinderProviders.Insert(1, new UserModelBinderProvider(
            builder.Services.BuildServiceProvider().GetRequiredService<UserService>()));

        options.ModelBinderProviders.Insert(2, new ProductModelBinderProvider(
            builder.Services.BuildServiceProvider().GetRequiredService<ProductService>()));

        options.ModelBinderProviders.Insert(3, new ManufacturerModelBinderProvider(
            builder.Services.BuildServiceProvider().GetRequiredService<ManufacturerService>()));

        options.ModelBinderProviders.Insert(4, new InvoiceModelBinderProvider(
            builder.Services.BuildServiceProvider().GetRequiredService<InvoiceServices>()));

        options.ModelBinderProviders.Insert(5, new CategoryModelBinderProvider(
            builder.Services.BuildServiceProvider().GetRequiredService<CategoryServices>()));

    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    }).AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });
    builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
    var app = builder.Build();

// app.UseMiddleware<CustomUnauthorizedResponseMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/V1/swagger.json", "Ecommerce API V1");
        c.RoutePrefix = string.Empty;
    });
// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run((async context =>
        {

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature?.Error is not null)
            {
                Log.Error(exceptionHandlerPathFeature.Error, "An Unhandled exception occured! {Path}",
                    exceptionHandlerPathFeature.Path);
            }

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An Error occured!");
            // var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            // Log.Error(exception, "An Unhandled exception occured.");
        }));
    });

    app.UseHttpsRedirection();
// app.UseDeveloperExceptionPage();
    app.UseRouting();
    app.UseCors("AllowReactApp");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();


    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
