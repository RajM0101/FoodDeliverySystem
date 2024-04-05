CREATE PROCEDURE [dbo].[web_GetSearchData] 
@SearchBy nvarchar(200) = ''

AS
BEGIN
    DECLARE @Query nvarchar(max) = ''
    DECLARE @WhereQuery nvarchar(max) = ''
   
    DECLARE @ResQuery nvarchar(max) = ''
    DECLARE @ResWhereQuery nvarchar(max) = ''
                
        SET @Query = '
        Select
			FoodID,
			FoodName,
			Price,
			Ingredient,
			f.ImageName,
			(SELECT isnull(ROUND((ISNULL(SUM(Rate),0)/COUNT(UserID)),2),0) FROM FoodRating fr where fr.FoodID=f.FoodID GROUP BY FoodID) Rate
			,ISNULL(IsTrusted,0) AS IsTrusted   
        FROM dbo.Food f
		LEFT JOIN dbo.Restaurant r ON r.RestaurantID=f.RestaurantID
		WHERE ISNULL(f.IsDeleted,0)=0 AND ISNULL(f.IsAvailable,0)=1 AND ISNULL(RestaurantStatus,0)=1'
                    
        IF(LEN(@SearchBy)>0 AND @SearchBy IS NOT NULL )
        BEGIN
            SET @WhereQuery = 'AND f.FoodName like ''%' + @SearchBy + '%'' ';
        END     
		
		SET @ResQuery = '
        SELECT 
			RestaurantID,
			RestaurantName,
			ImageName,
			ISNULL(IsTiffinServices,0) AS IsTiffinServices            
        FROM dbo.Restaurant r
		WHERE ISNULL(r.IsDelete,0)=0 AND ISNULL(r.IsActive,0)=1 AND ISNULL(RestaurantStatus,0)=1 '
                    
        IF(LEN(@SearchBy)>0 AND @SearchBy IS NOT NULL )
        BEGIN
            SET @ResWhereQuery = 'AND r.RestaurantName like ''%' + @SearchBy + '%'' ';
        END          
           
        EXEC (@Query+@WhereQuery) 
		EXEC (@ResQuery+@ResWhereQuery) 
END






GO