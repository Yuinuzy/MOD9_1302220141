
using NSwag.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Model.mahasiswa;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<mahasiswadb>(opt => opt.UseInMemoryDatabase("mahasiswa"));
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

// configure swagger tutorial di https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio-code
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/", async (mahasiswadb db) =>
{
    // default data using array list 
    var mhs = new mahasiswa[]
    {
        new mahasiswa { Id = 1, Nama = "Ahmad Fadli Akbar", Nim = "1302220126"},
        new mahasiswa { Id = 2, Nama = "Syauqi Dhiya Ul Haq", Nim = "1302220982" },
        new mahasiswa { Id = 3, Nama = "Raphael Permana Barus", Nim = "1302220142" },
        new mahasiswa { Id = 4, Nama = "M Tsaqif Zayyan", Nim = "1302220141" },
        new mahasiswa { Id = 5, Nama = "Nicholas Xavier R.", Nim = "1302220212" },
        new mahasiswa { Id = 6, Nama = "Rafie Aydin Ihsan", Nim = "1302221456" },
    };
    db.mhs.AddRange(mhs);
    await db.SaveChangesAsync();
    // dari aspnetcore.httml
    return Results.Redirect("/swagger"); // redirect to swagger
});

app.MapGet("/mahasiswa", async (mahasiswadb db) =>
{

    return Results.Ok(await db.mhs.ToListAsync());
});

app.MapGet("/mahasiswa/{id}", async (mahasiswadb db, int id) =>
{
    var mhs = await db.mhs.FindAsync(id);
    if (mhs == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(mhs);
});

app.MapPost("/mahasiswa", async (mahasiswadb db, mahasiswa mhs) =>
{
    Console.WriteLine(mhs);
    db.mhs.Add(mhs);
    await db.SaveChangesAsync();
    return Results.Created($"/mahasiswa/{mhs.Id}", mhs);
});

app.MapPut("/mahasiswa/{id}", async (mahasiswadb db, int id, mahasiswa mhs) =>
{
    if (id != mhs.Id)
    {
        return Results.BadRequest();
    }
    db.Entry(mhs).State = EntityState.Modified;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/mahasiswa/{id}", async (mahasiswadb db, int id) =>
{
    var mhs = await db.mhs.FindAsync(id);
    if (mhs == null)
    {
        return Results.NotFound();
    }
    db.mhs.Remove(mhs);
    await db.SaveChangesAsync();
    return Results.NoContent();
});







app.Run();
