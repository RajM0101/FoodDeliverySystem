CREATE TABLE [dbo].[OrderPaymentDetail](
	[OrderPaymentDetailID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[OrderID] [int] NOT NULL,
	[OrderDetailID] [int] NULL,
	[PayoutAmount] [money] NULL,
	[FoodPrice] [money] NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_SellerComissionDetail] PRIMARY KEY CLUSTERED 
(
	[OrderPaymentDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO