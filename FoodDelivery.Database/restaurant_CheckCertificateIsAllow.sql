CREATE PROCEDURE [dbo].[restaurant_CheckCertificateIsAllow]
@RestaurantID INT=0
AS 
BEGIN  
	DECLARE @FoodCount INT =(SELECT COUNT(1) FROM dbo.Food WHERE RestaurantID=@RestaurantID AND ISNULL(IsDeleted,0)=0)

	DECLARE @FoodRateSum DECIMAL(18,6) =0

	SET @FoodRateSum=(SELECT SUM(Rate) FROM dbo.FoodRating fr
		LEFT JOIN dbo.Food f ON f.FoodId=fr.FoodId
		LEFT JOIN dbo.Restaurant r ON r.RestaurantId=f.RestaurantId
		WHERE r.RestaurantId=@RestaurantID AND ISNULL(IsDelete,0)=0 AND ISNULL(IsDeleted,0)=0)


	SET @FoodCount=(CASE WHEN @FoodCount=0 THEN 1 ELSE @FoodCount END)

	DECLARE @AvgOfFood DECIMAL(18,6)=(select @FoodRateSum/@FoodCount)

	IF(@AvgOfFood>4)
	BEGIN
		SELECT CAST(1 AS bit) as IsCertificateIsAllow
	END
	ELSE
	BEGIN
		SELECT CAST(0 AS bit) as IsCertificateIsAllow
	END

	
END




GO