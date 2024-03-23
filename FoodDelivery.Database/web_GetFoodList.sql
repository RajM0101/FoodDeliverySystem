CREATE PROCEDURE [dbo].[web_GetFoodList]
@IsVegFood bit,
@IsJainFood bit,
@IsRatingCheck bit,
@PriceMinVal decimal(18,6),
@PriceMaxVal decimal(18,6)
AS 
BEGIN  
    DECLARE @QRY NVARCHAR(MAX)= '';      
    DECLARE @QRYWHERE NVARCHAR(MAX)= ''; 
	
	   
	  SET @QRY = '
	 SELECT  RestaurantID,
			FoodID,
			FoodName,
			Price,
			ImageName,
			Ingredient,
			DiscountInPercentage,
			IsBestSeller,
			IsVegetarian,
			IsJainAvailable,
			DisplayOrder,
			Rate,
			IsTiffin
    FROM( 
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
			 ,IsJainAvailable
			 ,DisplayOrder
			 ,(SELECT ROUND((ISNULL(SUM(Rate),0)/COUNT(UserID)),2) FROM FoodRating fr where fr.FoodID=f.FoodID GROUP BY FoodID) Rate
			 ,ISNULL(IsTiffin,0) AS IsTiffin
		FROM Food f
		LEFT JOIN Restaurant r ON r.RestaurantID=f.RestaurantID
		WHERE IsAvailable=1 AND ISNULL(IsDeleted,0)=0 AND r.RestaurantStatus=1 '

	 SET @QRYWHERE=' )TempTable WHERE 1=1';

	IF @IsVegFood=1
    BEGIN 
        SET @QRYWHERE+=' AND IsVegetarian=' + cast(@IsVegFood as nvarchar(20))
    END 
	IF @IsJainFood=1
    BEGIN 
        SET @QRYWHERE+=' AND IsBestSeller=' + cast(@IsJainFood as nvarchar(20))
    END 
	IF @IsRatingCheck=1
    BEGIN 
        SET @QRYWHERE+=' AND Rate>4'
    END  
	SET @QRYWHERE+=' AND Price BETWEEN '+CAST(@PriceMinVal AS nvarchar(50))+' AND ' +CAST(@PriceMaxVal AS nvarchar(50))
	 SET @QRYWHERE+=' ORDER BY DisplayOrder,IsBestSeller';

	 EXEC(@QRY+@QRYWHERE)
	 --select @qry+@QRYWHERE
 END




GO