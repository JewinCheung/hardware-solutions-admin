USE [PmisDb]
GO

/****** Object:  Table [dbo].[Hard_MaterialType]    Script Date: 2018/9/21 22:10:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Hard_MaterialType](
	[SerialNo] [nvarchar](50) NOT NULL,
	[MatTypeNo] [nvarchar](50) NOT NULL,
	[MatTypeName] [nvarchar](50) NULL,
	[ParentNo] [nvarchar](50) NOT NULL,
	[MatTypeMemo] [nvarchar](500) NULL,
	[CreateUser] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[LastUser] [int] NULL,
	[LastTime] [datetime] NULL,
 CONSTRAINT [PK_Hard_MaterialType_1] PRIMARY KEY CLUSTERED 
(
	[SerialNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

