using HeroesAPI.Services;
using Microsoft.Azure.Cosmos;
using HeroesAPI.Controllers;
using HeroesAPI.Helpers;

var builder = WebApplication.CreateBuilder(args);

string MyAllowAllOrigins = "_myAllowAllOrigins";
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowAllOrigins,
                      policy =>
                      {
                          policy.WithOrigins().AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });

    options.AddPolicy(name: MyAllowSpecificOrigins,
                  policy =>
                  {
                      policy.WithOrigins("http://example.com",
                                          "http://www.contoso.com");
                  });
});

// Add services to the container.
string? cosmosConnectionString = builder.Configuration.GetConnectionString("CosmosDB");

CosmosClient client = new(cosmosConnectionString);
// Services
builder.Services.AddSingleton(client);
builder.Services.AddTransient<ServiceCosmosDb>();

// Helpers
builder.Services.AddSingleton<HelperDocumentId>();
HelperCosmosDb helperCosmosDb = new(builder.Configuration, client);
builder.Services.AddSingleton(helperCosmosDb);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
