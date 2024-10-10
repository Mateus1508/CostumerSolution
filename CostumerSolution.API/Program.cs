using CostumerSolution.API.Application.Validators;
using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.Interfaces;
using CostumerSolution.API.Infrastructure.Mapping;
using CostumerSolution.API.Infrastructure.Persistence;
using CostumerSolution.API.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//para usar o InMemory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CostumerDb"));

//Para usar o SQL Server
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("ViacepClient", client =>
{
    // Definindo a URL base para a API do ViaCep
    client.BaseAddress = new Uri("https://viacep.com.br");
})
.AddResilienceHandler("ResilienceStrategy", resilienceBuilder =>
{
    // Configuração da política de retry
    resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
    {
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromSeconds(1),
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true,
        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
            .Handle<HttpRequestException>()
            .HandleResult(response => !response.IsSuccessStatusCode)
    });

    // Configuração da política de Circuit Breaker
    resilienceBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
    {
        SamplingDuration = TimeSpan.FromSeconds(10),
        FailureRatio = 0.2,
        MinimumThroughput = 3,
        BreakDuration = TimeSpan.FromSeconds(30),
        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
            .Handle<HttpRequestException>()
            .HandleResult(response => !response.IsSuccessStatusCode)
    });
});
builder.Services.AddScoped<ICostumerRepository, CostumerRepository>();

builder.Services.AddScoped<IValidator<Costumer>, CostumerValidator>();
builder.Services.AddScoped<IValidator<string>, CepValidator>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
