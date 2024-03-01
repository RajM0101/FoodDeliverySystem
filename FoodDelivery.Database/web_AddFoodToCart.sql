CREATE PROCEDURE [dbo].[web_AddFoodToCart]
	@UserId INT =0,
    @FoodID INT =0,
	@Qauntity INT=0
AS
    IF NOT EXISTS(SELECT 1 FROM UserCart u  WHERE UserID = @UserId AND FoodID = @FoodID)
    BEGIN
        INSERT INTO UserCart(  
            UserID,
            FoodID,
			Qauntity,
            CreatedDate
        )
        VALUES ( 
            @UserId,
            @FoodID,
			1,
            GetDate()
        )

        SELECT 
            1 AS result ,                                               -- 1 - Insert New value in UserCart
            CONVERT(varchar,COUNT(u.UserID)) AS TotalCount,
            CONVERT(varchar, SUM(F.Price*u.Qauntity)) AS TotalPrice
        FROM UserCart u
        LEFT JOIN Food f ON f.FoodID = u.FoodID
        WHERE UserID=@UserId
    END
    ELSE
    BEGIN
		Update UserCart SET Qauntity=@Qauntity WHERE UserID = @UserId AND FoodID = @FoodID
		

		SELECT 
				3 AS result ,                                               -- 1 - Insert Qauntity value in UserCart
				CONVERT(varchar,COUNT(u.UserID)) AS TotalCount,
				CONVERT(varchar, SUM(F.Price*u.Qauntity)) AS TotalPrice
			FROM UserCart u
			LEFT JOIN Food f ON f.FoodID = u.FoodID
			WHERE UserID=@UserId 
    END


GO