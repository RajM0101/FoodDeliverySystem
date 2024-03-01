CREATE PROCEDURE [dbo].[web_UserLogin]
@MobileNo  NVARCHAR(100),
@Password NVARCHAR(50) 
AS
BEGIN
    DECLARE @IsUserAvailable INT= 0, @MatchPassword INT= 0;
    DECLARE @Result INT =0

	SELECT @IsUserAvailable = COUNT(1) FROM dbo.[User] WHERE  MobileNo = @MobileNo AND ISNULL(IsDelete,0)=0
		 
    IF(@IsUserAvailable = 0)
    BEGIN
        SET @Result = 4; 
		SELECT @Result AS RetStatus;
	END
	ELSE
    BEGIN 
        SELECT @MatchPassword = COUNT(1) FROM dbo.[User] WHERE MobileNo = @MobileNo AND [Password] = @Password AND ISNULL(IsDelete,0)=0 ;

		IF(@MatchPassword = 0)
		BEGIN
			SET @Result = 2;
			SELECT @Result AS RetStatus;
		END
		ELSE
		BEGIN
            DECLARE @IsUserActive INT= (SELECT COUNT(1) FROM dbo.[User] WHERE MobileNo = @MobileNo
                AND [Password] = @Password AND ISNULL(IsDelete,0)=0 AND IsActive=1)

            IF(@IsUserActive=1)
            BEGIN
                SET @Result = 1;
                SELECT @Result AS RetStatus;
                SELECT 
					u.UserID as UserId,
					u.Name as FullName,
					u.MobileNo as MobileNumber
				FROM dbo.[User] AS u
				WHERE u.MobileNo = @MobileNo
            END
			ELSE
			BEGIN
				SET @Result = 3; 
                SELECT @Result AS RetStatus;
			END
		END
	END 
END




GO


