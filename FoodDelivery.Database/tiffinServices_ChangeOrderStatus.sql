CREATE PROCEDURE [dbo].[tiffinServices_ChangeOrderStatus]
@OrderDetailID INT=0, -- 0 then Add mode
@OrderStatusID INT=0
AS 
BEGIN  
	UPDATE OrderDetail SET OrderStatus=@OrderStatusID WHERE OrderDetailID=@OrderDetailID
	Select 200 as status
END





GO