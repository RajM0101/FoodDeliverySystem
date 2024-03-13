CREATE PROCEDURE [dbo].[web_RateToFood]
@OrderDetailId INT=0,
@UserId INT=0,
@Rate DECIMAL(18,6)=0
AS 
BEGIN  
	DECLARE @FoodId INT=0
	SELECT @FoodId=FoodId FROM OrderDetail WHERE OrderDetailID=@OrderDetailId

	INSERT INTO FoodRating(FoodID,UserID,Rate,RateDate)
	VALUES(@FoodId,@UserId,@Rate,GETDATE())
	 
	 Select cast(1 as int) as Status
 END


GO