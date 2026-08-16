# Healthcare CRM

Healthcare CRM is a healthcare management web application built with ASP.NET Core MVC and Entity Framework Core. It is designed to support patient management, doctor records, appointment scheduling, dashboard analytics, and administrative workflows for a clinic or healthcare operations team.

This project provides a complete local demo setup for the Track A web app and is intended to run easily in Visual Studio Code.

## Overview

The application includes the core healthcare CRM workflows needed for a working demo:

- secure login and registration
- staff/admin role separation
- patient CRUD operations
- patient search and pagination
- doctor listing and status management
- appointment booking and management
- dashboard overview
- admin controls and audit tracking
- seeded demo data
- Swagger API documentation

## Tech Stack

- ASP.NET Core MVC
- .NET 10
- Entity Framework Core
- SQL Server / LocalDB
- Razor Views
- Bootstrap
- Swagger / OpenAPI

## Features

### Authentication and Authorization
- Login and registration workflows
- Session-based authentication
- Admin and staff role assignment
- Access control for admin-only pages

### Patient Management
- Add, view, edit, and delete patients
- Search by name, phone number, or date of birth
- Pagination for large patient lists
- Patient detail views and validation handling

### Doctor Management
- Doctor directory and basic status tracking
- Active/inactive doctor logic
- Listing support for Track A demo operations

### Appointments
- Book appointments against patient and doctor records
- View appointment status updates
- Track appointment notes and scheduling data

### Admin Panel
- Change user roles
- Activate or deactivate accounts
- Export user records as CSV
- Review recent activity audit log

### API Documentation
- Swagger UI is enabled at `/swagger`
- Core patient and doctor endpoints are exposed for testing and demo use

## Default Demo Accounts

The app seeds default accounts automatically on first run.

### Admin
- Email: `admin@healthcarecrm.com`
- Password: `Admin@123`

### Staff
- Email: `staff@healthcarecrm.com`
- Password: `Staff@123`

## Project Structure

```text
HealthcareCRM/
├── Controllers/         # MVC and API controllers
├── Data/                # DbContext and seed logic
├── Models/              # Domain and view models
├── Migrations/          # EF Core migrations
├── Views/               # Razor UI pages
├── wwwroot/             # Static frontend assets
├── appsettings.json     # App configuration
├── Program.cs           # Startup and DB bootstrap
├── HealthcareCRM.csproj # Project configuration
└── README.md            # Project documentation
```

## Prerequisites

Before running the project, ensure the following are installed:

- .NET 10 SDK
- SQL Server LocalDB or SQL Server Express
- Visual Studio Code
- C# extension for VS Code

## Getting Started

1. Open the project folder in VS Code.
2. Open a terminal in the project directory.
3. Restore packages:

```bash
cd HealthcareCRM
dotnet restore
```

4. Build the project:

```bash
dotnet build
```

5. Run the app:

```bash
dotnet run --urls http://localhost:5137
```

6. Open the app in your browser:

```text
http://localhost:5137
```

7. Open Swagger UI:

```text
http://localhost:5137/swagger
```

## Database Configuration

The app uses SQL Server LocalDB by default. Update the connection string in `HealthcareCRM/appsettings.json` if you are using a different SQL Server instance.

On first run, the application creates the database if needed and populates the database with demo data.

## Running Notes

- The app is designed to run locally in VS Code.
- If the app is already running and the exe is locked, stop the old `HealthcareCRM` process before restarting.
- The database is seeded with sample patients, doctors, appointments, and users for demonstration purposes.

## Demo Flow

A professional demo can follow this flow:

1. Log in as admin
2. Open the dashboard
3. Review patient list
4. Search for a patient
5. Create a new patient
6. Edit patient details
7. Delete a patient after confirmation
8. Show the doctor list
9. Open the admin panel
10. Show API docs in Swagger

## Sprint Status

This project is a working Track A healthcare CRM web application with the core features implemented and locally validated.

Completed elements include:

- authentication and access control
- patient management
- patient filtering and pagination
- doctor module base setup
- admin controls
- startup and database repair logic
- seeded demo data

## License

This project is intended for educational and internship/demo use. Please confirm before reusing it outside the project scope.
