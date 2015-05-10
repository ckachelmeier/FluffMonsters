
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/09/2015 22:08:40
-- Generated from EDMX file: C:\Users\Cade\Documents\Visual Studio 2012\Projects\FluffMonsters\FluffMonsters.Models\FluffMonster.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [FluffMonsters];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CustomerPet_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerPet] DROP CONSTRAINT [FK_CustomerPet_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerPet_Pet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerPet] DROP CONSTRAINT [FK_CustomerPet_Pet];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerAddress]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Addresses] DROP CONSTRAINT [FK_CustomerAddress];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerPhoneNumber]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PhoneNumbers] DROP CONSTRAINT [FK_CustomerPhoneNumber];
GO
IF OBJECT_ID(N'[dbo].[FK_PetVisit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Visits] DROP CONSTRAINT [FK_PetVisit];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[Pets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pets];
GO
IF OBJECT_ID(N'[dbo].[Addresses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Addresses];
GO
IF OBJECT_ID(N'[dbo].[PhoneNumbers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhoneNumbers];
GO
IF OBJECT_ID(N'[dbo].[Visits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Visits];
GO
IF OBJECT_ID(N'[dbo].[CustomerPet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerPet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ContactMethod] nvarchar(max)  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [Address_Id] int  NULL
);
GO

-- Creating table 'Pets'
CREATE TABLE [dbo].[Pets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Breed] nvarchar(max)  NOT NULL,
    [Species] nvarchar(max)  NOT NULL,
    [FavoriteToy] nvarchar(max)  NOT NULL,
    [WeightInKilograms] float  NOT NULL,
    [HeightInMeters] float  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [Color] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Addresses'
CREATE TABLE [dbo].[Addresses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AddressLine1] nvarchar(max)  NOT NULL,
    [AddressLine2] nvarchar(max)  NOT NULL,
    [District] nvarchar(max)  NOT NULL,
    [PostalCode] nvarchar(max)  NOT NULL,
    [Country] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PhoneNumbers'
CREATE TABLE [dbo].[PhoneNumbers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PhoneNumberType] nvarchar(max)  NOT NULL,
    [Number] nvarchar(max)  NOT NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'Visits'
CREATE TABLE [dbo].[Visits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CheckInDate] datetime  NOT NULL,
    [CheckOutDate] datetime  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [PetId] int  NOT NULL
);
GO

-- Creating table 'CustomerPet'
CREATE TABLE [dbo].[CustomerPet] (
    [Owners_Id] int  NOT NULL,
    [Pets_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pets'
ALTER TABLE [dbo].[Pets]
ADD CONSTRAINT [PK_Pets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [PK_Addresses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PhoneNumbers'
ALTER TABLE [dbo].[PhoneNumbers]
ADD CONSTRAINT [PK_PhoneNumbers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Visits'
ALTER TABLE [dbo].[Visits]
ADD CONSTRAINT [PK_Visits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Owners_Id], [Pets_Id] in table 'CustomerPet'
ALTER TABLE [dbo].[CustomerPet]
ADD CONSTRAINT [PK_CustomerPet]
    PRIMARY KEY NONCLUSTERED ([Owners_Id], [Pets_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Owners_Id] in table 'CustomerPet'
ALTER TABLE [dbo].[CustomerPet]
ADD CONSTRAINT [FK_CustomerPet_Customer]
    FOREIGN KEY ([Owners_Id])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Pets_Id] in table 'CustomerPet'
ALTER TABLE [dbo].[CustomerPet]
ADD CONSTRAINT [FK_CustomerPet_Pet]
    FOREIGN KEY ([Pets_Id])
    REFERENCES [dbo].[Pets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerPet_Pet'
CREATE INDEX [IX_FK_CustomerPet_Pet]
ON [dbo].[CustomerPet]
    ([Pets_Id]);
GO

-- Creating foreign key on [Address_Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_CustomerAddress]
    FOREIGN KEY ([Address_Id])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerAddress'
CREATE INDEX [IX_FK_CustomerAddress]
ON [dbo].[Customers]
    ([Address_Id]);
GO

-- Creating foreign key on [CustomerId] in table 'PhoneNumbers'
ALTER TABLE [dbo].[PhoneNumbers]
ADD CONSTRAINT [FK_CustomerPhoneNumber]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerPhoneNumber'
CREATE INDEX [IX_FK_CustomerPhoneNumber]
ON [dbo].[PhoneNumbers]
    ([CustomerId]);
GO

-- Creating foreign key on [PetId] in table 'Visits'
ALTER TABLE [dbo].[Visits]
ADD CONSTRAINT [FK_PetVisit]
    FOREIGN KEY ([PetId])
    REFERENCES [dbo].[Pets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PetVisit'
CREATE INDEX [IX_FK_PetVisit]
ON [dbo].[Visits]
    ([PetId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------