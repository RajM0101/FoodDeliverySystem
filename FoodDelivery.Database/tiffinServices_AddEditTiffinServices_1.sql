CREATE PROCEDURE [dbo].[tiffinServices_AddEditTiffinServices]
@TiffinServicesID int=0, -- 0 then Add mode 
@OwnerName nvarchar(200),
@TiffinServicesName nvarchar(200),
@Email nvarchar(200) = null,  
@MobileNo nvarchar(20), 
@Password nvarchar(50)=NULL,  
@ShopPlotNumber nvarchar(50),
@Floor nvarchar(50),     
@BuildingName nvarchar(500)= null,   
@ZipCode nvarchar(50)= null,
@TiffinServicesImageName nvarchar(max)=null
AS 
BEGIN  
	IF @TiffinServicesID = 0 
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
            CreatedDate,
			IsTiffinServices
        )
		VALUES 
        (
            @OwnerName,
            @TiffinServicesName,
            @Email,
            @MobileNo,
            @Password,
            @ShopPlotNumber,
            @Floor, 
            @BuildingName,
            @ZipCode, 
            0, 
			1,
            GetDate(),
			1
        )
	END
	ELSE
	BEGIN
		Update [Restaurant]
		SET
            OwnerName=@OwnerName,
            RestaurantName=@TiffinServicesName,
            Email=@Email,
            MobileNo=@MobileNo,
            ShopPlotNumber=@ShopPlotNumber,
            Floor=@Floor,
            BuildingName=@BuildingName,
			ImageName=@TiffinServicesImageName,
            ZipCode=@ZipCode,
            ModifiedDate=GetDate()
		WHERE RestaurantID=@TiffinServicesID
	END
END





GO