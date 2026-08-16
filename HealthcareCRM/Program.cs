using HealthcareCRM.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

static void EnsureDatabaseSchema(AppDbContext db)
{
    var connection = db.Database.GetDbConnection();
    var connectionString = connection.ConnectionString;

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        db.Database.EnsureCreated();
        return;
    }

    if (connection.State != System.Data.ConnectionState.Open)
        connection.Open();

    using var command = connection.CreateCommand();
    command.CommandText = "SELECT COUNT(*) FROM sys.tables WHERE name = 'Users';";
    var userTableExists = Convert.ToInt32(command.ExecuteScalar());
    if (userTableExists == 0)
    {
        db.Database.EnsureCreated();
        return;
    }

    command.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'IsActive';";
    var hasIsActiveColumn = Convert.ToInt32(command.ExecuteScalar());
    if (hasIsActiveColumn == 0)
    {
        db.Database.ExecuteSqlRaw("ALTER TABLE dbo.Users ADD IsActive bit NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT 1");
    }

    command.CommandText = "SELECT COUNT(*) FROM sys.tables WHERE name = 'AuditLogs';";
    var auditTableExists = Convert.ToInt32(command.ExecuteScalar());
    if (auditTableExists == 0)
    {
        db.Database.ExecuteSqlRaw(@"
            CREATE TABLE dbo.AuditLogs (
                Id int NOT NULL IDENTITY(1,1),
                Action nvarchar(100) NOT NULL,
                EntityType nvarchar(100) NOT NULL,
                EntityId int NULL,
                Details nvarchar(max) NOT NULL,
                PerformedBy nvarchar(max) NOT NULL,
                PerformedByUserId int NULL,
                CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
                CONSTRAINT PK_AuditLogs PRIMARY KEY (Id)
            );");
    }
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex) when (ex is SqlException || ex is InvalidOperationException)
    {
        try
        {
            db.Database.EnsureDeleted();
        }
        catch
        {
            // Ignore cleanup failures and repair schema below.
        }

        db.Database.EnsureCreated();
    }

    if (!db.Database.CanConnect())
    {
        db.Database.EnsureCreated();
    }
    else
    {
        EnsureDatabaseSchema(db);
    }

    DbSeeder.Seed(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    try
    {
       await next();
    }
    catch (Exception ex)
    {
       if (context.Request.Path.StartsWithSegments("/api"))
       {
           context.Response.StatusCode = StatusCodes.Status500InternalServerError;
           context.Response.ContentType = "application/json";
           await context.Response.WriteAsJsonAsync(new
           {
               success = false,
               data = (object?)null,
               message = "An unexpected error occurred. Please try again later.",
               details = app.Environment.IsDevelopment() ? ex.Message : null
           });
           return;
       }

       context.Response.Redirect("/Home/Error");
    }
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "HealthcareCRM API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();