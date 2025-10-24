using System.Linq.Expressions;
using System.Text.Json.Serialization;
using entityframework.dto;
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
    options
    // .UseLazyLoadingProxies()
    .UseSqlServer(connectionString);
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

app.MapGet("pagination", async (MyBoardsContext db) =>
{
    // user input
    var filter = "a";
    string sortBy = "FullName";
    bool sortByDescending = false;
    int pageNumber = 2;
    int pageSize = 5;

    var query = db.Users.Where(u => filter == null || u.FullName.ToLower().Contains(filter.ToLower()) || u.Email.ToLower().Contains(filter.ToLower()));

    var totalItems = query.Count();

    if (sortBy != null)
    {
        var columnsSelector = new Dictionary<string, Expression<Func<User, object>>>
        {
            {nameof(User.Email), user => user.Email},
            {nameof(User.FullName), user => user.FullName},
        };

        var sortByExpression = columnsSelector[sortBy];
        query = sortByDescending ? query.OrderByDescending(sortByExpression) : query.OrderBy(sortByExpression);
    }

    var result = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

    var pagedResult = new PagedResult<User>(result, totalItems, pageSize, pageNumber);
    return pagedResult;
});

app.MapGet("data", async (MyBoardsContext db) =>
{
    // var user = await db.Users.FirstAsync(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));

    // var entries1 = db.ChangeTracker.Entries();

    // user.Email = "user1_updated@example.com";

    // var entries2 = db.ChangeTracker.Entries();

    // db.SaveChanges();
    // var userComments = await db.Comments.Where(c => c.UserId == user.Id).ToListAsync();

    // var minWorkItemsCount = "85";

    // var states = db.States.FromSqlInterpolated($@"
    //     SELECT wis.id, wis.Message
    //     FROM States wis
    //     JOIN WorkItems wi on wi.StateId = wis.Id
    //     GROUP BY wis.Id, wis.Message
    //     HAVING COUNT(wi.Id) > {minWorkItemsCount}").ToList();


    // db.Database.ExecuteSqlRaw(@"
    //     UPDATE Comments
    //     SET UpdatedDate = GETDATE()
    //     WHERE Id = 2");
    // return states;

    // var topAuthors = db.ViewTopAuthors.ToList();
    // return topAuthors;

    // var withAddress = true;

    // var user = db.Users.First(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));

    // if (withAddress)
    // {
    //     var result = new { FullName = user.FullName, Address = $"{user.Address.Street}, {user.Address.City}" };
    //     return result;
    // }

    // var addresses = db.Addresses.Where(a => a.Coordinate.Latitude > 10).ToList();
    // return new { FullName = user.FullName, Address = "-" };

    var comments = await db.Users.Include(u => u.Address).Include(u => u.Comments).Where(u => u.Address.Country == "Albania").SelectMany(u => u.Comments).Select(c => c.Message).ToListAsync();

    return comments;
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

    var user = await db.Users.Include(u => u.Comments).FirstAsync(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));

    // var userComments = await db.Comments.Where(c => c.UserId == user.Id).ToListAsync();
    // db.Comments.RemoveRange(userComments);
    // await db.SaveChangesAsync();
    db.Users.Remove(user);
    await db.SaveChangesAsync();
});

app.Run();

