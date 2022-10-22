using Lizard;
using Lizard.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<LizardDbContext>(
    svc => new LizardDbContext(svc.UseSqlServer(builder.Configuration["database"]).Options)
);

new LizardDbContext(new DbContextOptionsBuilder().UseSqlServer(builder.Configuration["database"]).Options).Database.Migrate();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddCheck("readiness", new ReadinessHealthCheck(builder.Configuration));

var app = builder.Build();

app.UseHealthChecks("/healthz");
app.MapHealthChecks("/readyz", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    Predicate = x => x.Tags.Contains("readiness")
});

var messageQueueListener = new Lizard.Messaging.MessageQueueListener(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || !string.IsNullOrWhiteSpace(app.Configuration["swagger"]))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
