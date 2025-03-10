using AzureCosmosDbApp.Data;
using AzureCosmosDbApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(provider =>
{
    var configuration = builder.Configuration;
    var service = new CosmosDbService(
        endpoint: configuration["CosmosDb:Endpoint"],
        key: configuration["CosmosDb:Key"],
        databaseId: "EcommerceDB",
        containerId: "Products"
    );

    Task.Run(async () => await service.InitializeCosmosDbAsync()).Wait();

    return service;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();