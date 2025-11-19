using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Application.Strategies;
using TaskManagement.Application.Factories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TaskManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IMovementRepository, MovementRepository>();
builder.Services.AddScoped<IMovementFactory, MovementFactory>();
builder.Services.AddScoped<IMovementService, MovementService>();
builder.Services.AddScoped<IMovementStrategy, EntryStrategy>();
builder.Services.AddScoped<IMovementStrategy, ExitStrategy>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Sistema de Controle de Estoque API",
        Version = "v1",
        Description = "API para gerenciamento de produtos e movimentações de estoque"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Controle de Estoque API v1");
        c.RoutePrefix = "swagger";
    });
}

app.MapGet("/health", () => Results.Ok(new { status = "OK", message = "Sistema de Controle de Estoque - API Running" }));

app.MapGet("/api/products", (IProductRepository productRepository) =>
{
    var products = productRepository.GetAll();
    return Results.Ok(products);
});

app.MapGet("/api/products/{id}", (int id, IProductRepository productRepository) =>
{
    var product = productRepository.GetById(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapPost("/api/products", (CreateProductRequest request, IProductRepository productRepository) =>
{
    var product = new TaskManagement.Domain.Entities.Product
    {
        Sku = request.Sku,
        Name = request.Name,
        Description = request.Description,
        Price = request.Price,
        StockQuantity = 0,
        CreatedAt = DateTime.UtcNow
    };

    var created = productRepository.Add(product);
    return Results.Created($"/api/products/{created.Id}", created);
});

app.MapGet("/api/movements", (IMovementRepository movementRepository) =>
{
    var movements = movementRepository.GetAll();
    return Results.Ok(movements);
});

app.MapGet("/api/movements/{id}", (int id, IMovementService movementService) =>
{
    var movement = movementService.GetMovementDetails(id);
    return movement is not null ? Results.Ok(movement) : Results.NotFound();
});

app.MapPost("/api/movements", (CreateMovementRequest request, IMovementService movementService) =>
{
    try
    {
        var movement = movementService.ProcessNewMovement(request.ProductId, request.Quantity, request.Type);
        return Results.Created($"/api/movements/{movement.Id}", movement);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();

public record CreateProductRequest(string Sku, string Name, string Description, decimal Price);
public record CreateMovementRequest(int ProductId, int Quantity, string Type);
