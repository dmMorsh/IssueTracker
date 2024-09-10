using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using IssueTracker.Infrastructure.Context;
using IssueTracker.Infrastructure.Repositories;
using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;
using IssueTracker.Application.Services;
using IssueTracker.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

//EF & Identity
string? connString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connString))
    .AddIdentity<ApplicationUser, ApplicationRole>(config =>
    {
        config.Password.RequireDigit = false;
        config.Password.RequireLowercase = false;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequireUppercase = false;
        config.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

//Auth
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AuthOptions:ISSUER"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AuthOptions:AUDIENCE"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthOptions:KEY"]!)),
            ValidateIssuerSigningKey = true,
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var path = context.HttpContext.Request.Path;
                if (path.StartsWithSegments("/api/chathub"))
                {
                    var accessToken = context.Request.Query["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                        context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

//CORS
string? frontUrl = builder.Configuration["Frontend:URL"];
if (!string.IsNullOrEmpty(frontUrl))
    frontUrl = "http://localhost:4200";

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        builder => builder
            // .SetIsOriginAllowed(_ => true) // Разрешить все источники
            .WithOrigins(frontUrl!, "http://host.docker.internal:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

//to add id of user where requared
builder.Services.AddScoped<UserGuidFilter>();

//to add services from .Infrastructure
var repositories = typeof(ChatRepository).Assembly.GetTypes()
    .Where(r => r.Name.EndsWith("Repository") && r.IsInterface == false)
    .ToList();
foreach (var repository in repositories)
{
    var interfaceType = repository.GetInterfaces().FirstOrDefault(i => i.Name == $"I{repository.Name}");
    if (interfaceType != null)
    {
        builder.Services.AddScoped(interfaceType, repository);
    }
}

// //to add services from .Application
var appServices = typeof(UserService).Assembly.GetTypes()
    .Where(s => s.Name.EndsWith("Service") && s.IsInterface == false).ToList();
foreach (var appService in appServices)
    builder.Services.Add(new ServiceDescriptor(appService, appService, ServiceLifetime.Scoped));

builder.Services.AddScoped<IChatRepository, ChatRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("api/chathub");

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var roles = new[] { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            ApplicationRole newrole = new ApplicationRole { Name = role };
            await roleManager.CreateAsync(newrole);
        };
    }
}

app.Run();
