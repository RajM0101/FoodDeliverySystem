﻿CREATE PROCEDURE [dbo].[web_GetFoodItemDetailsById]
@FoodID int
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
		IsAvailable,
		ISNULL(IsTrusted,0) AS IsTrusted
	FROM dbo.Food f
	WHERE ISNULL(f.IsDeleted,0)=0 AND f.FoodID=@FoodID
    
         
END




GO