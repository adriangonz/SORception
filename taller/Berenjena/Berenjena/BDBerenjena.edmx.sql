
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/11/2013 17:38:35
-- Generated from EDMX file: C:\Users\Ruben\Documents\cocainum\taller\Berenjena\Berenjena\BDBerenjena.edmx
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
    [id] int IDENTITY(1,1) NOT NULL,
    [timeStamp] datetime  NOT NULL,
    [state] nvarchar(max)  NOT NULL,
    [token] nvarchar(max)  NOT NULL
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

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------