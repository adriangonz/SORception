
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/09/2014 18:10:08
-- Generated from EDMX file: C:\Users\Ruben\Documents\sorception\taller\Berenjena\Eggplant\BDBerenjena.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BDBerenjena];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_SolicitudLineaSolicitud]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineaSolicitudSet] DROP CONSTRAINT [FK_SolicitudLineaSolicitud];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[TokensSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TokensSet];
GO
IF OBJECT_ID(N'[dbo].[SolicitudSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SolicitudSet];
GO
IF OBJECT_ID(N'[dbo].[LineaSolicitudSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineaSolicitudSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'TokensSet'
CREATE TABLE [dbo].[TokensSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [timeStamp] datetime  NOT NULL,
    [state] nvarchar(max)  NOT NULL,
    [token] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SolicitudSet'
CREATE TABLE [dbo].[SolicitudSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [sg_id] int  NOT NULL,
    [timeStamp] datetime  NOT NULL,
    [status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'LineaSolicitudSet'
CREATE TABLE [dbo].[LineaSolicitudSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SolicitudId] int  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL,
    [cantidad] int  NOT NULL,
    [sg_id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'TokensSet'
ALTER TABLE [dbo].[TokensSet]
ADD CONSTRAINT [PK_TokensSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'SolicitudSet'
ALTER TABLE [dbo].[SolicitudSet]
ADD CONSTRAINT [PK_SolicitudSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineaSolicitudSet'
ALTER TABLE [dbo].[LineaSolicitudSet]
ADD CONSTRAINT [PK_LineaSolicitudSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SolicitudId] in table 'LineaSolicitudSet'
ALTER TABLE [dbo].[LineaSolicitudSet]
ADD CONSTRAINT [FK_SolicitudLineaSolicitud]
    FOREIGN KEY ([SolicitudId])
    REFERENCES [dbo].[SolicitudSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SolicitudLineaSolicitud'
CREATE INDEX [IX_FK_SolicitudLineaSolicitud]
ON [dbo].[LineaSolicitudSet]
    ([SolicitudId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------