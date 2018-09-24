USE [PmisDb]
GO

/****** Object:  Table [dbo].[Hard_MaterialItem]    Script Date: 2018/9/25 0:22:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Hard_MaterialItem](
	[SerialNo] [nvarchar](50) NOT NULL,
	[ItemNo] [nvarchar](50) NULL,
	[ItemName] [nvarchar](50) NULL,
	[SubItemNo] [nvarchar](50) NULL,
	[SubItemType] [nvarchar](50) NULL,
	[MatTypeNo] [nvarchar](50) NOT NULL,
	[Brand] [nvarchar](100) NULL,
	[ItemSpecs] [nvarchar](500) NULL,
	[ItemModel] [nvarchar](500) NULL,
	[ItemUnit] [nvarchar](30) NULL,
	[ItemDesc] [nvarchar](100) NULL,
	[ItemMemo] [nvarchar](200) NULL,
	[WarrantyPeriod] [int] NULL,
	[warrantyType] [nvarchar](50) NULL,
	[WarrantyCost] [decimal](18, 2) NULL,
	[Taxrate] [int] NULL,
	[DeliveryDay] [int] NULL,
	[MinPurchase] [int] NULL,
	[MainSupplier] [nvarchar](100) NULL,
	[ConfigDesc] [nvarchar](500) NULL,
	[CostPrice] [decimal](18, 2) NULL,
	[PurchasePrice] [decimal](18, 2) NULL,
	[Offer] [decimal](18, 2) NULL,
	[SpecificUse] [nvarchar](500) NULL,
	[AttachFiles] [nvarchar](500) NULL,
	[IsEnergy] [int] NOT NULL,
	[IsDel] [int] NULL,
	[IsStop] [int] NOT NULL,
	[ItemQtySafety] [int] NULL,
	[CreateUser] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[LastUser] [int] NULL,
	[LastTime] [datetime] NULL,
 CONSTRAINT [PK_Hard_MaterialItem] PRIMARY KEY CLUSTERED 
(
	[SerialNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

