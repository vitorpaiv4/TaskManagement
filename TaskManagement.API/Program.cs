using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Application.Services;
using TaskManagement.Application.Factories;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Strategies;
using AppTaskFactory = TaskManagement.Application.Factories.TaskFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TaskManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskFactory, AppTaskFactory>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskStatusStrategy, CompletedStatusStrategy>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "OK", message = "TaskManagement API is running" }));

app.MapGet("/api/tasks", (ITaskService taskService) =>
{
    var tasks = taskService.GetTaskDetails(0); // <-- Isso está errado!
    return Results.Ok(tasks);
});

app.MapGet("/api/tasks/{id}", (int id, ITaskService taskService) =>
{
    var task = taskService.GetTaskDetails(id);
    return task is not null ? Results.Ok(task) : Results.NotFound();
});

app.MapPost("/api/tasks", (CreateTaskRequest request, ITaskService taskService) =>
{
    var task = taskService.Create(request.Title, request.Description, request.ResponsibleUserId);
    return Results.Created($"/api/tasks/{task.Id}", task);
});

app.MapPut("/api/tasks/{id}/status", (int id, UpdateStatusRequest request, ITaskService taskService) =>
{
    try
    {
        var task = taskService.UpdateStatus(id, request.NewStatus);
        return Results.Ok(task);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();

public record CreateTaskRequest(string Title, string Description, int? ResponsibleUserId);
public record UpdateStatusRequest(string NewStatus);
