
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/28/2013 19:54:04
-- Generated from EDMX file: C:\Users\marti_000\Documents\Proyectos\SORception\sg\SG\ManagerSystem\ManagerSystemEntityModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [managersystem];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DesguaceConjunto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DesguaceConjunto];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DesguaceConjunto'
CREATE TABLE [dbo].[DesguaceConjunto] (
    [id] int IDENTITY(1,1) NOT NULL,
    [active] bit  NOT NULL,
    [name] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'DesguaceConjunto'
ALTER TABLE [dbo].[DesguaceConjunto]
ADD CONSTRAINT [PK_DesguaceConjunto]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------