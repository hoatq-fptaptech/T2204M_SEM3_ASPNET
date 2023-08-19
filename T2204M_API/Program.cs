using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
var builder = WebApplication.CreateBuilder(args);

// Add Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
            policy =>
            {
                //policy.WithOrigins("https://24h.com.vn");
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            }
        );
});
// Add services to the container.


builder.Services.AddControllers()
    .AddNewtonsoftJson(options=> options.SerializerSettings.ReferenceLoopHandling
        = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Connection Database
var connectionString = builder.Configuration.GetConnectionString("T2204M_API");
T2204M_API.Entities.T2204mApiContext.ConnectionString = connectionString;
builder.Services.AddDbContext<T2204M_API.Entities.T2204mApiContext>(
        options => options.UseSqlServer(connectionString)
    );
// Add Authetication JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
// add Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy =>
                policy.RequireClaim(ClaimTypes.Email, "admin@gmail.com"));
//options.AddPolicy("SuperAdminXX", policy =>
//            policy.RequireClaim(ClaimTypes.Email));
options.AddPolicy("ValidYearOld", policy => policy.AddRequirements(
            new T2204M_API.Requirements.YearOldRequirement(
                Convert.ToInt32(builder.Configuration["ValidYearOld:Min"]),
                Convert.ToInt32(builder.Configuration["ValidYearOld:Max"])
            )
        )
    );
});
builder.Services.AddSingleton<IAuthorizationHandler, T2204M_API.Handlers.ValidYearOldHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

