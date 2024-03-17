CREATE PROCEDURE [dbo].[admin_GetAllFoodByRestaurantID]
@Search NVARCHAR(50) = '',         
@DisplayStart INT,    
@PageSize INT,    
@SortColumnName VARCHAR(50)=NULL,    
@SortOrder VARCHAR(50)='ASC',
@RestaurantID INT=0,
@noOfRecords INT Out

AS 
BEGIN  
	 SET @Search= REPLACE (@Search,'''','');

    DECLARE @QRY NVARCHAR(MAX)= '';
    DECLARE @QRYTABLE NVARCHAR(MAX)= '';      
    DECLARE @QRYStatus NVARCHAR(MAX)= '';    
    DECLARE @QRYWHERE NVARCHAR(MAX)= '';    
    DECLARE @PAGINATION NVARCHAR(MAX)= '';    
    DECLARE @CountRecord NVARCHAR(MAX)= ''; 

	SET @QRY = '    SELECT
                        RestaurantID,
						FoodID,
						FoodName,
						Price,
						ImageName,
                        Ingredient,	
						DiscountInPercentage,
						IsBestSeller,
						IsVegetarian,
						IsAvailable,
						Rate
                    FROM(
                    SELECT 
						 RestaurantID
						 ,f.FoodID
						 ,FoodName
						 ,Price
						 ,ImageName
						 ,Ingredient
						 ,DiscountInPercentage
						 ,IsBestSeller
						 ,IsVegetarian 
						 ,IsAvailable
						 ,(SELECT ROUND((ISNULL(SUM(Rate),0)/COUNT(UserID)),2) FROM FoodRating fr where fr.FoodID=f.FoodID GROUP BY FoodID) Rate
                         ';

    SET @QRYTABLE = ' FROM Food f';

    SET @QRYWHERE += ' WHERE 1=1 '; 
	 IF @RestaurantId IS NOT NULL AND @RestaurantId > -1
	BEGIN 
	    SET @QRYWHERE =@QRYWHERE+' AND RestaurantID=' + cast(@RestaurantId as nvarchar(20))+''
	END   

    SET @QRYWHERE += ')TempTable where 1=1'

	IF(@Search IS NOT NULL AND len(@Search) > 0)        
    BEGIN        
        SET @QRYWHERE +=' AND (LOWER(Ingredient)  like N''%' + LOWER(@Search) + '%'''+ '
        OR LOWER(FoodName) like N''%' +LOWER(@Search) + '%'')';
    END   
    SET @CountRecord = 'SELECT @noOfRecord=COUNT(*) FROM ('+(@QRY+ @QRYTABLE + @QRYWHERE)+') countRecord';    
    SET @PAGINATION = ' ORDER BY '+@SortColumnName+' '+@SortOrder+' OFFSET '+CONVERT(VARCHAR, @DisplayStart)+' ROWS FETCH NEXT '+CONVERT(VARCHAR, @PageSize)+' ROWS ONLY'; 
    EXECUTE sp_executesql    
    @CountRecord,    
    N'@noOfRecord int OUTPUT',    
    @noOfRecord = @noOfRecords OUTPUT;    

    --SELECT (@QRY+@QRYTABLE+@QRYWHERE+@PAGINATION);    
    EXEC (@QRY+@QRYTABLE+@QRYWHERE+@PAGINATION);  


 END



GO