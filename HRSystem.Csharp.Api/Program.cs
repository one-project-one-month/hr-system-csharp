using HRSystem.Csharp.Domain;
using HRSystem.Csharp.Domain.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// BL Service
builder.Services.AddScoped<BL_MenuGroup>();
builder.Services.AddScoped<DA_MenuGroup>();
builder.Services.AddScoped<BL_Menu>();
builder.Services.AddScoped<DA_Menu>();

builder.AddDomain();

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
