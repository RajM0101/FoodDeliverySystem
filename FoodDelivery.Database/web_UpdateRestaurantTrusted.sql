CREATE PROCEDURE [dbo].[web_UpdateRestaurantTrusted]
AS 
BEGIN  
	IF OBJECT_ID('tempdb..#TempRestaurant') IS NOT NULL  -- Create Manually New Table [#TempDesign]
	BEGIN
		DROP TABLE #TempRestaurant;
	END;

	CREATE TABLE #TempRestaurant
	(
		[RowNo] [int] Identity(1,1),
		[RestaurantID] INT NULL,
	);
	INSERT INTO #TempRestaurant(RestaurantID)
	SELECT RestaurantID FROM dbo.Restaurant WHERE RestaurantStatus=1 AND IsActive=1 AND ISNULL(IsDelete,0)=0	 

	DECLARE @RestaurantCount INT=(SELECT COUNT(1) FROM #TempRestaurant)
	DECLARE @Count INT=1

	WHILE(@Count <= @RestaurantCount)
	BEGIN
		DECLARE @RestaurantID INT=(SELECT RestaurantID FROM #TempRestaurant WHERE RowNo=@Count)
		DECLARE @FoodCount INT=(SELECT COUNT(1) FROM Food WHERE RestaurantID=@RestaurantID AND ISNULL(IsDeleted,0)=0)
		DECLARE @TotalFoodRate DECIMAL(18,6)=0

		SET @TotalFoodRate=(SELECT SUM(Rate) FROM dbo.FoodRating fr
		LEFT JOIN dbo.Food f ON f.FoodId=fr.FoodId
		LEFT JOIN dbo.Restaurant r ON r.RestaurantId=f.RestaurantId
		WHERE r.RestaurantId=@RestaurantID AND ISNULL(IsDelete,0)=0 AND ISNULL(IsDeleted,0)=0)

		SET @FoodCount=(CASE WHEN @FoodCount=0 THEN 1 ELSE @FoodCount END)

		DECLARE @Avg DECIMAL(18,6)=ISNULL(@TotalFoodRate,0)/@FoodCount

		IF(@Avg >4)
		BEGIN
			UPDATE Food set IsTrusted=1,ModifyDate=GETDATE() WHERE RestaurantID=@RestaurantID
		END
		SET @Count=@Count+1;
	 END
END


GO