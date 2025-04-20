using Microsoft.EntityFrameworkCore;
using SE160244.ProductManagement.Repo.Models;
using SE160244.ProductManagement.Repo.Repositories.Implement;
using SE160244.ProductManagement.Repo.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json",optional:false, reloadOnChange:true);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

//builder.Services.AddDbContext<MyStoreDBContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB")));
//builder.Services.AddScoped<MyStoreDBContext>();

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
