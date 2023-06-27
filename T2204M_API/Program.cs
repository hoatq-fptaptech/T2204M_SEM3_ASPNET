using Microsoft.EntityFrameworkCore;
using System.Text;
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
builder.Services.AddDbContext<T2204M_API.Entities.T2204mApiContext>(
        options => options.UseSqlServer(connectionString)
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

