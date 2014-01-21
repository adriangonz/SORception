
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/21/2014 12:50:15
-- Generated from EDMX file: C:\Alex\Desarrollo\SORception\taller\Berenjena\Eggplant\BDBerenjena.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-Eggplant-20131216073914];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_SolicitudLineaSolicitud]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineaSolicitudSet] DROP CONSTRAINT [FK_SolicitudLineaSolicitud];
GO
IF OBJECT_ID(N'[dbo].[FK_PedidoLineaPedido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineaPedidoSet] DROP CONSTRAINT [FK_PedidoLineaPedido];
GO
IF OBJECT_ID(N'[dbo].[FK_PedidoSolicitud]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PedidoSet] DROP CONSTRAINT [FK_PedidoSolicitud];
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
IF OBJECT_ID(N'[dbo].[PedidoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PedidoSet];
GO
IF OBJECT_ID(N'[dbo].[LineaPedidoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineaPedidoSet];
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
    [status] nvarchar(max)  NOT NULL,
    [user_id] nvarchar(max)  NOT NULL
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

-- Creating table 'PedidoSet'
CREATE TABLE [dbo].[PedidoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [timeStamp] datetime  NOT NULL,
    [Solicitud_Id] int  NOT NULL
);
GO

-- Creating table 'LineaPedidoSet'
CREATE TABLE [dbo].[LineaPedidoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [linea_oferta_id] int  NOT NULL,
    [quantity] int  NOT NULL,
    [price] decimal(18,0)  NOT NULL,
    [PedidoId] int  NOT NULL,
    [sg_id] int  NOT NULL,
    [state] nvarchar(max)  NOT NULL
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

-- Creating primary key on [Id] in table 'PedidoSet'
ALTER TABLE [dbo].[PedidoSet]
ADD CONSTRAINT [PK_PedidoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineaPedidoSet'
ALTER TABLE [dbo].[LineaPedidoSet]
ADD CONSTRAINT [PK_LineaPedidoSet]
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

-- Creating foreign key on [PedidoId] in table 'LineaPedidoSet'
ALTER TABLE [dbo].[LineaPedidoSet]
ADD CONSTRAINT [FK_PedidoLineaPedido]
    FOREIGN KEY ([PedidoId])
    REFERENCES [dbo].[PedidoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PedidoLineaPedido'
CREATE INDEX [IX_FK_PedidoLineaPedido]
ON [dbo].[LineaPedidoSet]
    ([PedidoId]);
GO

-- Creating foreign key on [Solicitud_Id] in table 'PedidoSet'
ALTER TABLE [dbo].[PedidoSet]
ADD CONSTRAINT [FK_PedidoSolicitud]
    FOREIGN KEY ([Solicitud_Id])
    REFERENCES [dbo].[SolicitudSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PedidoSolicitud'
CREATE INDEX [IX_FK_PedidoSolicitud]
ON [dbo].[PedidoSet]
    ([Solicitud_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------