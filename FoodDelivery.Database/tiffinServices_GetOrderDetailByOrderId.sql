CREATE PROCEDURE [dbo].[tiffinServices_GetOrderDetailByOrderId]
@OrderDetailID INT=0, -- 0 then Add mode
@TiffinServicesID INT=0
AS 
BEGIN  
	 SELECT
        o.OrderId AS OrderId,
        (od.OrderDetailID) AS OrderDetailID,
        (f.FoodID) AS FoodId,
		f.FoodName as Name,
		f.Price,
		od.Qauntity,
        (SELECT (SUM(f1.Price*(od1.Qauntity))) FROM Food f1 
                                LEFT JOIN [OrderDetail] od1 ON od1.FoodId = f1.FoodId
                                WHERE od1.OrderId = o.OrderId AND od.RestaurantID=@TiffinServicesID AND od1.OrderDetailID=@OrderDetailID
        ) AS TotalPrice,
		od.OrderStatus
    FROM [Order] o
    JOIN OrderDetail od ON od.OrderId = o.OrderId 
    JOIN Food f ON f.FoodID = od.FoodId 
	Where od.RestaurantID=@TiffinServicesID AND od.OrderDetailID=@OrderDetailID
END







GO