CREATE PROCEDURE [dbo].[admin_GetUserList]
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
                        UserId,Name,MobileNo,Address
                    FROM(
                    select 
						UserId,Name,MobileNo,Address
                         ';

    SET @QRYTABLE = ' FROM dbo.[User] ';

    SET @QRYWHERE += ' WHERE 1=1 '; 

    SET @QRYWHERE += ')TempTable where 1=1' 

    IF(@Search IS NOT NULL AND len(@Search) > 0)        
    BEGIN        
        SET @QRYWHERE +=' AND (LOWER(Name) like N''%' +LOWER(@Search) + '%'')';
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