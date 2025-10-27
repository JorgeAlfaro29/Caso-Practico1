using PAW3CP1.Architecture;
using PAW3CP1.Core.BusinessLogic;
using PAW3CP1.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using PAW3CP1.Data.Models;
using PAW3CP1.Mvc.ServiceApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

//  Infraestructura: DbContext
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//  Repositorios
builder.Services.AddScoped<IRepositoryTask, RepositoryTask>();

//  Lógica de negocio
builder.Services.AddScoped<ITaskBusiness, TaskBusiness>();

//  Servicios externos / API
builder.Services.AddScoped<IRestProvider, RestProvider>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
