CREATE PROCEDURE [dbo].[restaurant_GetResataurantDetailsById]
@RestaurantID int
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
		ImageName as RestaurantImageName
	FROM dbo.Restaurant r
	WHERE RestaurantID=@RestaurantID
    
         
END




GO