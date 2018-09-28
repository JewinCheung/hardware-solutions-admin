USE [PmisDb]
GO

/****** Object:  Table [dbo].[Hard_BaseDictType]    Script Date: 2018/9/25 10:41:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Hard_BaseDictType](
	[SerialNo] [nvarchar](50) NOT NULL,
	[DictTypeNo] [nvarchar](50) NOT NULL,
	[DictTypeName] [nvarchar](100) NOT NULL,
	[Validate] [datetime] NOT NULL,
	[DisDate] [datetime] NULL,
	[TypeLevel] [int] NOT NULL,
	[ParentNo] [nvarchar](50) NULL,
	[IsEnabled] [int] NOT NULL,
	[BelongArea] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateUser] [int] NOT NULL,
	[LastTime] [datetime] NULL,
	[LastUser] [int] NULL,
	[Memo] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Hard_BaseDictType] PRIMARY KEY CLUSTERED 
(
	[SerialNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

