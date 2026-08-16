# Healthcare-CRM

Healthcare-CRM is a .NET MVC clinic management application for managing patients, doctors, appointments, dashboard analytics, and admin access in a healthcare workflow.

## Included features

The current project already contains the core Track A features:

- Login and registration
- Session-based authentication
- Admin and Staff roles
- Patient management with CRUD and search
- Patient API endpoints with pagination and filter support
- Doctor list, create, edit, and active/inactive controls
- Appointment booking and status management
- Dashboard stats endpoint and UI
- Admin user role management
- Swagger API documentation
- EF Core database setup and migrations
- Seeded demo data and default admin credentials

## Default admin account

The app seeds a default administrator on first run:

- Email: `admin@healthcarecrm.com`
- Password: `Admin@123`

This makes it easy to log in immediately in VS Code and demonstrate the admin panel.

## Prerequisites

- .NET 10 SDK
- SQL Server LocalDB or SQL Server Express
- Visual Studio Code
- C# extension for VS Code

## Run in VS Code

1. Open the repository folder in VS Code.
2. Open a terminal at the project root.
3. Restore NuGet packages:

   `cd HealthcareCRM`
   `dotnet restore`

4. Build the app:

   `dotnet build`

5. Make sure the connection string is valid for your machine. The default project setting is:

   `Server=(localdb)\MSSQLLocalDB;Database=HealthcareCRM;Trusted_Connection=True;TrustServerCertificate=True;`

   If you use a different SQL Server instance, update the value in `HealthcareCRM/appsettings.json`.

6. Apply database migrations and seed data:

   `dotnet ef database update`

7. Run the app:

   `dotnet watch run --launch-profile http`

   or

   `dotnet run --launch-profile http`

8. Open the app in the browser:

   `http://localhost:5137`

9. Open Swagger UI:

   `http://localhost:5137/swagger`

## Project structure

- `HealthcareCRM/Controllers/` – MVC controllers and APIs
- `HealthcareCRM/Models/` – domain models and view models
- `HealthcareCRM/Data/` – EF Core database context and seed logic
- `HealthcareCRM/Views/` – Razor pages for UI
- `HealthcareCRM/Migrations/` – EF Core migrations
- `HealthcareCRM/wwwroot/` – front-end assets

## Notes

- The app uses SQL Server, not SQLite.
- The local database is created automatically if the connection is valid.
- The project has been verified to build and run locally in the current environment.
- The seed data creates admin, patients, doctors, and sample appointments so the dashboard and appointment pages are populated immediately.

## Common tasks

- Create patient: `/Patient/Create`
- View patients: `/Patient`
- View appointments: `/Appointment`
- Dashboard: `/Dashboard`
- Admin panel: `/Admin`
