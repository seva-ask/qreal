USE [QReal]
GO
/****** Object:  Table [dbo].[LogicalInstances]    Script Date: 03/19/2010 03:14:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogicalInstances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_LogicalInstances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InstanceProperties]    Script Date: 03/19/2010 03:14:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstanceProperties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InstanceId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](50) NULL,
 CONSTRAINT [PK_InstanceProperties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GraphicInstances]    Script Date: 03/19/2010 03:14:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GraphicInstances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LogicalId] [int] NOT NULL,
	[ParentId] [int] NULL,
	[X] [int] NOT NULL,
	[Y] [int] NOT NULL,
 CONSTRAINT [PK_GraphicInstances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_PropertiesToLogicalInstances]    Script Date: 03/19/2010 03:14:36 ******/
ALTER TABLE [dbo].[InstanceProperties]  WITH CHECK ADD  CONSTRAINT [FK_PropertiesToLogicalInstances] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[LogicalInstances] ([Id])
GO
ALTER TABLE [dbo].[InstanceProperties] CHECK CONSTRAINT [FK_PropertiesToLogicalInstances]
GO
/****** Object:  ForeignKey [FK_GraphicParents]    Script Date: 03/19/2010 03:14:36 ******/
ALTER TABLE [dbo].[GraphicInstances]  WITH CHECK ADD  CONSTRAINT [FK_GraphicParents] FOREIGN KEY([ParentId])
REFERENCES [dbo].[GraphicInstances] ([Id])
GO
ALTER TABLE [dbo].[GraphicInstances] CHECK CONSTRAINT [FK_GraphicParents]
GO
/****** Object:  ForeignKey [FK_GraphicToLogical]    Script Date: 03/19/2010 03:14:36 ******/
ALTER TABLE [dbo].[GraphicInstances]  WITH CHECK ADD  CONSTRAINT [FK_GraphicToLogical] FOREIGN KEY([LogicalId])
REFERENCES [dbo].[LogicalInstances] ([Id])
GO
ALTER TABLE [dbo].[GraphicInstances] CHECK CONSTRAINT [FK_GraphicToLogical]
GO
