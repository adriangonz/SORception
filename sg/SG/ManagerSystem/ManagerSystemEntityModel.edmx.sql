
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/17/2013 16:48:41
-- Generated from EDMX file: C:\Users\Ruben\Documents\sorception\sg\SG\ManagerSystem\ManagerSystemEntityModel.edmx
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

IF OBJECT_ID(N'[dbo].[FK_OfertaLineaPedidoOferta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineaPedidoOfertaConjunto] DROP CONSTRAINT [FK_OfertaLineaPedidoOferta];
GO
IF OBJECT_ID(N'[dbo].[FK_DesguaceOferta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OfertaConjunto] DROP CONSTRAINT [FK_DesguaceOferta];
GO
IF OBJECT_ID(N'[dbo].[FK_TallerOferta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OfertaConjunto] DROP CONSTRAINT [FK_TallerOferta];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DesguaceConjunto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DesguaceConjunto];
GO
IF OBJECT_ID(N'[dbo].[OfertaConjunto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OfertaConjunto];
GO
IF OBJECT_ID(N'[dbo].[LineaPedidoOfertaConjunto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineaPedidoOfertaConjunto];
GO
IF OBJECT_ID(N'[dbo].[Tallers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tallers];
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

-- Creating table 'OfertaConjunto'
CREATE TABLE [dbo].[OfertaConjunto] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Desguace_id] int  NOT NULL,
    [id_en_desguace] int  NOT NULL,
    [date] datetime  NOT NULL,
    [state] nvarchar(max)  NOT NULL,
    [Desguace_id1] int  NULL,
    [TallerId] int  NOT NULL,
    [TallerId1] int  NULL
);
GO

-- Creating table 'LineaPedidoOfertaConjunto'
CREATE TABLE [dbo].[LineaPedidoOfertaConjunto] (
    [id] int IDENTITY(1,1) NOT NULL,
    [id_en_desguace] int  NOT NULL,
    [price] int  NOT NULL,
    [quantity] int  NOT NULL,
    [Oferta_id] int  NOT NULL
);
GO

-- Creating table 'Tallers'
CREATE TABLE [dbo].[Tallers] (
    [Id] int IDENTITY(1,1) NOT NULL,
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

-- Creating primary key on [id] in table 'OfertaConjunto'
ALTER TABLE [dbo].[OfertaConjunto]
ADD CONSTRAINT [PK_OfertaConjunto]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'LineaPedidoOfertaConjunto'
ALTER TABLE [dbo].[LineaPedidoOfertaConjunto]
ADD CONSTRAINT [PK_LineaPedidoOfertaConjunto]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'Tallers'
ALTER TABLE [dbo].[Tallers]
ADD CONSTRAINT [PK_Tallers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Oferta_id] in table 'LineaPedidoOfertaConjunto'
ALTER TABLE [dbo].[LineaPedidoOfertaConjunto]
ADD CONSTRAINT [FK_OfertaLineaPedidoOferta]
    FOREIGN KEY ([Oferta_id])
    REFERENCES [dbo].[OfertaConjunto]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OfertaLineaPedidoOferta'
CREATE INDEX [IX_FK_OfertaLineaPedidoOferta]
ON [dbo].[LineaPedidoOfertaConjunto]
    ([Oferta_id]);
GO

-- Creating foreign key on [Desguace_id1] in table 'OfertaConjunto'
ALTER TABLE [dbo].[OfertaConjunto]
ADD CONSTRAINT [FK_DesguaceOferta]
    FOREIGN KEY ([Desguace_id1])
    REFERENCES [dbo].[DesguaceConjunto]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DesguaceOferta'
CREATE INDEX [IX_FK_DesguaceOferta]
ON [dbo].[OfertaConjunto]
    ([Desguace_id1]);
GO

-- Creating foreign key on [TallerId1] in table 'OfertaConjunto'
ALTER TABLE [dbo].[OfertaConjunto]
ADD CONSTRAINT [FK_TallerOferta]
    FOREIGN KEY ([TallerId1])
    REFERENCES [dbo].[Tallers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TallerOferta'
CREATE INDEX [IX_FK_TallerOferta]
ON [dbo].[OfertaConjunto]
    ([TallerId1]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------