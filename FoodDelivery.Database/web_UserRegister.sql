CREATE PROCEDURE [dbo].[web_UserRegister]
@UserId int=0, -- 0 then Add mode 
@Name nvarchar(200), 
@MobileNo nvarchar(20), 
@Password nvarchar(50)=NULL,     
@Address nvarchar(500)= null
AS 
BEGIN  
	 DECLARE @RetStatus INT = 0
	IF (NOT EXISTS(SELECT 1 FROM dbo.[User] WHERE ISNULL(IsDelete,0) = 0 AND IsActive = 1 AND MobileNo=@MobileNo))
    BEGIN
		INSERT INTO dbo.[User]
        (
            Name,
            MobileNo,
            Password,
            Address,
            IsActive,
            CreatedDate
        )
		VALUES 
        (
            @Name,
            @MobileNo,
            @Password,
            @Address,
			1,
            GetDate()
        )
		SET @RetStatus = 1 --Registered
        SET @UserId = SCOPE_IDENTITY()

	END
	ELSE
    BEGIN
        SET @RetStatus = 2 --Already Exist
    END
	 SELECT @RetStatus AS RetStatus, @UserId AS UserId
END


GO