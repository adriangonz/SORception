
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/10/2013 13:51:47
-- Generated from EDMX file: C:\Users\Ruben\Documents\sorception\taller\Berenjena\Berenjena\BDBerenjena.edmx
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


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[TokensSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TokensSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'TokensSet'
CREATE TABLE [dbo].[TokensSet] (
    [token] int  NOT NULL,
    [TimeStamp] datetime  NOT NULL,
    [state] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [token] in table 'TokensSet'
ALTER TABLE [dbo].[TokensSet]
ADD CONSTRAINT [PK_TokensSet]
    PRIMARY KEY CLUSTERED ([token] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------