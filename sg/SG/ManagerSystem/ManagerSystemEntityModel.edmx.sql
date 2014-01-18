
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/18/2014 20:33:30
-- Generated from EDMX file: C:\Users\marti_000\Documents\Proyectos\SORception\sg\SG\ManagerSystem\ManagerSystemEntityModel.edmx
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

IF OBJECT_ID(N'[dbo].[FK_DesguaceOferta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OfertaSet] DROP CONSTRAINT [FK_DesguaceOferta];
GO
IF OBJECT_ID(N'[dbo].[FK_TallerSolicitud]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SolicitudSet] DROP CONSTRAINT [FK_TallerSolicitud];
GO
IF OBJECT_ID(N'[dbo].[FK_OfertaLineaOferta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineaOfertaSet] DROP CONSTRAINT [FK_OfertaLineaOferta];
GO
IF OBJECT_ID(N'[dbo].[FK_SolicitudLineaSolicitud]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineasSolicitudSet] DROP CONSTRAINT [FK_SolicitudLineaSolicitud];
GO
IF OBJECT_ID(N'[dbo].[FK_DesguaceToken]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TokenSet] DROP CONSTRAINT [FK_DesguaceToken];
GO
IF OBJECT_ID(N'[dbo].[FK_TallerToken]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TokenSet] DROP CONSTRAINT [FK_TallerToken];
GO
IF OBJECT_ID(N'[dbo].[FK_LineaSolicitudLineaOferta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineaOfertaSet] DROP CONSTRAINT [FK_LineaSolicitudLineaOferta];
GO
IF OBJECT_ID(N'[dbo].[FK_SolicitudOferta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OfertaSet] DROP CONSTRAINT [FK_SolicitudOferta];
GO
IF OBJECT_ID(N'[dbo].[FK_LineaOfertaLineaOfertaSeleccionada]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineaOfertaSeleccionadaSet] DROP CONSTRAINT [FK_LineaOfertaLineaOfertaSeleccionada];
GO
IF OBJECT_ID(N'[dbo].[FK_LineaSolicitudFlag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Flags1] DROP CONSTRAINT [FK_LineaSolicitudFlag];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DesguaceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DesguaceSet];
GO
IF OBJECT_ID(N'[dbo].[OfertaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OfertaSet];
GO
IF OBJECT_ID(N'[dbo].[LineaOfertaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineaOfertaSet];
GO
IF OBJECT_ID(N'[dbo].[TallerSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TallerSet];
GO
IF OBJECT_ID(N'[dbo].[SolicitudSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SolicitudSet];
GO
IF OBJECT_ID(N'[dbo].[LineasSolicitudSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineasSolicitudSet];
GO
IF OBJECT_ID(N'[dbo].[LineaOfertaSeleccionadaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineaOfertaSeleccionadaSet];
GO
IF OBJECT_ID(N'[dbo].[TokenSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TokenSet];
GO
IF OBJECT_ID(N'[dbo].[Logs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Logs];
GO
IF OBJECT_ID(N'[dbo].[Flags1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Flags1];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DesguaceSet'
CREATE TABLE [dbo].[DesguaceSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [active] bit  NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [deleted] bit  NOT NULL
);
GO

-- Creating table 'OfertaSet'
CREATE TABLE [dbo].[OfertaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [id_en_desguace] int  NOT NULL,
    [date] datetime  NOT NULL,
    [status] nvarchar(max)  NOT NULL,
    [DesguaceId] int  NOT NULL,
    [SolicitudId] int  NOT NULL,
    [deleted] bit  NOT NULL
);
GO

-- Creating table 'LineaOfertaSet'
CREATE TABLE [dbo].[LineaOfertaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [id_en_desguace] int  NOT NULL,
    [price] float  NOT NULL,
    [quantity] int  NOT NULL,
    [OfertaId] int  NOT NULL,
    [notes] nvarchar(max)  NOT NULL,
    [LineaSolicitudId] int  NOT NULL,
    [status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TallerSet'
CREATE TABLE [dbo].[TallerSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [active] bit  NOT NULL,
    [name] nvarchar(max)  NOT NULL,
    [deleted] bit  NOT NULL
);
GO

-- Creating table 'SolicitudSet'
CREATE TABLE [dbo].[SolicitudSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [id_en_taller] int  NOT NULL,
    [date] datetime  NOT NULL,
    [status] nvarchar(max)  NOT NULL,
    [TallerId] int  NOT NULL,
    [deleted] bit  NOT NULL,
    [deadline] datetime  NOT NULL
);
GO

-- Creating table 'LineasSolicitudSet'
CREATE TABLE [dbo].[LineasSolicitudSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [quantity] int  NOT NULL,
    [id_en_taller] int  NOT NULL,
    [description] nvarchar(max)  NOT NULL,
    [SolicitudId] int  NOT NULL,
    [status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'LineaOfertaSeleccionadaSet'
CREATE TABLE [dbo].[LineaOfertaSeleccionadaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [quantity] int  NOT NULL,
    [LineaOferta_Id] int  NOT NULL
);
GO

-- Creating table 'TokenSet'
CREATE TABLE [dbo].[TokenSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [created] datetime  NOT NULL,
    [updated] datetime  NOT NULL,
    [token] nvarchar(max)  NOT NULL,
    [is_valid] bit  NOT NULL,
    [DesguaceId] int  NULL,
    [TallerId] int  NULL
);
GO

-- Creating table 'Logs'
CREATE TABLE [dbo].[Logs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [timestamp] datetime  NOT NULL,
    [message] nvarchar(max)  NOT NULL,
    [level] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FlagSet'
CREATE TABLE [dbo].[FlagSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [type] nvarchar(max)  NOT NULL,
    [price] int  NOT NULL,
    [LineaSolicitud_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'DesguaceSet'
ALTER TABLE [dbo].[DesguaceSet]
ADD CONSTRAINT [PK_DesguaceSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OfertaSet'
ALTER TABLE [dbo].[OfertaSet]
ADD CONSTRAINT [PK_OfertaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineaOfertaSet'
ALTER TABLE [dbo].[LineaOfertaSet]
ADD CONSTRAINT [PK_LineaOfertaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TallerSet'
ALTER TABLE [dbo].[TallerSet]
ADD CONSTRAINT [PK_TallerSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SolicitudSet'
ALTER TABLE [dbo].[SolicitudSet]
ADD CONSTRAINT [PK_SolicitudSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineasSolicitudSet'
ALTER TABLE [dbo].[LineasSolicitudSet]
ADD CONSTRAINT [PK_LineasSolicitudSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineaOfertaSeleccionadaSet'
ALTER TABLE [dbo].[LineaOfertaSeleccionadaSet]
ADD CONSTRAINT [PK_LineaOfertaSeleccionadaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TokenSet'
ALTER TABLE [dbo].[TokenSet]
ADD CONSTRAINT [PK_TokenSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Logs'
ALTER TABLE [dbo].[Logs]
ADD CONSTRAINT [PK_Logs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FlagSet'
ALTER TABLE [dbo].[FlagSet]
ADD CONSTRAINT [PK_FlagSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DesguaceId] in table 'OfertaSet'
ALTER TABLE [dbo].[OfertaSet]
ADD CONSTRAINT [FK_DesguaceOferta]
    FOREIGN KEY ([DesguaceId])
    REFERENCES [dbo].[DesguaceSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DesguaceOferta'
CREATE INDEX [IX_FK_DesguaceOferta]
ON [dbo].[OfertaSet]
    ([DesguaceId]);
GO

-- Creating foreign key on [TallerId] in table 'SolicitudSet'
ALTER TABLE [dbo].[SolicitudSet]
ADD CONSTRAINT [FK_TallerSolicitud]
    FOREIGN KEY ([TallerId])
    REFERENCES [dbo].[TallerSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TallerSolicitud'
CREATE INDEX [IX_FK_TallerSolicitud]
ON [dbo].[SolicitudSet]
    ([TallerId]);
GO

-- Creating foreign key on [OfertaId] in table 'LineaOfertaSet'
ALTER TABLE [dbo].[LineaOfertaSet]
ADD CONSTRAINT [FK_OfertaLineaOferta]
    FOREIGN KEY ([OfertaId])
    REFERENCES [dbo].[OfertaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OfertaLineaOferta'
CREATE INDEX [IX_FK_OfertaLineaOferta]
ON [dbo].[LineaOfertaSet]
    ([OfertaId]);
GO

-- Creating foreign key on [SolicitudId] in table 'LineasSolicitudSet'
ALTER TABLE [dbo].[LineasSolicitudSet]
ADD CONSTRAINT [FK_SolicitudLineaSolicitud]
    FOREIGN KEY ([SolicitudId])
    REFERENCES [dbo].[SolicitudSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SolicitudLineaSolicitud'
CREATE INDEX [IX_FK_SolicitudLineaSolicitud]
ON [dbo].[LineasSolicitudSet]
    ([SolicitudId]);
GO

-- Creating foreign key on [DesguaceId] in table 'TokenSet'
ALTER TABLE [dbo].[TokenSet]
ADD CONSTRAINT [FK_DesguaceToken]
    FOREIGN KEY ([DesguaceId])
    REFERENCES [dbo].[DesguaceSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DesguaceToken'
CREATE INDEX [IX_FK_DesguaceToken]
ON [dbo].[TokenSet]
    ([DesguaceId]);
GO

-- Creating foreign key on [TallerId] in table 'TokenSet'
ALTER TABLE [dbo].[TokenSet]
ADD CONSTRAINT [FK_TallerToken]
    FOREIGN KEY ([TallerId])
    REFERENCES [dbo].[TallerSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TallerToken'
CREATE INDEX [IX_FK_TallerToken]
ON [dbo].[TokenSet]
    ([TallerId]);
GO

-- Creating foreign key on [LineaSolicitudId] in table 'LineaOfertaSet'
ALTER TABLE [dbo].[LineaOfertaSet]
ADD CONSTRAINT [FK_LineaSolicitudLineaOferta]
    FOREIGN KEY ([LineaSolicitudId])
    REFERENCES [dbo].[LineasSolicitudSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LineaSolicitudLineaOferta'
CREATE INDEX [IX_FK_LineaSolicitudLineaOferta]
ON [dbo].[LineaOfertaSet]
    ([LineaSolicitudId]);
GO

-- Creating foreign key on [SolicitudId] in table 'OfertaSet'
ALTER TABLE [dbo].[OfertaSet]
ADD CONSTRAINT [FK_SolicitudOferta]
    FOREIGN KEY ([SolicitudId])
    REFERENCES [dbo].[SolicitudSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SolicitudOferta'
CREATE INDEX [IX_FK_SolicitudOferta]
ON [dbo].[OfertaSet]
    ([SolicitudId]);
GO

-- Creating foreign key on [LineaOferta_Id] in table 'LineaOfertaSeleccionadaSet'
ALTER TABLE [dbo].[LineaOfertaSeleccionadaSet]
ADD CONSTRAINT [FK_LineaOfertaLineaOfertaSeleccionada]
    FOREIGN KEY ([LineaOferta_Id])
    REFERENCES [dbo].[LineaOfertaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LineaOfertaLineaOfertaSeleccionada'
CREATE INDEX [IX_FK_LineaOfertaLineaOfertaSeleccionada]
ON [dbo].[LineaOfertaSeleccionadaSet]
    ([LineaOferta_Id]);
GO

-- Creating foreign key on [LineaSolicitud_Id] in table 'FlagSet'
ALTER TABLE [dbo].[FlagSet]
ADD CONSTRAINT [FK_LineaSolicitudFlag]
    FOREIGN KEY ([LineaSolicitud_Id])
    REFERENCES [dbo].[LineasSolicitudSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LineaSolicitudFlag'
CREATE INDEX [IX_FK_LineaSolicitudFlag]
ON [dbo].[FlagSet]
    ([LineaSolicitud_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------