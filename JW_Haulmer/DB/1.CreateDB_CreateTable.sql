-- Script: CreateDatabaseAndTable.sql
-- Crea la base de datos y la tabla para la API JW_Haulmer

-- 1️⃣ Crear la base de datos
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CardTransaction')
BEGIN
    CREATE DATABASE CardTransaction;
    PRINT 'Base de datos CardTransaction creada.';
END
ELSE
BEGIN
    PRINT 'La base de datos CardTransaction ya existe.';
END
GO

-- 2️⃣ Usar la base de datos
USE CardTransaction;
GO

-- 3️⃣ Crear la tabla Transactions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Transactions')
BEGIN
    CREATE TABLE [dbo].[Transactions](
        [TransactionId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [MerchantId] NVARCHAR(20) NOT NULL,
        [PanMasked] NVARCHAR(25) NOT NULL,          
        [Expiry] CHAR(5) NOT NULL,                  
        [Amount] DECIMAL(18,2) NOT NULL,
        [Currency] CHAR(3) NOT NULL,
        [Status] NVARCHAR(20) NOT NULL,             
        [IsoCode] CHAR(2) NOT NULL,                 
        [AuthorizationCode] NVARCHAR(10) NULL,      
        [CreatedAt] DATETIME NOT NULL DEFAULT GETUTCDATE()
    );
    PRINT 'Tabla Transactions creada.';
END
ELSE
BEGIN
    PRINT 'La tabla Transactions ya existe.';
END
GO
