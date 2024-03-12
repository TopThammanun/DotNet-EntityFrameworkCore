using DotNet_EntityFrameworkCore.DataCore;
using DotNet_EntityFrameworkCore.Domain;
using DotNet_EntityFrameworkCore.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ITDBContext, TDBContext>();
builder.Services.AddSingleton<IDbModelConfigure, DBConfigure>();
builder.Services.AddScoped<TUnitOfWork, TUnitOfWork>();
builder.Services.AddScoped<IPlanService, PlanService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();