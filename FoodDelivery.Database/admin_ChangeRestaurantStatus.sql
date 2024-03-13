CREATE PROCEDURE [dbo].[admin_ChangeRestaurantStatus]
@RestaurantID INT=0, -- 0 then Add mode
@RestaurantStatus bit=0
AS 
BEGIN  
	UPDATE Restaurant SET RestaurantStatus=@RestaurantStatus WHERE RestaurantID=@RestaurantID
	Select 200 as status
END





GO