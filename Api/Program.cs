using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

// Add and configure database context
builder.Services.AddDbContext<AppDbContext>(options => {

    var connectionString = builder.Configuration.GetConnectionString("Default");
    if (string.IsNullOrEmpty(connectionString)) throw new Exception("Connection string not found");
    
    options.UseSqlite(connectionString.Replace("{path}", AppContext.BaseDirectory));
});

builder.Services.AddTransient<IUserDbService, UserDbService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseReDoc( option => option.SpecUrl = "/openapi/v1.json");
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.Run();
