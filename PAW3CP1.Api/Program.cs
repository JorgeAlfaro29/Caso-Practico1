using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PAW3CP1.Core.BusinessLogic;
using PAW3CP1.Data.Models;
using PAW3CP1.Data.Repositories;
using PAW3CP1.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Business Logic
builder.Services.AddScoped<ITaskBusiness, TaskBusiness>();

// Repositories
builder.Services.AddScoped<IRepositoryTask, RepositoryTask>();

builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// este es el Endpoint para actualizar estado de aprobación
app.MapPut("/approvals/{id}", async (int id, string status, ClaimsPrincipal user, ITaskBusiness business) =>
{
    var role = user.IsInRole("Manager") ? "Manager" :
               user.IsInRole("SystemAdmin") ? "SystemAdmin" : "User";

    var result = await business.UpdateApprovalStatusAsync(id, status, role);
    return Results.Ok(new { message = result });
}).RequireAuthorization();

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