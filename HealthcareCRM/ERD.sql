-- Users Table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100),
    Email NVARCHAR(100),
    PasswordHash NVARCHAR(255),
    CreatedAt DATETIME
);

-- Patients Table
CREATE TABLE Patients (
    Id INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(255),
    DateOfBirth DATETIME,
    CreatedAt DATETIME
);

-- Appointments Table (future use)
CREATE TABLE Appointments (
    Id INT PRIMARY KEY IDENTITY,
    PatientId INT FOREIGN KEY REFERENCES Patients(Id),
    DoctorName NVARCHAR(100),
    AppointmentDate DATETIME,
    Notes NVARCHAR(500)
);