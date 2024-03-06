CREATE PROCEDURE [dbo].[web_GetAllRestaurant]
AS 
BEGIN  

	Select 
		RestaurantID
		,RestaurantName
		,ShopPlotNumber +Floor +BuildingName as Address
		,ZipCode
		,ImageName
	 from Restaurant
	where IsActive=1 AND Isnull(IsDelete,0)=0

 end

GO