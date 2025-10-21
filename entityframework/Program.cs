using System.Text.Json.Serialization;
using entityframework.entities;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<MyBoardsContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MyBoardsConnectionString");
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<MyBoardsContext>();

var pendingMigrations = dbContext.Database.GetPendingMigrations();

if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

var users = dbContext.Users.ToList();

if (!users.Any())
{
    var user1 = new User
    {
        Email = "user1@example.com",
        FullName = "User One",
        Address = new Address
        {
            City = "Warszawa",
            Street = "Main Street 1",
        }
    };

    var user2 = new User
    {
        Email = "user2@example.com",
        FullName = "User Two",
        Address = new Address
        {
            City = "Warszawa",
            Street = "Main Street 2",
        }
    };

    dbContext.Users.AddRange(user1, user2);
    dbContext.SaveChanges();
}

var webTag = new Tag { Value = "Web" };
var uiTag = new Tag { Value = "UI" };
var desktopTag = new Tag { Value = "Desktop" };
var apiTag = new Tag { Value = "API" };
var serviceTag = new Tag { Value = "Service" };

dbContext.Tags.AddRange(webTag, uiTag, desktopTag, apiTag, serviceTag);
dbContext.SaveChanges();

app.MapGet("data", async (MyBoardsContext db) =>
{
    var user = await db.Users.Include(u => u.Comments).ThenInclude(c => c.WorkItem).Include(u => u.Address).FirstAsync(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));
    // var userComments = await db.Comments.Where(c => c.UserId == user.Id).ToListAsync();
    return user;
});

app.MapPost("update", async (MyBoardsContext db) =>
{
    var epic = await db.Epics.FirstAsync(e => e.Id == 1);

    epic.Area = "Updated area";
    epic.Priority = 1;
    epic.StartDate = DateTime.Now;

    epic.StateId = 1;

    await db.SaveChangesAsync();
    return epic;
});

app.MapPost("create", async (MyBoardsContext db) =>
{

    var address = new Address()
    {
        Id = Guid.NewGuid(),
        City = "New City123",
        Country = "New Country123",
        Street = "New Street 123231",
        PostalCode = "12345"
    };

    var user = new User()
    {
        Email = "user1231@example.com",
        FullName = "User123 Name",
        Address = address
    };

    db.Users.Add(user);

    await db.SaveChangesAsync();
    return user;
});

app.MapDelete("delete", async (MyBoardsContext db) =>
{

    var workItemTags = await db.WorkItemTag.Where(wt => wt.WorkItemId == 12).ToListAsync();

    // db.WorkItemTag.RemoveRange(workItemTag);
    // await db.SaveChangesAsync();

    // db.RemoveRange(workItemTag);
    // await db.SaveChangesAsync();

    // var workItem = await db.WorkItems.FirstAsync(wi => wi.Id == 16);

    // db.RemoveRange(workItem);
    // await db.SaveChangesAsync();

    var user = await db.Users.FirstAsync(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));

    var userComments = await db.Comments.Where(c => c.UserId == user.Id).ToListAsync();
    db.Comments.RemoveRange(userComments);
    await db.SaveChangesAsync();
    db.Users.Remove(user);
    await db.SaveChangesAsync();
});

app.Run();

