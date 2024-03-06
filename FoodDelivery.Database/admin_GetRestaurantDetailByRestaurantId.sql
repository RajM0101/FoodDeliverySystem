--Exec admin_GetRestaurantDetailByRestaurantId  2
CREATE PROCEDURE [dbo].[admin_GetRestaurantDetailByRestaurantId]
@RestaurantID INT=0
AS 
BEGIN  
	 SELECT
        RestaurantID,
		OwnerName,
		RestaurantName,
		MobileNo,
		Email,
		ShopPlotNumber,
		Floor,
		BuildingName,
		ZipCode,
		RestaurantStatus,
		ImageName as RestaurantImageName
    FROM [Restaurant] r
	Where r.RestaurantID=@RestaurantID
END






GO