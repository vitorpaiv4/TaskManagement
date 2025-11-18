using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Application.Services;
using TaskManagement.Application.Factories;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Strategies;
using TaskManagement.Domain.Entities; // Necessário para TaskItem
using System.Collections.Generic;
using System;

// Removendo using's redundantes para evitar conflitos de extensão (o Swashbuckle já está no pacote)
// using Swashbuckle.AspNetCore.SwaggerUI; 
// using Microsoft.AspNetCore.Builder; 
// using Microsoft.Extensions.DependencyInjection; 

using AppTaskFactory = TaskManagement.Application.Factories.TaskFactory;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. Configuração dos Serviços (Injeção de Dependência)
// =========================================================================

// Configuração do Swagger para Minimal APIs (necessita do pacote Swashbuckle.AspNetCore)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do DbContext e String de Conexão
builder.Services.AddDbContext<TaskManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro dos Contratos e Implementações (SOLID/DIP)
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskFactory, AppTaskFactory>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Registro do Padrão Strategy
builder.Services.AddScoped<ITaskStatusStrategy, CompletedStatusStrategy>();

var app = builder.Build();

// =========================================================================
// 2. Configuração dos Middlewares (Pipeline de Requisição)
// =========================================================================

// Habilita o Swagger e Swagger UI APENAS no ambiente de Desenvolvimento
if (app.Environment.IsDevelopment())
{
    // Esses métodos de extensão agora devem ser encontrados pelo compilador
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// =========================================================================
// 3. Definição dos Endpoints (Rotas da API)
// =========================================================================

// HEALTH CHECK
app.MapGet("/health", () => Results.Ok(new { status = "OK", message = "TaskManagement API is running" }));

// GET ALL: Obtém todas as tarefas
app.MapGet("/api/tasks", (ITaskService taskService) =>
{
    var tasks = taskService.GetAllTasks();
    return Results.Ok(tasks);
});

// GET BY ID: Obtém uma tarefa por ID
app.MapGet("/api/tasks/{id}", (int id, ITaskService taskService) =>
{
    var task = taskService.GetTaskDetails(id);
    return task is not null ? Results.Ok(task) : Results.NotFound();
});

// POST: Cria uma nova tarefa
app.MapPost("/api/tasks", (CreateTaskRequest request, ITaskService taskService) =>
{
    try
    {
        var task = taskService.Create(request.Title, request.Description, request.ResponsibleUserId);
        return Results.Created($"/api/tasks/{task.Id}", task);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

// PUT: Atualiza o status da tarefa
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

// Records (Modelos de Requisição)
public record CreateTaskRequest(string Title, string Description, int? ResponsibleUserId);
public record UpdateStatusRequest(string NewStatus);