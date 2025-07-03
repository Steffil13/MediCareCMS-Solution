Use Medicare_DB;
select * from Roles;

INSERT INTO Users (Username, Password, RoleId) VALUES 

( 'dr.john', 'doc123', 3)
GO
INSERT INTO Roles (RoleName) VALUES
('Admin'),
('Receptionist'),
('Doctor'),
('Pharmacist'),
('Lab');
GO

CREATE PROCEDURE sp_GetAppointmentsByDoctorAndDate
    @DoctorId INT,
    @Date DATE
AS
BEGIN
    SELECT 
        a.AppointmentId,
        a.DoctorId,
        a.PatientId,
        a.Date,
        a.Time,
        a.Token,
        a.IsConsulted,
        p.Name AS PatientName
    FROM Appointment a
    JOIN Patient p ON a.PatientId = p.PatientId
    WHERE a.DoctorId = @DoctorId AND a.Date = @Date
    ORDER BY a.Time;
END

CREATE PROCEDURE sp_GetAppointmentById
    @AppointmentId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT 
        a.*,
        p.Name AS PatientName
    FROM Appointment a
    JOIN Patient p ON a.PatientId = p.PatientId
    WHERE a.AppointmentId = @AppointmentId;
END


CREATE PROCEDURE sp_GetPatientSummary
    @PatientId NVARCHAR(50)
AS
BEGIN
    -- First result set: latest diagnosis
    SELECT TOP 1 pr.Diagnosis
    FROM Prescription pr
    JOIN Appointment a ON a.AppointmentId = pr.AppointmentId
    WHERE a.PatientId = @PatientId
    ORDER BY a.Date DESC;

    -- Second result set: associated medicines
    SELECT mi.MedicineName
    FROM Prescription pr
    JOIN Appointment a ON a.AppointmentId = pr.AppointmentId
    JOIN PrescribedMedicine pm ON pm.PrescriptionId = pr.PrescriptionId
    JOIN MedicineInventory mi ON mi.MedicineId = pm.MedicineId
    WHERE a.PatientId = @PatientId;
END

CREATE PROCEDURE sp_GetMedicineInventory
AS
BEGIN
    SELECT * FROM MedicineInventory;
END


CREATE PROCEDURE sp_SavePrescription
    @AppointmentId UNIQUEIDENTIFIER,
    @Symptoms NVARCHAR(500),
    @Diagnosis NVARCHAR(500)
AS
BEGIN
    UPDATE Appointment
    SET IsConsulted = 1
    WHERE AppointmentId = @AppointmentId;

    INSERT INTO Prescription (AppointmentId, Symptoms, Diagnosis)
    VALUES (@AppointmentId, @Symptoms, @Diagnosis);

    SELECT SCOPE_IDENTITY();
END


CREATE PROCEDURE sp_AddPrescriptionMedicine
    @PrescriptionId INT,
    @MedicineId INT,
    @Dosage NVARCHAR(50),
    @Duration NVARCHAR(50)
AS
BEGIN
    INSERT INTO PrescribedMedicine (PrescriptionId, MedicineId, Dosage, Duration)
    VALUES (@PrescriptionId, @MedicineId, @Dosage, @Duration);
END

CREATE PROCEDURE sp_GetDoctorSchedule
    @DoctorId INT
AS
BEGIN
    SELECT * FROM DoctorSchedule
    WHERE DoctorId = @DoctorId
    ORDER BY Date;
END

CREATE PROCEDURE sp_UpdateDoctorSchedule
    @DoctorId INT,
    @Date DATE,
    @IsAvailable BIT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM DoctorSchedule WHERE DoctorId = @DoctorId AND Date = @Date)
    BEGIN
        UPDATE DoctorSchedule
        SET IsAvailable = @IsAvailable
        WHERE DoctorId = @DoctorId AND Date = @Date;
    END
    ELSE
    BEGIN
        INSERT INTO DoctorSchedule (DoctorId, Date, IsAvailable)
        VALUES (@DoctorId, @Date, @IsAvailable);
    END
END


