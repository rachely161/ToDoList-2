using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TodoApi;


var builder = WebApplication.CreateBuilder(args);

// הוספת DbContext לשירותים
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// הפעלת מדיניות CORS
app.UseCors("AllowAllOrigins");



if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}



app.MapGet("/", () => "Hello World!");


app.MapGet("/tasks", async (ToDoDbContext db) => await db.Items.ToListAsync()); // שליפת כל המשימות

app.MapPost("/tasks", async (Item item, ToDoDbContext db) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{item.Id}", item); // הוספת משימה חדשה
});

app.MapPut("/tasks/{id}", async (int id, bool isComplete, ToDoDbContext db) =>
{
    var task = await db.Items.FindAsync(id);
    if (task == null) return Results.NotFound();

    task.IsComplete = isComplete;
    await db.SaveChangesAsync();
    return Results.Ok(task);
});

app.MapDelete("/tasks/{id}", async (int id, ToDoDbContext db) =>
{
    var task = await db.Items.FindAsync(id);
    if (task == null) return Results.NotFound();

    db.Items.Remove(task);
    await db.SaveChangesAsync(); // מחיקת משימה
    return Results.NoContent();
});




app.Run();