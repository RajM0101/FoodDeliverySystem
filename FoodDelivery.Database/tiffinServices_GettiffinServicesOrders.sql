CREATE PROCEDURE [dbo].[tiffinServices_GettiffinServicesOrders]
@TiffinServicesID INT=0,
@Search NVARCHAR(50) = '',         
@DisplayStart INT,    
@PageSize INT,    
@SortColumnName VARCHAR(50)=NULL,    
@SortOrder VARCHAR(50)='ASC',
@noOfRecords INT Out     
As    
BEGIN
    SET @Search= REPLACE (@Search,'''','');

    DECLARE @QRY NVARCHAR(MAX)= '';
    DECLARE @QRYTABLE NVARCHAR(MAX)= '';      
    DECLARE @QRYStatus NVARCHAR(MAX)= '';    
    DECLARE @QRYWHERE NVARCHAR(MAX)= '';    
    DECLARE @PAGINATION NVARCHAR(MAX)= '';    
    DECLARE @CountRecord NVARCHAR(MAX)= ''; 
        
    DECLARE @CurrencyRate NVARCHAR(MAX) = '';

    SET @QRY = '    SELECT
                        OrderId,
						OrderDetailID,
                        FoodId,
						Name,
						Qauntity,
                        OrderDate,
                        Price,
						OrderStatus
                    FROM(
                    SELECT
                        o.OrderId AS OrderId,
						od.OrderDetailID as OrderDetailID,
                        (f.FoodID) AS FoodId,
						f.FoodName as Name,
						od.Qauntity,
                        dbo.GetDateFormat(o.CreatedDate) AS OrderDate,
						f.Price AS Price,
						os.OrderStatusName AS OrderStatus
                         ';

    SET @QRYTABLE = ' FROM [Order] o
                            JOIN OrderDetail od ON od.OrderId = o.OrderId 
                            JOIN Food f ON f.FoodID = od.FoodId
							JOIN OrderStatus os ON os.OrderStatusID=od.OrderStatus  ';

    SET @QRYWHERE += ' WHERE 1=1 '; 

    IF @TiffinServicesID IS NOT NULL AND @TiffinServicesID > -1
	BEGIN 
	    SET @QRYWHERE =@QRYWHERE+' AND od.RestaurantId=' + cast(@TiffinServicesID as nvarchar(20))+''
	END        

    SET @QRYWHERE += ')TempTable where 1=1' 

    IF(@Search IS NOT NULL AND len(@Search) > 0)        
    BEGIN        
        SET @QRYWHERE +=' AND (LOWER( Price)  like N''%' + LOWER(@Search) + '%'''+ '
        OR LOWER(Name) like N''%' +LOWER(@Search) + '%'''+'
        OR LOWER(CONVERT(VARCHAR(100), OrderDate, 110)) like ''%'+LOWER(@Search)+'%'')';
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