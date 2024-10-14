using EventSourcingMedium.API.Models;
using EventSourcingMedium.API.Services.EventStreaming;
using EventSourcingMedium.API.Services.PostInformationServices.CommandService;
using EventSourcingMedium.API.Services.PostInformationServices.QueryService;
using EventStore.Client;
using EventStore.ClientAPI;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddDbContextPool<AppDbContext>(db => db.UseSqlServer(connectionString));
builder.Services.AddScoped<IPostInformationCommandService, PostInformationCommandService>();
builder.Services.AddScoped<IPostInformationQueryService, PostInformationQueryService>();

builder.Services.AddScoped<IEventStoreService, EventStoreService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//var eventConnectionString = EventStoreConnection.Create(
//    connectionString: EventStoreDB.ConnectionString);

//eventConnectionString.ConnectAsync().GetAwaiter().GetResult();
//builder.Services.AddSingleton(eventConnectionString);
var client = EventStoreClientSettings.Create(EventStoreDB.ConnectionString);
builder.Services.AddSingleton(client);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();