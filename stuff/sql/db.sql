USE [QReal]
GO
/****** Object:  Table [dbo].[Instances]    Script Date: 03/06/2010 03:31:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ParentId] [int] NULL,
 CONSTRAINT [PK_Entities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_Parents]    Script Date: 03/06/2010 03:31:44 ******/
ALTER TABLE [dbo].[Instances]  WITH CHECK ADD  CONSTRAINT [FK_Parents] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Instances] ([Id])
GO
ALTER TABLE [dbo].[Instances] CHECK CONSTRAINT [FK_Parents]
GO
