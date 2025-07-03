USE Medicare_DB;
```
-- 1. Create the Database
CREATE DATABASE Medicare_DB;
GO

USE Medicare_DB;
GO
SELECT * FROM Users
SELECT * FROM Roles

-- 2. Create Roles Table
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) UNIQUE NOT NULL
);
GO

-- 3. Insert Role Data
INSERT INTO Roles (RoleName) VALUES
('Admin'),
('Receptionist'),
('Doctor'),
('Pharmacist'),
('Lab');
GO

--insert dummy admin data
INSERT INTO Users (Username, Password, RoleId) VALUES 
('admin', 'admin123', 1)

-- 4. Create Users Table with FK to Roles
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    [Password] NVARCHAR(100) NOT NULL,
    RoleId INT NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);
GO



CREATE OR ALTER  PROCEDURE sp_ValidateUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(100)
AS
BEGIN
    SELECT 
        U.UserId,
        U.Username,
        U.Password,
        R.RoleName AS Role
    FROM Users U
    JOIN Roles R ON U.RoleId = R.RoleId
    WHERE U.Username = @Username AND U.Password = @Password;
END
GO


--SP for Admin 
--for receptonist
CREATE OR ALTER PROCEDURE sp_InsertReceptionist
    @Name NVARCHAR(100),
    @Contact NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EmpId NVARCHAR(10)
    DECLARE @Password NVARCHAR(20)
    DECLARE @Username NVARCHAR(100)

    SELECT @EmpId = 'REC' + RIGHT('0000' + CAST(ISNULL(MAX(ReceptionistId), 0) + 1 AS VARCHAR), 4)
    FROM Receptionists

    SET @Username = @Name + RIGHT(CAST(ABS(CHECKSUM(NEWID())) % 100 AS VARCHAR), 2)
    SET @Password = LEFT(@Name, 3) + RIGHT(CAST(ABS(CHECKSUM(NEWID())) % 1000 AS VARCHAR), 3)

    INSERT INTO Receptionists (RecepEmpId, Name, Contact, IsActive, Username, Password)
    VALUES (@EmpId, @Name, @Contact, 1, @Username, @Password)

    INSERT INTO Users (Username, Password, RoleId)
    VALUES (@Username, @Password, 2)
END
GO




CREATE OR ALTER PROCEDURE sp_GetReceptionists
AS
BEGIN
    SELECT * FROM Receptionists WHERE IsActive = 1
END
GO


CREATE PROCEDURE sp_GetReceptionistById
    @ReceptionistId INT
AS
BEGIN
    SELECT * FROM Receptionists WHERE ReceptionistId = @ReceptionistId
END
GO

CREATE OR ALTER PROCEDURE sp_UpdateReceptionist
    @ReceptionistId INT,
    @Name NVARCHAR(100),
    @Contact NVARCHAR(20)
AS
BEGIN
    UPDATE Receptionists
    SET Name = @Name, Contact = @Contact
    WHERE ReceptionistId = @ReceptionistId
END
GO

CREATE OR ALTER PROCEDURE sp_DeactivateReceptionist
    @ReceptionistId INT
AS
BEGIN
    UPDATE Receptionists SET IsActive = 0 WHERE ReceptionistId = @ReceptionistId
END
GO

--for doctor

CREATE OR ALTER PROCEDURE sp_InsertDoctor
    @Name NVARCHAR(100),
    @Contact NVARCHAR(20)
AS
BEGIN
    DECLARE @EmpId NVARCHAR(10)
    DECLARE @Password NVARCHAR(20)
    DECLARE @Username NVARCHAR(100)

    SELECT @EmpId = 'DOC' + RIGHT('0000' + CAST(ISNULL(MAX(DoctorId), 0) + 1 AS VARCHAR), 4)
    FROM Doctors

    SET @Username = @Name + RIGHT(CAST(ABS(CHECKSUM(NEWID())) % 100 AS VARCHAR), 2)
    SET @Password = LEFT(@Name, 3) + RIGHT(CAST(ABS(CHECKSUM(NEWID())) % 1000 AS VARCHAR), 3)

    INSERT INTO Doctors (DocEmpId, Name, Contact, IsActive, Username, Password)
    VALUES (@EmpId, @Name, @Contact, 1, @Username, @Password)

    INSERT INTO Users (Username, Password, RoleId)
    VALUES (@Username, @Password, 3)
END
GO


CREATE OR ALTER PROCEDURE sp_GetDoctors AS
BEGIN
    SELECT * FROM Doctors WHERE IsActive = 1
END
GO


CREATE OR ALTER PROCEDURE sp_GetDoctorById
    @DoctorId INT
AS
BEGIN
    SELECT * FROM Doctors WHERE DoctorId = @DoctorId
END
GO

CREATE OR ALTER PROCEDURE sp_UpdateDoctor
    @DoctorId INT,
    @Name NVARCHAR(100),
    @Contact NVARCHAR(20)
AS
BEGIN
    UPDATE Doctors SET Name = @Name, Contact = @Contact WHERE DoctorId = @DoctorId
END
GO

CREATE OR ALTER PROCEDURE sp_DeactivateDoctor
    @DoctorId INT
AS
BEGIN
    UPDATE Doctors SET IsActive = 0 WHERE DoctorId = @DoctorId
END
GO


--Pharmacist

CREATE OR ALTER PROCEDURE sp_InsertPharmacist
    @Name NVARCHAR(100),
    @Contact NVARCHAR(20)
AS
BEGIN
    DECLARE @EmpId NVARCHAR(10)
    DECLARE @Password NVARCHAR(20)
    DECLARE @Username NVARCHAR(100)

    SELECT @EmpId = 'PHA' + RIGHT('0000' + CAST(ISNULL(MAX(PharmacistId), 0) + 1 AS VARCHAR), 4)
    FROM Pharmacists

    SET @Username = @Name + RIGHT(CAST(ABS(CHECKSUM(NEWID())) % 100 AS VARCHAR), 2)
    SET @Password = LEFT(@Name, 3) + RIGHT(CAST(ABS(CHECKSUM(NEWID())) % 1000 AS VARCHAR), 3)

    INSERT INTO Pharmacists (PhaEmpId, Name, Contact, IsActive, Username, Password)
    VALUES (@EmpId, @Name, @Contact, 1, @Username, @Password)

    INSERT INTO Users (Username, Password, RoleId)
    VALUES (@Username, @Password, 4)
END
GO


CREATE OR ALTER PROCEDURE sp_GetPharmacists AS
BEGIN
    SELECT * FROM Pharmacists WHERE IsActive = 1
END
GO

CREATE PROCEDURE sp_GetPharmacistById
    @PharmacistId INT
AS
BEGIN
    SELECT * FROM Pharmacists WHERE PharmacistId = @PharmacistId
END
GO

CREATE OR ALTER PROCEDURE sp_UpdatePharmacist
    @PharmacistId INT,
    @Name NVARCHAR(100),
    @Contact NVARCHAR(20)
AS
BEGIN
    UPDATE Pharmacists SET Name = @Name, Contact = @Contact WHERE PharmacistId = @PharmacistId
END
GO

CREATE OR ALTER PROCEDURE sp_DeactivatePharmacist
    @PharmacistId INT
AS
BEGIN
    UPDATE Pharmacists SET IsActive = 0 WHERE PharmacistId = @PharmacistId
END
GO

--Lab

-- ? INSERT LabTechnician ? Uses table [Lab]
CREATE OR ALTER PROCEDURE sp_InsertLabTechnician
    @Name NVARCHAR(100),
    @Contact NVARCHAR(20)
AS
BEGIN
    DECLARE @EmpId NVARCHAR(10)
    DECLARE @Password NVARCHAR(20)
    DECLARE @Username NVARCHAR(100)

    SELECT @EmpId = 'LAB' + RIGHT('0000' + CAST(ISNULL(MAX(LabId), 0) + 1 AS VARCHAR), 4)
    FROM Lab

    SET @Username = @Name + RIGHT(CAST(ABS(CHECKSUM(NEWID())) % 100 AS VARCHAR), 2)
    SET @Password = LEFT(@Name, 3) + RIGHT(CAST(ABS(CHECKSUM(NEWID())) % 1000 AS VARCHAR), 3)

    INSERT INTO Lab (LabEmpId, Name, Contact, IsActive, Username, Password)
    VALUES (@EmpId, @Name, @Contact, 1, @Username, @Password)

    INSERT INTO Users (Username, Password, RoleId)
    VALUES (@Username, @Password, 5)
END
GO



-- ? GET All LabTechnicians
CREATE OR ALTER PROCEDURE sp_GetLabTechnicians
AS
BEGIN
    SELECT * FROM Lab WHERE IsActive = 1
END
GO

-- ? GET LabTechnician By Id
CREATE OR ALTER PROCEDURE sp_GetLabTechnicianById
    @LabTechnicianId INT
AS
BEGIN
    SELECT * FROM Lab WHERE LabId = @LabTechnicianId
END
GO

-- ? UPDATE LabTechnician
CREATE OR ALTER PROCEDURE sp_UpdateLabTechnician
    @LabTechnicianId INT,
    @Name NVARCHAR(100),
    @Contact NVARCHAR(20)
AS
BEGIN
    UPDATE Lab SET Name = @Name, Contact = @Contact
    WHERE LabId = @LabTechnicianId
END
GO

-- ? DEACTIVATE LabTechnician
CREATE OR ALTER PROCEDURE sp_DeactivateLabTechnician
    @LabTechnicianId INT
AS
BEGIN
    UPDATE Lab SET IsActive = 0
    WHERE LabId = @LabTechnicianId
END
GO


SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Receptionists';


--Tables
CREATE TABLE Receptionists (
    ReceptionistId INT PRIMARY KEY IDENTITY,
    RecepEmpId NVARCHAR(10) UNIQUE,
    Name NVARCHAR(100),
    Contact NVARCHAR(20),
    Username NVARCHAR(100),
    Password NVARCHAR(100),
    IsActive BIT
);

CREATE TABLE Doctors (
    DoctorId INT PRIMARY KEY IDENTITY,
    DocEmpId NVARCHAR(10) UNIQUE,
    Name NVARCHAR(100),
    Contact NVARCHAR(20),
    Username NVARCHAR(100),
    Password NVARCHAR(100),
    IsActive BIT
);

CREATE TABLE Pharmacists (
    PharmacistId INT PRIMARY KEY IDENTITY,
    PhaEmpId NVARCHAR(10) UNIQUE,
    Name NVARCHAR(100),
    Contact NVARCHAR(20),
    Username NVARCHAR(100),
    Password NVARCHAR(100),
    IsActive BIT
);

CREATE TABLE Lab (
    LabId INT PRIMARY KEY IDENTITY,
    LabEmpId NVARCHAR(10) UNIQUE,
    Name NVARCHAR(100),
    Contact NVARCHAR(20),
    Username NVARCHAR(100),
    Password NVARCHAR(100),
    IsActive BIT
);

SELECT * FROM Doctors;
SELECT * FROM Users;
SELECT * FROM Roles;
TRUNCATE TABLE Users;
```