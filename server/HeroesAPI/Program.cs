using HeroesAPI.Services;
using Microsoft.Azure.Cosmos;
using HeroesAPI.Helpers;
using HeroesAPI.Data;
using Microsoft.EntityFrameworkCore;
using HeroesAPI.Repositories;
using HeroesAPI.Models;

var builder = WebApplication.CreateBuilder(args);

string MyAllowAllOrigins = "_myAllowAllOrigins";
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowAllOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });

    options.AddPolicy(name: MyAllowSpecificOrigins,
                  policy =>
                  {
                      policy.WithOrigins("http://localhost:4000", "https://localhost:4000");
                  });
});

// Add services to the container.

builder.Services.AddDbContext<HeroesAppContext>(options =>
{
    options.UseSqlServer("Data Source=FELIX-LAPTOP;Initial Catalog=heroes;Persist Security Info=True;User ID=sa;Trust Server Certificate=True");
});

builder.Services.AddTransient<RepositoryAddressesSql>();
builder.Services.AddTransient<RepositoryUsersSql>();

string? cosmosConnectionString = builder.Configuration.GetConnectionString("CosmosDB");

// Services
CosmosClient client = new(cosmosConnectionString);
builder.Services.AddSingleton(client);
builder.Services.AddTransient<ServiceCosmosDb>();

// Helpers
builder.Services.AddSingleton<HelperDocumentId>();
builder.Services.AddSingleton<HelperOAuthToken>();

// Security
HelperOAuthToken helperOAuth = new(builder.Configuration);
builder.Services.AddAuthentication(helperOAuth.GetAuthenticationOptions()).AddJwtBearer(helperOAuth.GetJwtOptions());
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ADMIN_ONLY", policy => policy.RequireRole(UserRole.Admin.ToString()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(MyAllowAllOrigins);
}
else
{
    app.UseCors(MyAllowSpecificOrigins);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
