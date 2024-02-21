
CREATE TABLE [dbo].[Restaurant](
	[RestaurantID] [int] IDENTITY(1,1) NOT NULL,
	[OwnerName] [nvarchar](200) NULL,
	[RestaurantName] [nvarchar](200) NULL,
	[Email] [nvarchar](200) NULL,
	[MobileNo] [nvarchar](20) NULL,
	[Password] [nvarchar](50) NULL,
	[ShopPlotNumber] [nvarchar](50) NULL,
	[Floor] [nvarchar](50) NULL,
	[BuildingName] [nvarchar](500) NULL,
	[ZipCode] [nvarchar](50) NULL,
	[RestaurantStatus] [int] NULL,
	[ImageName] [nvarchar](max) NULL,
	[IsActive] [bit] NULL,
	[IsDelete] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[ApprovedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Restaurant] PRIMARY KEY CLUSTERED 
(
	[RestaurantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1--Pending for Approval ,2--Approved' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Restaurant', @level2type=N'COLUMN',@level2name=N'RestaurantStatus'
GO

