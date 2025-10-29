using Microsoft.EntityFrameworkCore;
using PAW3CP1.Core.BusinessLogic;
using PAW3CP1.Data.Models;
using PAW3CP1.Data.Repositories;
using PAW3CP1.Models.DTO;

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

app.MapGet("/userroles/view", async (TaskDbContext db) =>
{
    var roles = await db.Roles
        .Select(r => new RoleDTO
        {
            RoleId = r.RoleId,
            RoleName = r.RoleName,
            Description = r.Description
        }).ToListAsync();

    var users = await db.Users
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
        .ToListAsync();

    var result = users.Select(u => new UserRoleViewDTO
    {
        UserId = u.UserId,
        Email = u.Email,
        FullName = u.FullName ?? "",
        CurrentRoleId = u.UserRoles.FirstOrDefault()?.RoleId ?? 0,
        CurrentRoleName = u.UserRoles.FirstOrDefault()?.Role?.RoleName ?? "Sin rol",
        AvailableRoles = roles
    }).ToList();

    return Results.Ok(result);
});

app.MapPost("/userroles/assign", async (TaskDbContext db, AssignRoleRequest request) =>
{
var user = await db.Users.FindAsync(request.UserId);
var role = await db.Roles.FindAsync(request.RoleId);

if (user == null || role == null)
return Results.BadRequest("Usuario o rol inválido.");

var existingRoles = db.UserRoles.Where(ur => ur.UserId == request.UserId);
db.UserRoles.RemoveRange(existingRoles); // Elimina el rol anterior

db.UserRoles.Add(new UserRole
{
UserId = request.UserId,
RoleId = request.RoleId,
Description = $"Asignado el {DateTime.Now:g}"
});

await db.SaveChangesAsync();
return Results.Ok("success");
});

app.Run();


public class AssignRoleRequest
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
}


