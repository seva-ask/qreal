
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/24/2010 03:38:30
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
    ALTER TABLE [dbo].[EdgeInstanceSet] DROP CONSTRAINT [FK_RootInstanceEdgeInstance];
GO
IF OBJECT_ID(N'[dbo].[FK_EdgesFromNode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EdgeInstanceSet] DROP CONSTRAINT [FK_EdgesFromNode];
GO
IF OBJECT_ID(N'[dbo].[FK_EdgesToNode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EdgeInstanceSet] DROP CONSTRAINT [FK_EdgesToNode];
GO
IF OBJECT_ID(N'[dbo].[FK_EdgeInstanceGeometryInformation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EdgeInstanceSet] DROP CONSTRAINT [FK_EdgeInstanceGeometryInformation];
GO
IF OBJECT_ID(N'[dbo].[FK_NodeInstanceGeometryInformation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NodeInstanceSet] DROP CONSTRAINT [FK_NodeInstanceGeometryInformation];
GO
IF OBJECT_ID(N'[dbo].[FK_NodeParents]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NodeInstanceSet] DROP CONSTRAINT [FK_NodeParents];
GO
IF OBJECT_ID(N'[dbo].[FK_GraphicInstanceParentableInstanceInheritance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParentableInstanceSet] DROP CONSTRAINT [FK_GraphicInstanceParentableInstanceInheritance];
GO
IF OBJECT_ID(N'[dbo].[FK_GraphicInstanceEdgeInstanceInheritance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EdgeInstanceSet] DROP CONSTRAINT [FK_GraphicInstanceEdgeInstanceInheritance];
GO
IF OBJECT_ID(N'[dbo].[FK_ParentableInstanceRootInstanceInheritance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RootInstanceSet] DROP CONSTRAINT [FK_ParentableInstanceRootInstanceInheritance];
GO
IF OBJECT_ID(N'[dbo].[FK_ParentableInstanceNodeInstanceInheritance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NodeInstanceSet] DROP CONSTRAINT [FK_ParentableInstanceNodeInstanceInheritance];
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
IF OBJECT_ID(N'[dbo].[ParentableInstanceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParentableInstanceSet];
GO
IF OBJECT_ID(N'[dbo].[RootInstanceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RootInstanceSet];
GO
IF OBJECT_ID(N'[dbo].[NodeInstanceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NodeInstanceSet];
GO
IF OBJECT_ID(N'[dbo].[EdgeInstanceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EdgeInstanceSet];
GO
IF OBJECT_ID(N'[dbo].[GeometryInformationSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GeometryInformationSet];
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

-- Creating table 'ParentableInstanceSet'
CREATE TABLE [dbo].[ParentableInstanceSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InheritanceId] int  NOT NULL
);
GO

-- Creating table 'RootInstanceSet'
CREATE TABLE [dbo].[RootInstanceSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InheritanceId] int  NOT NULL
);
GO

-- Creating table 'NodeInstanceSet'
CREATE TABLE [dbo].[NodeInstanceSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentId] int  NULL,
    [InheritanceId] int  NOT NULL
);
GO

-- Creating table 'EdgeInstanceSet'
CREATE TABLE [dbo].[EdgeInstanceSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentId] int  NOT NULL,
    [NodeFromId] int  NULL,
    [NodeToId] int  NULL,
    [PortFrom] float  NULL,
    [PortTo] float  NULL,
    [InheritanceId] int  NOT NULL
);
GO

-- Creating table 'GeometryInformationSet'
CREATE TABLE [dbo].[GeometryInformationSet] (
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

-- Creating primary key on [Id] in table 'ParentableInstanceSet'
ALTER TABLE [dbo].[ParentableInstanceSet]
ADD CONSTRAINT [PK_ParentableInstanceSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RootInstanceSet'
ALTER TABLE [dbo].[RootInstanceSet]
ADD CONSTRAINT [PK_RootInstanceSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NodeInstanceSet'
ALTER TABLE [dbo].[NodeInstanceSet]
ADD CONSTRAINT [PK_NodeInstanceSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EdgeInstanceSet'
ALTER TABLE [dbo].[EdgeInstanceSet]
ADD CONSTRAINT [PK_EdgeInstanceSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GeometryInformationSet'
ALTER TABLE [dbo].[GeometryInformationSet]
ADD CONSTRAINT [PK_GeometryInformationSet]
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

-- Creating foreign key on [ParentId] in table 'EdgeInstanceSet'
ALTER TABLE [dbo].[EdgeInstanceSet]
ADD CONSTRAINT [FK_RootInstanceEdgeInstance]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[RootInstanceSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RootInstanceEdgeInstance'
CREATE INDEX [IX_FK_RootInstanceEdgeInstance]
ON [dbo].[EdgeInstanceSet]
    ([ParentId]);
GO

-- Creating foreign key on [NodeFromId] in table 'EdgeInstanceSet'
ALTER TABLE [dbo].[EdgeInstanceSet]
ADD CONSTRAINT [FK_EdgesFromNode]
    FOREIGN KEY ([NodeFromId])
    REFERENCES [dbo].[NodeInstanceSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EdgesFromNode'
CREATE INDEX [IX_FK_EdgesFromNode]
ON [dbo].[EdgeInstanceSet]
    ([NodeFromId]);
GO

-- Creating foreign key on [NodeToId] in table 'EdgeInstanceSet'
ALTER TABLE [dbo].[EdgeInstanceSet]
ADD CONSTRAINT [FK_EdgesToNode]
    FOREIGN KEY ([NodeToId])
    REFERENCES [dbo].[NodeInstanceSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EdgesToNode'
CREATE INDEX [IX_FK_EdgesToNode]
ON [dbo].[EdgeInstanceSet]
    ([NodeToId]);
GO

-- Creating foreign key on [Id] in table 'EdgeInstanceSet'
ALTER TABLE [dbo].[EdgeInstanceSet]
ADD CONSTRAINT [FK_EdgeInstanceGeometryInformation]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[GeometryInformationSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'NodeInstanceSet'
ALTER TABLE [dbo].[NodeInstanceSet]
ADD CONSTRAINT [FK_NodeInstanceGeometryInformation]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[GeometryInformationSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ParentId] in table 'NodeInstanceSet'
ALTER TABLE [dbo].[NodeInstanceSet]
ADD CONSTRAINT [FK_NodeParents]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[ParentableInstanceSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_NodeParents'
CREATE INDEX [IX_FK_NodeParents]
ON [dbo].[NodeInstanceSet]
    ([ParentId]);
GO

-- Creating foreign key on [InheritanceId] in table 'ParentableInstanceSet'
ALTER TABLE [dbo].[ParentableInstanceSet]
ADD CONSTRAINT [FK_GraphicInstanceParentableInstanceInheritance]
    FOREIGN KEY ([InheritanceId])
    REFERENCES [dbo].[GraphicInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GraphicInstanceParentableInstanceInheritance'
CREATE INDEX [IX_FK_GraphicInstanceParentableInstanceInheritance]
ON [dbo].[ParentableInstanceSet]
    ([InheritanceId]);
GO

-- Creating foreign key on [InheritanceId] in table 'EdgeInstanceSet'
ALTER TABLE [dbo].[EdgeInstanceSet]
ADD CONSTRAINT [FK_GraphicInstanceEdgeInstanceInheritance]
    FOREIGN KEY ([InheritanceId])
    REFERENCES [dbo].[GraphicInstances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GraphicInstanceEdgeInstanceInheritance'
CREATE INDEX [IX_FK_GraphicInstanceEdgeInstanceInheritance]
ON [dbo].[EdgeInstanceSet]
    ([InheritanceId]);
GO

-- Creating foreign key on [InheritanceId] in table 'RootInstanceSet'
ALTER TABLE [dbo].[RootInstanceSet]
ADD CONSTRAINT [FK_ParentableInstanceRootInstanceInheritance]
    FOREIGN KEY ([InheritanceId])
    REFERENCES [dbo].[ParentableInstanceSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ParentableInstanceRootInstanceInheritance'
CREATE INDEX [IX_FK_ParentableInstanceRootInstanceInheritance]
ON [dbo].[RootInstanceSet]
    ([InheritanceId]);
GO

-- Creating foreign key on [InheritanceId] in table 'NodeInstanceSet'
ALTER TABLE [dbo].[NodeInstanceSet]
ADD CONSTRAINT [FK_ParentableInstanceNodeInstanceInheritance]
    FOREIGN KEY ([InheritanceId])
    REFERENCES [dbo].[ParentableInstanceSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ParentableInstanceNodeInstanceInheritance'
CREATE INDEX [IX_FK_ParentableInstanceNodeInstanceInheritance]
ON [dbo].[NodeInstanceSet]
    ([InheritanceId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------