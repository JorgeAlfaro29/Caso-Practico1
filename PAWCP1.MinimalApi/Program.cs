using Microsoft.EntityFrameworkCore;
using PAW3CP1.Core.BusinessLogic;
using PAW3CP1.Data.Models;
using PAW3CP1.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TaskDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// Registrar repositorio y business en DI
builder.Services.AddScoped<IRepositoryTask, RepositoryTask>();
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();
builder.Services.AddScoped<ITaskBusiness, TaskBusiness>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
var app = builder.Build();

//obtener todos los prodcutos
app.MapGet("/Task", async (ITaskBusiness taskBusiness) =>
    Results.Ok(await taskBusiness.GetTask(id: null)));


app.MapGet("/Task/complete", async (TaskDbContext db) =>
    await db.Tasks.Where(t => t.Id != 0).ToListAsync());

app.MapGet("/login", async (string email, IUserBusiness userBusiness) =>
{
    try
    {
        var user = await userBusiness.ValidateLogin(email);
        return Results.Ok(user);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { message = ex.Message });
    }
});



app.Run();
