CREATE PROCEDURE [dbo].[tiffinServices_GetFoodItemDetailsById]
@FoodID int,
@TiffinServicesID int
AS 
BEGIN  

	SELECT 
		FoodID,
		FoodName, 
		Price, 
		Ingredient, 
		IsJainAvailable, 
		IsBestSeller,
		IsVegetarian,
		ImageName as FoodImageName,
		DiscountInPercentage,
		DisplayOrder,
		IsAvailable
	FROM dbo.Food f
	WHERE ISNULL(f.IsDeleted,0)=0 AND f.FoodID=@FoodID AND RestaurantID=@TiffinServicesID
    
         
END



GO