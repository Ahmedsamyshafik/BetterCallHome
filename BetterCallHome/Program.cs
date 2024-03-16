using Core;
using Domin.Models;
using Domin.Seeding;
using Infrastructure.Data;
using infrustructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Context;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Swagger Authentication
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo", Version = "v1" });
});
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation    
    swagger.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 5 Web API",
        Description = " ITI Projrcy"
    });

    // To Enable authorization using Swagger (JWT)    
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Go on",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
            new string[] {}
        }
                });
});
#endregion




//---- My Services----------


//builder.Services.AddMvc();

//----Context + Identity + Authentication + Email
#region Context + Identity + Authentication + Email

builder.Services.AddDbContext<AppDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("defualtConnection"))
    );
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
{
    o.TokenLifespan = TimeSpan.FromHours(2);
});
builder.Services.AddScoped<IUserTwoFactorTokenProvider<ApplicationUser>, EmailTokenProvider<ApplicationUser>>();
//builder.Services.AddScoped<IUserTwoFactorTokenProvider<ApplicationUser>, CustomEmailTokenProvider>();


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    IConfigurationSection googleSec = builder.Configuration.GetSection("authentication:Google");
    googleOptions.ClientId = googleSec["client_id"];
    googleOptions.ClientSecret = googleSec["client_secret"];
    //googleOptions.ClientId = builder.Configuration["authentication:Google:client_id"];
    //googleOptions.ClientSecret = builder.Configuration["authentication:Google:client_secret"];
});
#endregion
//----- JWT + Cros
#region JWT + Cros
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = false,
        //   ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("MyPolicy", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        corsPolicyBuilder.WithOrigins("http://localhost:3001","http://localhost:3000");

    });
});
//WithOrignal("http://") React Port
#endregion

//---- Repo Services
#region Repo Inject
builder.Services.AddCoreDependencies();
builder.Services.AddInfrustructureDependencies();
builder.Services.AddServicesDependencies();
#endregion



//--- Razor Page
builder.Services.AddRazorPages();

#region Add Serilog
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseStaticFiles();

//app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();

app.UseCors("MyPolicy"); // ? To enable front end to acces apis
//app.UseMvc(); //?
app.MapRazorPages();
app.MapControllers();


//app.MapControllerRoute(
//    name: "areaRoute",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//);

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


// 
#region Data Seeder
var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    DataSeeder.Seed(service);
}
#endregion

#region Logging
app.Use(async (context, next) =>
{
    LogContext.PushProperty("UserId", context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    LogContext.PushProperty("UserName", context.User.FindFirst(ClaimTypes.Name)?.Value);

    await next();
});

app.UseSerilogRequestLogging();
#endregion


app.Run();
