
CREATE PROCEDURE [dbo].[web_GetAllFoodByRestaurantID]
@RestaurantID INT=0
AS 
BEGIN  

	 SELECT 
		 RestaurantID
		 ,FoodID
		 ,FoodName
		 ,Price
		 ,ImageName
		 ,Ingredient
		 ,DiscountInPercentage
		 ,IsBestSeller
		 ,IsVegetarian 
	 FROM Food 
	 WHERE RestaurantID=@RestaurantID AND IsAvailable=1 AND ISNULL(IsDeleted,0)=0
	 ORDER BY DisplayOrder,IsBestSeller

 END

GO
