
-- Exec restaurant_GetFoodItemDetailsById 3,1
CREATE PROCEDURE [dbo].[restaurant_GetFoodItemDetailsById]
@FoodID int,
@RestaurantID int
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
	WHERE ISNULL(f.IsDeleted,0)=0 AND f.FoodID=@FoodID AND RestaurantID=@RestaurantID
    
         
END


GO


