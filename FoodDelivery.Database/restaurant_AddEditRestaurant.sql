CREATE PROCEDURE [dbo].[restaurant_AddEditRestaurant]
@RestaurantID int=0, -- 0 then Add mode 
@OwnerName nvarchar(200),
@RestaurantName nvarchar(200),
@Email nvarchar(200) = null,  
@MobileNo nvarchar(20), 
@Password nvarchar(50)=NULL,  
@ShopPlotNumber nvarchar(50),
@Floor nvarchar(50),     
@BuildingName nvarchar(500)= null,   
@ZipCode nvarchar(50)= null,
@RestaurantImageName nvarchar(max)=null
AS 
BEGIN  
	IF @RestaurantID = 0 
	BEGIN
		INSERT INTO [Restaurant]
        (
            OwnerName,
            RestaurantName,
            Email,
            MobileNo,
            Password,
            ShopPlotNumber,
            Floor,
            BuildingName,
            ZipCode,
			RestaurantStatus,
            IsActive,
            CreatedDate
        )
		VALUES 
        (
            @OwnerName,
            @RestaurantName,
            @Email,
            @MobileNo,
            @Password,
            @ShopPlotNumber,
            @Floor, 
            @BuildingName,
            @ZipCode, 
            1, 
			1,
            GetDate()
        )
	END
	ELSE
	BEGIN
		Update [Restaurant]
		SET
            OwnerName=@OwnerName,
            RestaurantName=@RestaurantName,
            Email=@Email,
            MobileNo=@MobileNo,
            ShopPlotNumber=@ShopPlotNumber,
            Floor=@Floor,
            BuildingName=@BuildingName,
			ImageName=@RestaurantImageName,
            ZipCode=@ZipCode,
            ModifiedDate=GetDate()
		WHERE RestaurantID=@RestaurantID
	END
END


GO