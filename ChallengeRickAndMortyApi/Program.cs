using ChallengeRickAndMortyApi.Infrastructure.DependencyInjection;
using ChallengeRickAndMortyApi.Infrastructure.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

SerilogConfig.ConfigureLogging(builder.Configuration);

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
