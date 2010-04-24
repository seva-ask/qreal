
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/24/2010 20:35:29
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
    ALTER TABLE [dbo].[EdgeInstances] DROP CONSTRAINT [FK_RootInstanceEdgeInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_EdgesFromNode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EdgeInstances] DROP CONSTRAINT [FK_EdgesFromNode];
GO
IF OBJECT_ID(N'[dbo].[FK_EdgesToNode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EdgeInstances] DROP CONSTRAINT [FK_EdgesToNode];
GO
IF OBJECT_ID(N'[dbo].[FK_NodeParents]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NodeInstances] DROP CONSTRAINT [FK_NodeParents];
GO
IF OBJECT_ID(N'[dbo].[FK_GraphicInstanceParentableInstanceInheritance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParentableInstances] DROP CONSTRAINT [FK_GraphicInstanceParentableInstanceInheritance];
GO
IF OBJECT_ID(N'[dbo].[FK_GraphicInstanceEdgeInstanceInheritance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EdgeInstances] DROP CONSTRAINT [FK_GraphicInstanceEdgeInstanceInheritance];
GO
IF OBJECT_ID(N'[dbo].[FK_ParentableInstanceRootInstanceInheritance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RootInstances] DROP CONSTRAINT [FK_ParentableInstanceRootInstanceInheritance];
GO
IF OBJECT_ID(N'[dbo].[FK_ParentableInstanceNodeInstanceInheritance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NodeInstances] DROP CONSTRAINT [FK_ParentableInstanceNodeInstanceInheritance];
GO
IF OBJECT_ID(N'[dbo].[FK_NodeInstanceGeometryInformation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NodeInstances] DROP CONSTRAINT [FK_NodeInstanceGeometryInformation];
GO
IF OBJECT_ID(N'[dbo].[FK_EdgeInstanceGeometryInformation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EdgeInstances] DROP CONSTRAINT [FK_EdgeInstanceGeometryInformation];
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
IF OBJECT_ID(N'[dbo].[ParentableInstances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParentableInstances];
GO
IF OBJECT_ID(N'[dbo].[RootInstances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RootInstances];
GO
IF OBJECT_ID(N'[dbo].[NodeInstances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NodeInstances];
GO
IF OBJECT_ID(N'[dbo].[EdgeInstances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EdgeInstances];
GO
IF OBJECT_ID(N'[dbo].[GeometryInformations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GeometryInformations];
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

-- Creating table 'ParentableInstances'
CREATE TABLE [dbo].[ParentableInstances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InheritanceId] int  NOT NULL
);
GO

-- Creating table 'RootInstances'
CREATE TABLE [dbo].[RootInstances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InheritanceId] int  NOT NULL
);
GO

-- Creating table 'NodeInstances'
CREATE TABLE [dbo].[NodeInstances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentId] int  NULL,
    [InheritanceId] int  NOT NULL,
    [GeometryInformationId] int  NOT NULL
);
GO

-- Creating table 'EdgeInstances'
CREATE TABLE [dbo].[EdgeInstances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentId] int  NOT NULL,
    [NodeFromId] int  NULL,
    [NodeToId] int  NULL,
    [PortFrom] float  NULL,
    [PortTo] float  NULL,
    [InheritanceId] int  NOT NULL,
    [GeometryInformationId] int  NOT NULL
);
GO

-- Creating table 'GeometryInformations'
CREATE TABLE [dbo].[GeometryInformations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [X] float  NOT NULL,
    [Y] float  NOT NULL,
    [Width] float  NOT NULL,
    [Height] float  NOT NULL
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

-- Creating primary key on [Id] in table 'ParentableInstances'
ALTER TABLE [dbo].[ParentableInstances]
ADD CONSTRAINT [PK_ParentableInstances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RootInstances'
ALTER TABLE [dbo].[RootInstances]
ADD CONSTRAINT [PK_RootInstances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NodeInstances'
ALTER TABLE [dbo].[NodeInstances]
ADD CONSTRAINT [PK_NodeInstances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EdgeInstances'
ALTER TABLE [dbo].[EdgeInstances]
ADD CONSTRAINT [PK_EdgeInstances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GeometryInformations'
ALTER TABLE [dbo].[GeometryInformations]
ADD CONSTRAINT [PK_GeometryInformations]
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

-- Creating foreign key on [ParentId] in table 'EdgeInstances'
ALTER TABLE [dbo].[EdgeInstances]
ADD CONSTRAINT [FK_RootInstanceEdgeInstance]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[RootInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RootInstanceEdgeInstance'
CREATE INDEX [IX_FK_RootInstanceEdgeInstance]
ON [dbo].[EdgeInstances]
    ([ParentId]);
GO

-- Creating foreign key on [NodeFromId] in table 'EdgeInstances'
ALTER TABLE [dbo].[EdgeInstances]
ADD CONSTRAINT [FK_EdgesFromNode]
    FOREIGN KEY ([NodeFromId])
    REFERENCES [dbo].[NodeInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EdgesFromNode'
CREATE INDEX [IX_FK_EdgesFromNode]
ON [dbo].[EdgeInstances]
    ([NodeFromId]);
GO

-- Creating foreign key on [NodeToId] in table 'EdgeInstances'
ALTER TABLE [dbo].[EdgeInstances]
ADD CONSTRAINT [FK_EdgesToNode]
    FOREIGN KEY ([NodeToId])
    REFERENCES [dbo].[NodeInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EdgesToNode'
CREATE INDEX [IX_FK_EdgesToNode]
ON [dbo].[EdgeInstances]
    ([NodeToId]);
GO

-- Creating foreign key on [ParentId] in table 'NodeInstances'
ALTER TABLE [dbo].[NodeInstances]
ADD CONSTRAINT [FK_NodeParents]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[ParentableInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_NodeParents'
CREATE INDEX [IX_FK_NodeParents]
ON [dbo].[NodeInstances]
    ([ParentId]);
GO

-- Creating foreign key on [InheritanceId] in table 'ParentableInstances'
ALTER TABLE [dbo].[ParentableInstances]
ADD CONSTRAINT [FK_GraphicInstanceParentableInstanceInheritance]
    FOREIGN KEY ([InheritanceId])
    REFERENCES [dbo].[GraphicInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GraphicInstanceParentableInstanceInheritance'
CREATE INDEX [IX_FK_GraphicInstanceParentableInstanceInheritance]
ON [dbo].[ParentableInstances]
    ([InheritanceId]);
GO

-- Creating foreign key on [InheritanceId] in table 'EdgeInstances'
ALTER TABLE [dbo].[EdgeInstances]
ADD CONSTRAINT [FK_GraphicInstanceEdgeInstanceInheritance]
    FOREIGN KEY ([InheritanceId])
    REFERENCES [dbo].[GraphicInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GraphicInstanceEdgeInstanceInheritance'
CREATE INDEX [IX_FK_GraphicInstanceEdgeInstanceInheritance]
ON [dbo].[EdgeInstances]
    ([InheritanceId]);
GO

-- Creating foreign key on [InheritanceId] in table 'RootInstances'
ALTER TABLE [dbo].[RootInstances]
ADD CONSTRAINT [FK_ParentableInstanceRootInstanceInheritance]
    FOREIGN KEY ([InheritanceId])
    REFERENCES [dbo].[ParentableInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ParentableInstanceRootInstanceInheritance'
CREATE INDEX [IX_FK_ParentableInstanceRootInstanceInheritance]
ON [dbo].[RootInstances]
    ([InheritanceId]);
GO

-- Creating foreign key on [InheritanceId] in table 'NodeInstances'
ALTER TABLE [dbo].[NodeInstances]
ADD CONSTRAINT [FK_ParentableInstanceNodeInstanceInheritance]
    FOREIGN KEY ([InheritanceId])
    REFERENCES [dbo].[ParentableInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ParentableInstanceNodeInstanceInheritance'
CREATE INDEX [IX_FK_ParentableInstanceNodeInstanceInheritance]
ON [dbo].[NodeInstances]
    ([InheritanceId]);
GO

-- Creating foreign key on [GeometryInformationId] in table 'NodeInstances'
ALTER TABLE [dbo].[NodeInstances]
ADD CONSTRAINT [FK_NodeInstanceGeometryInformation]
    FOREIGN KEY ([GeometryInformationId])
    REFERENCES [dbo].[GeometryInformations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_NodeInstanceGeometryInformation'
CREATE INDEX [IX_FK_NodeInstanceGeometryInformation]
ON [dbo].[NodeInstances]
    ([GeometryInformationId]);
GO

-- Creating foreign key on [GeometryInformationId] in table 'EdgeInstances'
ALTER TABLE [dbo].[EdgeInstances]
ADD CONSTRAINT [FK_EdgeInstanceGeometryInformation]
    FOREIGN KEY ([GeometryInformationId])
    REFERENCES [dbo].[GeometryInformations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EdgeInstanceGeometryInformation'
CREATE INDEX [IX_FK_EdgeInstanceGeometryInformation]
ON [dbo].[EdgeInstances]
    ([GeometryInformationId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------