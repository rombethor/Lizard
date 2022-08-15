using Lizard;
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

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHealthChecks("/healthz");

var messageQueueListener = new Lizard.Messaging.MessageQueueListener(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
