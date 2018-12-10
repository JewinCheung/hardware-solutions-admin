USE [PmisDb]
GO

/****** Object:  Table [dbo].[Hard_GroupByItme]    Script Date: 2018/12/10 23:01:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Hard_GroupByItme](
	[SerialNo] [nvarchar](50) NOT NULL,
	[ProLineNo] [nvarchar](50) NULL,
	[ItemNo] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[CreateUser] [int] NULL,
 CONSTRAINT [PK_Hard_GroupByItme] PRIMARY KEY CLUSTERED 
(
	[SerialNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

