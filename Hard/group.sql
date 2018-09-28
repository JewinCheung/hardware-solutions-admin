USE [PmisDb]
GO

/****** Object:  Table [dbo].[Hard_GroupByProLine]    Script Date: 2018/9/28 15:38:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Hard_GroupByProLine](
	[SerialNo] [nvarchar](50) NOT NULL,
	[ProLineNo] [nvarchar](50) NOT NULL,
	[GroupNo] [nvarchar](50) NOT NULL,
	[GroupType] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateUser] [int] NOT NULL,
 CONSTRAINT [PK_Hard_GroupByProLine] PRIMARY KEY CLUSTERED 
(
	[SerialNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分组类型, 硬件类别编号：MatTypeNo（GroupNo=MatTypeNo）；硬件编号：ItemNo（GroupNo=ItemNo）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Hard_GroupByProLine', @level2type=N'COLUMN',@level2name=N'GroupType'
GO

