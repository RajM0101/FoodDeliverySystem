CREATE PROCEDURE [dbo].[tiffinServices_DeleteFood]
@FoodID INT
AS
BEGIN
	DECLARE @ImageName VARCHAR(200)=''  
    SELECT @ImageName=ImageName FROM [Food] WHERE FoodID=@FoodID;

	IF EXISTS(Select 1 from [Food] WHERE FoodID=@FoodID)
    BEGIN 
        UPDATE [Food] SET IsDeleted=1 WHERE FoodID=@FoodID;    
        SELECT @ImageName AS ImageName, cast(1 AS bit) AS AllowToDelete
	END
	ELSE 
	BEGIN
		SELECT @ImageName AS ImageName, cast(0 AS bit) AS AllowToDelete
	END
END;

GO