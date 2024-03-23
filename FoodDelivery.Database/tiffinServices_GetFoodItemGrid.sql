CREATE PROCEDURE [dbo].[tiffinServices_GetFoodItemGrid]
@TiffinServicesID int,
@Search NVARCHAR(50) = NULL,    
@DiscountInPercentage int=0, 
@IsAvailable INT=-1, -- -1: ALL, 0: InActive 1:Active   
@IsVegetarian INT=-1,
@IsBestSeller INT=-1,
@DisplayStart INT,    
@PageSize INT,    
@SortColumnName VARCHAR(50)=NULL,    
@SortOrder VARCHAR(50)='asc',    
@noOfRecords INT Out     
As    
BEGIN    
	DECLARE @QRY NVARCHAR(MAX)= '';      
    DECLARE @QRYStatus NVARCHAR(MAX)= '';    
    DECLARE @QRYWHERE NVARCHAR(MAX)= '';    
    DECLARE @PAGINATION NVARCHAR(MAX)= '';    
    DECLARE @CountRecord NVARCHAR(MAX)= '';     

    SET @QRY = 'SELECT  FoodID,
						FoodName,
						Price,
						DiscountInPercentage,
						IsJainAvailable,
						IsBestSeller,
						IsVegetarian,
						IsAvailable
                FROM(    
                    Select     
                        FoodID,
						FoodName,
						Price,
						DiscountInPercentage,
						IsJainAvailable,
						IsBestSeller,
						IsVegetarian,
						IsAvailable
                    FROM FOOD';     

    SET @QRYWHERE = ' WHERE ISNULL(IsTiffin,0)=1 AND ISNULL(IsDeleteD,0)=0 AND RestaurantID='+cast(@TiffinServicesID as nvarchar(MAX) ); 

	IF(@Search IS NOT NULL AND @Search<>'' AND LEN(@Search)>0)
    BEGIN    
        SET @QRYWHERE+=' AND FoodName like ''%'+COALESCE(@Search, '')+'%'''+'';    
    END;
    IF @DiscountInPercentage IS NOT NULL AND @DiscountInPercentage > 0
    BEGIN 
        SET @QRYWHERE+=' AND DiscountInPercentage=' + cast(@DiscountInPercentage as nvarchar(20))
    END   
    
    IF @IsAvailable IS NOT NULL AND @IsAvailable > -1
    BEGIN 
        SET @QRYWHERE+=' AND IsAvailable=' + cast(@IsAvailable as nvarchar(20))
    END 
	IF @IsVegetarian IS NOT NULL AND @IsVegetarian > -1
    BEGIN 
        SET @QRYWHERE+=' AND IsVegetarian=' + cast(@IsVegetarian as nvarchar(20))
    END 
	IF @IsBestSeller IS NOT NULL AND @IsBestSeller > -1
    BEGIN 
        SET @QRYWHERE+=' AND IsBestSeller=' + cast(@IsBestSeller as nvarchar(20))
    END 

    SET @QRYWHERE += ')TempTable'    
    SET @CountRecord = 'SELECT @noOfRecord=COUNT(*) FROM ('+(@QRY  + @QRYWHERE)+') countRecord';    
    SET @PAGINATION = ' ORDER BY '+@SortColumnName+' '+@SortOrder+' OFFSET '+CONVERT(VARCHAR, @DisplayStart)+' ROWS FETCH NEXT '+CONVERT(VARCHAR, @PageSize)+' ROWS ONLY'; 
    EXECUTE sp_executesql    
    @CountRecord,    
    N'@noOfRecord int OUTPUT',    
    @noOfRecord = @noOfRecords OUTPUT;    
    --Select (@QRY+@QRYWHERE+@PAGINATION);  
    EXEC (@QRY+@QRYWHERE+@PAGINATION);   
END





GO