CREATE PROCEDURE [dbo].[web_GetFoodList]
AS 
BEGIN  

	 SELECT 
		 f.RestaurantID
		 ,FoodID
		 ,FoodName
		 ,Price
		 ,f.ImageName
		 ,Ingredient
		 ,DiscountInPercentage
		 ,IsBestSeller
		 ,IsVegetarian 
	 FROM Food f
	 LEFT JOIN Restaurant r ON r.RestaurantID=f.RestaurantID
	 WHERE IsAvailable=1 AND ISNULL(IsDeleted,0)=0 AND r.RestaurantStatus=1
	 ORDER BY DisplayOrder,IsBestSeller

 END


GO