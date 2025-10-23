using Microsoft.EntityFrameworkCore;
using PAW3CP1.Core.BusinessLogic;
using PAW3CP1.Data.Models;
using PAW3CP1.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TaskDb2Context>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// Registrar repositorio y business en DI
builder.Services.AddScoped<IRepositoryTask, RepositoryTask>();
builder.Services.AddScoped<ITaskBusiness, TaskBusiness>();
var app = builder.Build();

//obtener todos los prodcutos
app.MapGet("/Task", async (ITaskBusiness taskBusiness) =>
    Results.Ok(await taskBusiness.GetTask(id: null)));


app.MapGet("/Task/complete", async (TaskDb2Context db) =>
    await db.Tasks.Where(t => t.Id != 0).ToListAsync());


app.Run();
