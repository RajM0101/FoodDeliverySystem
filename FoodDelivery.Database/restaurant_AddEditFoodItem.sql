

CREATE PROCEDURE [dbo].[restaurant_AddEditFoodItem]
@FoodID INT=0, -- 0 then Add mode
@RestaurantID INT=0,  
@FoodName VARCHAR(200),  
@Price INT, 
@Ingredient NVARCHAR(MAX), 
@IsJainAvailable BIT,  
@IsBestSeller BIT,  
@IsVegetarian BIT,  
@ImageName NVARCHAR(500),
@DisplayOrder INT,
@IsAvailable BIT,
@DiscountInPercentage INT  
AS 
BEGIN  
	IF @FoodID = 0 
	BEGIN 
		INSERT INTO Food
        (
			RestaurantID,
            FoodName,
            Price,
            Ingredient,
            IsJainAvailable,
			IsVegetarian,
            IsBestSeller,
            ImageName, 
            DisplayOrder,			
			DiscountInPercentage,
            IsAvailable, 
			CreatedDate
        )
		VALUES 
        (
			@RestaurantID,
            @FoodName, 
            @Price, 
            @Ingredient, 
            @IsJainAvailable, 
            @IsVegetarian, 
            @IsBestSeller, 
            @ImageName,
            @DisplayOrder, 
            @DiscountInPercentage,
			@IsAvailable, 
            GetDate()
        )
		
		SET @FoodID=SCOPE_IDENTITY(); 
		select 200 AS status,@FoodID AS FoodItemID
	END
	ELSE
	BEGIN
		UPDATE Food SET
		    FoodName=@FoodName,
            Price=@Price,
            Ingredient=@Ingredient,
            IsJainAvailable=@IsJainAvailable, 
            IsVegetarian=@IsVegetarian, 
            IsBestSeller=@IsBestSeller, 
            ImageName= (Case when @ImageName IS NULL THEN ImageName ELSE @ImageName END), 
            DisplayOrder=@DisplayOrder,
			DiscountInPercentage =@DiscountInPercentage,
            ModifyDate=GetDate() 
		WHERE FoodID=@FoodID
	END
	select 201 AS status,@FoodID AS FoodItemID
END


GO


