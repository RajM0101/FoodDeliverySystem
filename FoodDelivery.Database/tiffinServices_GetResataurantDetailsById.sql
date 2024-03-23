CREATE PROCEDURE [dbo].[tiffinServices_GetResataurantDetailsById]
@TiffinServicesID int
AS 
BEGIN  

	SELECT 
		RestaurantID as TiffinServicesID,
		OwnerName, 
		RestaurantName as TiffinServicesName, 
		MobileNo, 
		Email, 
		ShopPlotNumber,
		Floor,
		BuildingName,
		ZipCode,
		ImageName as TiffinServicesImageName
	FROM dbo.Restaurant r
	WHERE RestaurantID=@TiffinServicesID 
    
         
END







GO