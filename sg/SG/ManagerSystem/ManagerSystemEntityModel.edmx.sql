
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/26/2013 11:59:23
-- Generated from EDMX file: C:\Users\Ruben\Documents\sorception\sg\SG\ManagerSystem\ManagerSystemEntityModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [managerSystemDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TallerSolicitud]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Solicituds] DROP CONSTRAINT [FK_TallerSolicitud];
GO
IF OBJECT_ID(N'[dbo].[FK_DesguaceOferta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OfertaConjunto] DROP CONSTRAINT [FK_DesguaceOferta];
GO
IF OBJECT_ID(N'[dbo].[FK_OfertaLineaPedidoOferta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineaPedidoOfertaConjunto] DROP CONSTRAINT [FK_OfertaLineaPedidoOferta];
GO
IF OBJECT_ID(N'[dbo].[FK_SolicitudOferta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OfertaConjunto] DROP CONSTRAINT [FK_SolicitudOferta];
GO
IF OBJECT_ID(N'[dbo].[FK_SolicitudLineaSolicitud]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineaSolicituds] DROP CONSTRAINT [FK_SolicitudLineaSolicitud];
GO
IF OBJECT_ID(N'[dbo].[FK_LineaSolicitudLineaOfertaSeleccionada]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineasOfertaSeleccionada] DROP CONSTRAINT [FK_LineaSolicitudLineaOfertaSeleccionada];
GO
IF OBJECT_ID(N'[dbo].[FK_LineaOfertaSeleccionadaLineaOferta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineaPedidoOfertaConjunto] DROP CONSTRAINT [FK_LineaOfertaSeleccionadaLineaOferta];
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
IF OBJECT_ID(N'[dbo].[Solicituds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Solicituds];
GO
IF OBJECT_ID(N'[dbo].[LineaSolicituds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineaSolicituds];
GO
IF OBJECT_ID(N'[dbo].[LineasOfertaSeleccionada]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineasOfertaSeleccionada];
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
    [id_en_desguace] int  NOT NULL,
    [date] datetime  NOT NULL,
    [state] nvarchar(max)  NOT NULL,
    [Desguace_id] int  NOT NULL,
    [Solicitud_id] int  NOT NULL
);
GO

-- Creating table 'LineaPedidoOfertaConjunto'
CREATE TABLE [dbo].[LineaPedidoOfertaConjunto] (
    [id] int IDENTITY(1,1) NOT NULL,
    [id_en_desguace] int  NOT NULL,
    [price] int  NOT NULL,
    [quantity] int  NOT NULL,
    [Oferta_id] int  NOT NULL,
    [LineaOfertaSeleccionadaId] int  NOT NULL
);
GO

-- Creating table 'Tallers'
CREATE TABLE [dbo].[Tallers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [active] bit  NOT NULL,
    [name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Solicituds'
CREATE TABLE [dbo].[Solicituds] (
    [id] int IDENTITY(1,1) NOT NULL,
    [id_en_taller] int  NOT NULL,
    [date] datetime  NOT NULL,
    [state] nvarchar(max)  NOT NULL,
    [TallerId] int  NOT NULL
);
GO

-- Creating table 'LineaSolicituds'
CREATE TABLE [dbo].[LineaSolicituds] (
    [id] int IDENTITY(1,1) NOT NULL,
    [price] int  NOT NULL,
    [quantity] int  NOT NULL,
    [id_en_taller] nvarchar(max)  NOT NULL,
    [Solicitud_id] int  NOT NULL
);
GO

-- Creating table 'LineasOfertaSeleccionada'
CREATE TABLE [dbo].[LineasOfertaSeleccionada] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [quantity] nvarchar(max)  NOT NULL,
    [LineaSolicitud_id] int  NOT NULL
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

-- Creating primary key on [id] in table 'Solicituds'
ALTER TABLE [dbo].[Solicituds]
ADD CONSTRAINT [PK_Solicituds]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'LineaSolicituds'
ALTER TABLE [dbo].[LineaSolicituds]
ADD CONSTRAINT [PK_LineaSolicituds]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'LineasOfertaSeleccionada'
ALTER TABLE [dbo].[LineasOfertaSeleccionada]
ADD CONSTRAINT [PK_LineasOfertaSeleccionada]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [TallerId] in table 'Solicituds'
ALTER TABLE [dbo].[Solicituds]
ADD CONSTRAINT [FK_TallerSolicitud]
    FOREIGN KEY ([TallerId])
    REFERENCES [dbo].[Tallers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TallerSolicitud'
CREATE INDEX [IX_FK_TallerSolicitud]
ON [dbo].[Solicituds]
    ([TallerId]);
GO

-- Creating foreign key on [Desguace_id] in table 'OfertaConjunto'
ALTER TABLE [dbo].[OfertaConjunto]
ADD CONSTRAINT [FK_DesguaceOferta]
    FOREIGN KEY ([Desguace_id])
    REFERENCES [dbo].[DesguaceConjunto]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DesguaceOferta'
CREATE INDEX [IX_FK_DesguaceOferta]
ON [dbo].[OfertaConjunto]
    ([Desguace_id]);
GO

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

-- Creating foreign key on [Solicitud_id] in table 'OfertaConjunto'
ALTER TABLE [dbo].[OfertaConjunto]
ADD CONSTRAINT [FK_SolicitudOferta]
    FOREIGN KEY ([Solicitud_id])
    REFERENCES [dbo].[Solicituds]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SolicitudOferta'
CREATE INDEX [IX_FK_SolicitudOferta]
ON [dbo].[OfertaConjunto]
    ([Solicitud_id]);
GO

-- Creating foreign key on [Solicitud_id] in table 'LineaSolicituds'
ALTER TABLE [dbo].[LineaSolicituds]
ADD CONSTRAINT [FK_SolicitudLineaSolicitud]
    FOREIGN KEY ([Solicitud_id])
    REFERENCES [dbo].[Solicituds]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SolicitudLineaSolicitud'
CREATE INDEX [IX_FK_SolicitudLineaSolicitud]
ON [dbo].[LineaSolicituds]
    ([Solicitud_id]);
GO

-- Creating foreign key on [LineaSolicitud_id] in table 'LineasOfertaSeleccionada'
ALTER TABLE [dbo].[LineasOfertaSeleccionada]
ADD CONSTRAINT [FK_LineaSolicitudLineaOfertaSeleccionada]
    FOREIGN KEY ([LineaSolicitud_id])
    REFERENCES [dbo].[LineaSolicituds]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LineaSolicitudLineaOfertaSeleccionada'
CREATE INDEX [IX_FK_LineaSolicitudLineaOfertaSeleccionada]
ON [dbo].[LineasOfertaSeleccionada]
    ([LineaSolicitud_id]);
GO

-- Creating foreign key on [LineaOfertaSeleccionadaId] in table 'LineaPedidoOfertaConjunto'
ALTER TABLE [dbo].[LineaPedidoOfertaConjunto]
ADD CONSTRAINT [FK_LineaOfertaSeleccionadaLineaOferta]
    FOREIGN KEY ([LineaOfertaSeleccionadaId])
    REFERENCES [dbo].[LineasOfertaSeleccionada]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LineaOfertaSeleccionadaLineaOferta'
CREATE INDEX [IX_FK_LineaOfertaSeleccionadaLineaOferta]
ON [dbo].[LineaPedidoOfertaConjunto]
    ([LineaOfertaSeleccionadaId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------