using PAW3CP1.Core.BusinessLogic;
using PAW3CP1.Data.Repositories;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Business Logic
builder.Services.AddScoped<ITaskBusiness, TaskBusiness>();

// Repositories
builder.Services.AddScoped<IRepositoryTask, RepositoryTask>();

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

app.Run();