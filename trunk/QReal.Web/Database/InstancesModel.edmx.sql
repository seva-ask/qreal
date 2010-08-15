
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 08/16/2010 03:16:27
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
IF OBJECT_ID(N'[dbo].[FK_RootInstanceEdgeInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_EdgeInstance] DROP CONSTRAINT [FK_RootInstanceEdgeInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_EdgesFromNode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_EdgeInstance] DROP CONSTRAINT [FK_EdgesFromNode];
GO
IF OBJECT_ID(N'[dbo].[FK_EdgesToNode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_EdgeInstance] DROP CONSTRAINT [FK_EdgesToNode];
GO
IF OBJECT_ID(N'[dbo].[FK_NodeParents]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_NodeInstance] DROP CONSTRAINT [FK_NodeParents];
GO
IF OBJECT_ID(N'[dbo].[FK_ParentableInstance_inherits_GraphicInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_ParentableInstance] DROP CONSTRAINT [FK_ParentableInstance_inherits_GraphicInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_RootInstance_inherits_ParentableInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_RootInstance] DROP CONSTRAINT [FK_RootInstance_inherits_ParentableInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_EdgeInstance_inherits_GraphicInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_EdgeInstance] DROP CONSTRAINT [FK_EdgeInstance_inherits_GraphicInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_NodeInstance_inherits_ParentableInstance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GraphicInstances_NodeInstance] DROP CONSTRAINT [FK_NodeInstance_inherits_ParentableInstance];
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
IF OBJECT_ID(N'[dbo].[GraphicInstances_ParentableInstance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GraphicInstances_ParentableInstance];
GO
IF OBJECT_ID(N'[dbo].[GraphicInstances_RootInstance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GraphicInstances_RootInstance];
GO
IF OBJECT_ID(N'[dbo].[GraphicInstances_EdgeInstance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GraphicInstances_EdgeInstance];
GO
IF OBJECT_ID(N'[dbo].[GraphicInstances_NodeInstance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GraphicInstances_NodeInstance];
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

-- Creating table 'GraphicInstances_ParentableInstance'
CREATE TABLE [dbo].[GraphicInstances_ParentableInstance] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'GraphicInstances_RootInstance'
CREATE TABLE [dbo].[GraphicInstances_RootInstance] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'GraphicInstances_EdgeInstance'
CREATE TABLE [dbo].[GraphicInstances_EdgeInstance] (
    [ParentId] int  NOT NULL,
    [NodeFromId] int  NULL,
    [NodeToId] int  NULL,
    [PortFrom] float  NULL,
    [PortTo] float  NULL,
    [X] float  NOT NULL,
    [Y] float  NOT NULL,
    [Width] float  NOT NULL,
    [Height] float  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'GraphicInstances_NodeInstance'
CREATE TABLE [dbo].[GraphicInstances_NodeInstance] (
    [ParentId] int  NULL,
    [X] float  NOT NULL,
    [Y] float  NOT NULL,
    [Width] float  NOT NULL,
    [Height] float  NOT NULL,
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

-- Creating primary key on [Id] in table 'GraphicInstances_ParentableInstance'
ALTER TABLE [dbo].[GraphicInstances_ParentableInstance]
ADD CONSTRAINT [PK_GraphicInstances_ParentableInstance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GraphicInstances_RootInstance'
ALTER TABLE [dbo].[GraphicInstances_RootInstance]
ADD CONSTRAINT [PK_GraphicInstances_RootInstance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GraphicInstances_EdgeInstance'
ALTER TABLE [dbo].[GraphicInstances_EdgeInstance]
ADD CONSTRAINT [PK_GraphicInstances_EdgeInstance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GraphicInstances_NodeInstance'
ALTER TABLE [dbo].[GraphicInstances_NodeInstance]
ADD CONSTRAINT [PK_GraphicInstances_NodeInstance]
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

-- Creating foreign key on [ParentId] in table 'GraphicInstances_EdgeInstance'
ALTER TABLE [dbo].[GraphicInstances_EdgeInstance]
ADD CONSTRAINT [FK_RootInstanceEdgeInstance]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[GraphicInstances_RootInstance]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RootInstanceEdgeInstance'
CREATE INDEX [IX_FK_RootInstanceEdgeInstance]
ON [dbo].[GraphicInstances_EdgeInstance]
    ([ParentId]);
GO

-- Creating foreign key on [NodeFromId] in table 'GraphicInstances_EdgeInstance'
ALTER TABLE [dbo].[GraphicInstances_EdgeInstance]
ADD CONSTRAINT [FK_EdgesFromNode]
    FOREIGN KEY ([NodeFromId])
    REFERENCES [dbo].[GraphicInstances_NodeInstance]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EdgesFromNode'
CREATE INDEX [IX_FK_EdgesFromNode]
ON [dbo].[GraphicInstances_EdgeInstance]
    ([NodeFromId]);
GO

-- Creating foreign key on [NodeToId] in table 'GraphicInstances_EdgeInstance'
ALTER TABLE [dbo].[GraphicInstances_EdgeInstance]
ADD CONSTRAINT [FK_EdgesToNode]
    FOREIGN KEY ([NodeToId])
    REFERENCES [dbo].[GraphicInstances_NodeInstance]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EdgesToNode'
CREATE INDEX [IX_FK_EdgesToNode]
ON [dbo].[GraphicInstances_EdgeInstance]
    ([NodeToId]);
GO

-- Creating foreign key on [ParentId] in table 'GraphicInstances_NodeInstance'
ALTER TABLE [dbo].[GraphicInstances_NodeInstance]
ADD CONSTRAINT [FK_NodeParents]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[GraphicInstances_ParentableInstance]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_NodeParents'
CREATE INDEX [IX_FK_NodeParents]
ON [dbo].[GraphicInstances_NodeInstance]
    ([ParentId]);
GO

-- Creating foreign key on [Id] in table 'GraphicInstances_ParentableInstance'
ALTER TABLE [dbo].[GraphicInstances_ParentableInstance]
ADD CONSTRAINT [FK_ParentableInstance_inherits_GraphicInstance]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[GraphicInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'GraphicInstances_RootInstance'
ALTER TABLE [dbo].[GraphicInstances_RootInstance]
ADD CONSTRAINT [FK_RootInstance_inherits_ParentableInstance]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[GraphicInstances_ParentableInstance]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'GraphicInstances_EdgeInstance'
ALTER TABLE [dbo].[GraphicInstances_EdgeInstance]
ADD CONSTRAINT [FK_EdgeInstance_inherits_GraphicInstance]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[GraphicInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'GraphicInstances_NodeInstance'
ALTER TABLE [dbo].[GraphicInstances_NodeInstance]
ADD CONSTRAINT [FK_NodeInstance_inherits_ParentableInstance]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[GraphicInstances_ParentableInstance]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------