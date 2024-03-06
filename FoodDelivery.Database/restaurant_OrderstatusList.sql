CREATE PROCEDURE [dbo].[restaurant_OrderstatusList]
AS 
BEGIN  
	 SELECT
		o.OrderStatusID,
		o.OrderStatusName
    FROM [OrderStatus] o
END





GO