USE [PmisDb]
GO

/****** Object:  Table [dbo].[Hard_MaterialConfig]    Script Date: 2018/9/21 22:10:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Hard_MaterialConfig](
	[SerialNo] [nvarchar](50) NOT NULL,
	[ItmeName] [nvarchar](100) NOT NULL,
	[ParentNo] [nvarchar](50) NOT NULL,
	[Memo] [nvarchar](200) NULL,
	[IsEnabled] [int] NULL,
	[CreateUser] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[LastUser] [int] NULL,
	[LastTime] [datetime] NULL,
 CONSTRAINT [PK_Hard_MaterialConfig] PRIMARY KEY CLUSTERED 
(
	[SerialNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

