CREATE PROCEDURE [dbo].[admin_AdminUserLogin]
@EmailOrMobileNo  NVARCHAR(100),
@Password NVARCHAR(50) 
AS
BEGIN
    DECLARE @IsAdminUserAvailable INT= 0, @MatchPassword INT= 0;
    DECLARE @Result INT =0

	SELECT @IsAdminUserAvailable = COUNT(1) FROM AdminUser WHERE (Email = @EmailOrMobileNo OR MobileNo = @EmailOrMobileNo) AND ISNULL(IsDelete,0)=0  Group By AdminUserId
		 
    IF(@IsAdminUserAvailable = 0)
    BEGIN
        SET @Result = 1; 
		SELECT @Result AS Result;
	END
	ELSE
    BEGIN 
        SELECT @MatchPassword = COUNT(1) FROM AdminUser WHERE (Email = @EmailOrMobileNo OR MobileNo = @EmailOrMobileNo) AND [Password] = @Password AND ISNULL(IsDelete,0)=0 ;

		IF(@MatchPassword = 0)
		BEGIN
			SET @Result = 4;
			SELECT @Result AS Result;
		END
		ELSE
		BEGIN
            DECLARE @IsAdminUserActive INT= (SELECT COUNT(1) FROM AdminUser WHERE (Email = @EmailOrMobileNo OR MobileNo = @EmailOrMobileNo) 
                AND [Password] = @Password AND ISNULL(IsDelete,0)=0 AND IsActive=1)

            IF(@IsAdminUserActive=1)
            BEGIN
                SET @Result = 5;
                SELECT @Result AS Result;
                SELECT 
					a.AdminUserId,
					(a.FirstName+' '+a.LastName) as AdminUserName,
					a.Email as Email,
					a.MobileNo as MobileNo
				FROM AdminUser AS a
				WHERE (a.Email = @EmailOrMobileNo) OR (a.MobileNo = @EmailOrMobileNo)
            END
            ELSE
            BEGIN
                SET @Result = 2; 
                SELECT @Result AS Result;
            END
		END
	END 
END


GO