using Microsoft.EntityFrameworkCore;
using WebApi.DAL;
using WebApi.DAL.Interfaces;
using WebApi.DAL.Repositories;
using WebApi.Domain.Entity;
using WebApi.Service;
using WebApi.Service.Implementations;
using WebApi.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Password>, PasswordsRepository>();
builder.Services.AddScoped<IRepository<BlockedUser>, BlockedRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddSingleton<ICryptography, Cryptography>();
builder.Services.AddHostedService<TimeHostedService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
});

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
