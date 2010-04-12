
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/12/2010 18:56:39
-- Generated from EDMX file: C:\Projects\QReal\silverlight\svn\trunk\QReal.Web\Database\InstancesModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [QReal];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_LogicalInstanceProperties]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InstanceProperties] DROP CONSTRAINT [FK_LogicalInstanceProperties];
GO
IF OBJECT_ID(N'[dbo].[FK_GraphicToLogical]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances] DROP CONSTRAINT [FK_GraphicToLogical];
GO
IF OBJECT_ID(N'[dbo].[FK_GraphicParents]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_GraphicVisualizedInstance] DROP CONSTRAINT [FK_GraphicParents];
GO
IF OBJECT_ID(N'[dbo].[FK_LinksFromNode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_LinkInstance] DROP CONSTRAINT [FK_LinksFromNode];
GO
IF OBJECT_ID(N'[dbo].[FK_LinksToNode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_LinkInstance] DROP CONSTRAINT [FK_LinksToNode];
GO
IF OBJECT_ID(N'[dbo].[FK_GraphicVisualizedInstance_inherits_GraphicInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_GraphicVisualizedInstance] DROP CONSTRAINT [FK_GraphicVisualizedInstance_inherits_GraphicInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_NodeInstance_inherits_GraphicVisualizedInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_NodeInstance] DROP CONSTRAINT [FK_NodeInstance_inherits_GraphicVisualizedInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_LinkInstance_inherits_GraphicVisualizedInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_LinkInstance] DROP CONSTRAINT [FK_LinkInstance_inherits_GraphicVisualizedInstance];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[LogicalInstances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogicalInstances];
GO
IF OBJECT_ID(N'[dbo].[InstanceProperties]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InstanceProperties];
GO
IF OBJECT_ID(N'[dbo].[GraphicInstances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GraphicInstances];
GO
IF OBJECT_ID(N'[dbo].[GraphicInstances_GraphicVisualizedInstance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GraphicInstances_GraphicVisualizedInstance];
GO
IF OBJECT_ID(N'[dbo].[GraphicInstances_NodeInstance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GraphicInstances_NodeInstance];
GO
IF OBJECT_ID(N'[dbo].[GraphicInstances_LinkInstance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GraphicInstances_LinkInstance];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'LogicalInstances'
CREATE TABLE [dbo].[LogicalInstances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InstanceProperties'
CREATE TABLE [dbo].[InstanceProperties] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NULL,
    [LogicalInstanceId] int  NOT NULL
);
GO

-- Creating table 'GraphicInstances'
CREATE TABLE [dbo].[GraphicInstances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LogicalInstanceId] int  NOT NULL
);
GO

-- Creating table 'GraphicInstances_GraphicVisualizedInstance'
CREATE TABLE [dbo].[GraphicInstances_GraphicVisualizedInstance] (
    [X] float  NOT NULL,
    [Y] float  NOT NULL,
    [Width] float  NOT NULL,
    [Height] float  NOT NULL,
    [ParentId] int  NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'GraphicInstances_NodeInstance'
CREATE TABLE [dbo].[GraphicInstances_NodeInstance] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'GraphicInstances_LinkInstance'
CREATE TABLE [dbo].[GraphicInstances_LinkInstance] (
    [PortFrom] float  NULL,
    [PortTo] float  NULL,
    [NodeFromId] int  NULL,
    [NodeToId] int  NULL,
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'LogicalInstances'
ALTER TABLE [dbo].[LogicalInstances]
ADD CONSTRAINT [PK_LogicalInstances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InstanceProperties'
ALTER TABLE [dbo].[InstanceProperties]
ADD CONSTRAINT [PK_InstanceProperties]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GraphicInstances'
ALTER TABLE [dbo].[GraphicInstances]
ADD CONSTRAINT [PK_GraphicInstances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GraphicInstances_GraphicVisualizedInstance'
ALTER TABLE [dbo].[GraphicInstances_GraphicVisualizedInstance]
ADD CONSTRAINT [PK_GraphicInstances_GraphicVisualizedInstance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GraphicInstances_NodeInstance'
ALTER TABLE [dbo].[GraphicInstances_NodeInstance]
ADD CONSTRAINT [PK_GraphicInstances_NodeInstance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GraphicInstances_LinkInstance'
ALTER TABLE [dbo].[GraphicInstances_LinkInstance]
ADD CONSTRAINT [PK_GraphicInstances_LinkInstance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [LogicalInstanceId] in table 'InstanceProperties'
ALTER TABLE [dbo].[InstanceProperties]
ADD CONSTRAINT [FK_LogicalInstanceProperties]
    FOREIGN KEY ([LogicalInstanceId])
    REFERENCES [dbo].[LogicalInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LogicalInstanceProperties'
CREATE INDEX [IX_FK_LogicalInstanceProperties]
ON [dbo].[InstanceProperties]
    ([LogicalInstanceId]);
GO

-- Creating foreign key on [LogicalInstanceId] in table 'GraphicInstances'
ALTER TABLE [dbo].[GraphicInstances]
ADD CONSTRAINT [FK_GraphicToLogical]
    FOREIGN KEY ([LogicalInstanceId])
    REFERENCES [dbo].[LogicalInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GraphicToLogical'
CREATE INDEX [IX_FK_GraphicToLogical]
ON [dbo].[GraphicInstances]
    ([LogicalInstanceId]);
GO

-- Creating foreign key on [ParentId] in table 'GraphicInstances_GraphicVisualizedInstance'
ALTER TABLE [dbo].[GraphicInstances_GraphicVisualizedInstance]
ADD CONSTRAINT [FK_GraphicParents]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[GraphicInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GraphicParents'
CREATE INDEX [IX_FK_GraphicParents]
ON [dbo].[GraphicInstances_GraphicVisualizedInstance]
    ([ParentId]);
GO

-- Creating foreign key on [NodeFromId] in table 'GraphicInstances_LinkInstance'
ALTER TABLE [dbo].[GraphicInstances_LinkInstance]
ADD CONSTRAINT [FK_LinksFromNode]
    FOREIGN KEY ([NodeFromId])
    REFERENCES [dbo].[GraphicInstances_NodeInstance]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LinksFromNode'
CREATE INDEX [IX_FK_LinksFromNode]
ON [dbo].[GraphicInstances_LinkInstance]
    ([NodeFromId]);
GO

-- Creating foreign key on [NodeToId] in table 'GraphicInstances_LinkInstance'
ALTER TABLE [dbo].[GraphicInstances_LinkInstance]
ADD CONSTRAINT [FK_LinksToNode]
    FOREIGN KEY ([NodeToId])
    REFERENCES [dbo].[GraphicInstances_NodeInstance]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LinksToNode'
CREATE INDEX [IX_FK_LinksToNode]
ON [dbo].[GraphicInstances_LinkInstance]
    ([NodeToId]);
GO

-- Creating foreign key on [Id] in table 'GraphicInstances_GraphicVisualizedInstance'
ALTER TABLE [dbo].[GraphicInstances_GraphicVisualizedInstance]
ADD CONSTRAINT [FK_GraphicVisualizedInstance_inherits_GraphicInstance]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[GraphicInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'GraphicInstances_NodeInstance'
ALTER TABLE [dbo].[GraphicInstances_NodeInstance]
ADD CONSTRAINT [FK_NodeInstance_inherits_GraphicVisualizedInstance]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[GraphicInstances_GraphicVisualizedInstance]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'GraphicInstances_LinkInstance'
ALTER TABLE [dbo].[GraphicInstances_LinkInstance]
ADD CONSTRAINT [FK_LinkInstance_inherits_GraphicVisualizedInstance]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[GraphicInstances_GraphicVisualizedInstance]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------