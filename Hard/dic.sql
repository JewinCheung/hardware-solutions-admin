USE [PmisDb]
GO

/****** Object:  Table [dbo].[Hard_BaseDict]    Script Date: 2018/9/25 10:40:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Hard_BaseDict](
	[SerialNo] [nvarchar](50) NOT NULL,
	[DictType] [nvarchar](100) NOT NULL,
	[DictNo] [nvarchar](50) NOT NULL,
	[DictName] [nvarchar](100) NOT NULL,
	[DictSort] [int] NOT NULL,
	[Memo] [nvarchar](500) NULL,
	[IsEnabled] [int] NOT NULL,
	[BelongArea] [nvarchar](50) NULL,
	[DictParam] [nvarchar](500) NULL,
	[CreateUser] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[LastUser] [int] NULL,
	[LastTime] [datetime] NULL,
 CONSTRAINT [PK_Hard_BaseDict] PRIMARY KEY CLUSTERED 
(
	[SerialNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

