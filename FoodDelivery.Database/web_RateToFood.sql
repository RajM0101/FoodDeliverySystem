CREATE PROCEDURE [dbo].[web_RateToFood]
@OrderDetailId INT=0,
@UserId INT=0,
@Rate DECIMAL(18,6)=0
AS 
BEGIN  
	DECLARE @FoodId INT=0
	SELECT @FoodId=FoodId FROM OrderDetail WHERE OrderDetailID=@OrderDetailId
	 
	IF EXISTS(Select 1 from FoodRating WHERE FoodID=@FoodID AND UserID=@UserId)
    BEGIN 
        UPDATE FoodRating SET Rate=@Rate WHERE FoodID=@FoodID AND UserID=@UserId
	END
	ELSE 
	BEGIN
		INSERT INTO FoodRating(FoodID,UserID,Rate,RateDate)
		VALUES(@FoodId,@UserId,@Rate,GETDATE())
	END
	 
	 Select cast(1 as int) as Status
 END



GO