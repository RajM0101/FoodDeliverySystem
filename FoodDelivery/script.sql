USE [master]
GO
/****** Object:  Database [FoodDelivery]    Script Date: 16-02-2024 22:36:59 ******/
CREATE DATABASE [FoodDelivery]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FoodDelivery', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLSERVER\MSSQL\DATA\FoodDelivery.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FoodDelivery_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLSERVER\MSSQL\DATA\FoodDelivery_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [FoodDelivery] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FoodDelivery].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FoodDelivery] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FoodDelivery] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FoodDelivery] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FoodDelivery] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FoodDelivery] SET ARITHABORT OFF 
GO
ALTER DATABASE [FoodDelivery] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FoodDelivery] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FoodDelivery] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FoodDelivery] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FoodDelivery] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FoodDelivery] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FoodDelivery] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FoodDelivery] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FoodDelivery] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FoodDelivery] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FoodDelivery] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FoodDelivery] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FoodDelivery] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FoodDelivery] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FoodDelivery] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FoodDelivery] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FoodDelivery] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FoodDelivery] SET RECOVERY FULL 
GO
ALTER DATABASE [FoodDelivery] SET  MULTI_USER 
GO
ALTER DATABASE [FoodDelivery] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FoodDelivery] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FoodDelivery] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FoodDelivery] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FoodDelivery] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'FoodDelivery', N'ON'
GO
ALTER DATABASE [FoodDelivery] SET QUERY_STORE = OFF
GO
USE [FoodDelivery]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [FoodDelivery]
GO
/****** Object:  Table [dbo].[AdminUser]    Script Date: 16-02-2024 22:36:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminUser](
	[AdminUserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](100) NULL,
	[MobileNo] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[SpecialPermission] [bit] NULL,
 CONSTRAINT [PK_AdminUser] PRIMARY KEY CLUSTERED 
(
	[AdminUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Food]    Script Date: 16-02-2024 22:36:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food](
	[FoodID] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantID] [int] NULL,
	[FoodName] [nvarchar](200) NULL,
	[Price] [int] NULL,
	[Ingredient] [nvarchar](500) NULL,
	[IsJainAvailable] [bit] NULL,
	[IsVegetarian] [bit] NULL,
	[IsBestSeller] [bit] NULL,
	[ImageName] [nvarchar](500) NULL,
	[DisplayOrder] [int] NULL,
	[DiscountInPercentage] [int] NULL,
	[IsAvailable] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[ModifyDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Food] PRIMARY KEY CLUSTERED 
(
	[FoodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Restaurant]    Script Date: 16-02-2024 22:36:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Restaurant](
	[RestaurantID] [int] IDENTITY(1,1) NOT NULL,
	[OwnerName] [nvarchar](200) NULL,
	[RestaurantName] [nvarchar](200) NULL,
	[Email] [nvarchar](200) NULL,
	[MobileNo] [nvarchar](20) NULL,
	[Password] [nvarchar](50) NULL,
	[ShopPlotNumber] [nvarchar](50) NULL,
	[Floor] [nvarchar](50) NULL,
	[BuildingName] [nvarchar](500) NULL,
	[ZipCode] [nvarchar](50) NULL,
	[RestaurantStatus] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDelete] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[ApprovedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Restaurant] PRIMARY KEY CLUSTERED 
(
	[RestaurantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  StoredProcedure [dbo].[restaurant_AddEditFoodItem]    Script Date: 16-02-2024 22:36:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[restaurant_AddEditFoodItem]
@FoodID INT=0, -- 0 then Add mode
@RestaurantID INT=0,  
@FoodName VARCHAR(200),  
@Price INT, 
@Ingredient NVARCHAR(MAX), 
@IsJainAvailable BIT,  
@IsBestSeller BIT,  
@IsVegetarian BIT,  
@ImageName NVARCHAR(500),
@DisplayOrder INT,
@IsAvailable BIT,
@DiscountInPercentage INT  
AS 
BEGIN  
	IF @FoodID = 0 
	BEGIN 
		INSERT INTO Food
        (
			RestaurantID,
            FoodName,
            Price,
            Ingredient,
            IsJainAvailable,
			IsVegetarian,
            IsBestSeller,
            ImageName, 
            DisplayOrder,			
			DiscountInPercentage,
            IsAvailable, 
			CreatedDate
        )
		VALUES 
        (
			@RestaurantID,
            @FoodName, 
            @Price, 
            @Ingredient, 
            @IsJainAvailable, 
            @IsVegetarian, 
            @IsBestSeller, 
            @ImageName,
            @DisplayOrder, 
            @DiscountInPercentage,
			@IsAvailable, 
            GetDate()
        )
		
		SET @FoodID=SCOPE_IDENTITY(); 
		select 200 AS status,@FoodID AS FoodItemID
	END
	ELSE
	BEGIN
		UPDATE Food SET
		    FoodName=@FoodName,
            Price=@Price,
            Ingredient=@Ingredient,
            IsJainAvailable=@IsJainAvailable, 
            IsVegetarian=@IsVegetarian, 
            IsBestSeller=@IsBestSeller, 
            ImageName= (Case when @ImageName IS NULL THEN ImageName ELSE @ImageName END), 
            DisplayOrder=@DisplayOrder,
			DiscountInPercentage =@DiscountInPercentage,
            ModifyDate=GetDate() 
		WHERE FoodID=@FoodID
	END
	select 201 AS status,@FoodID AS FoodItemID
END


GO
/****** Object:  StoredProcedure [dbo].[restaurant_AddEditRestaurant]    Script Date: 16-02-2024 22:36:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
--EXEC restaurant_AddEditRestaurant 0,8989898898,'shiv','shiv@gmail.com','surat',1,NULL,NULL,NULL,1,1,1
*/
CREATE PROCEDURE [dbo].[restaurant_AddEditRestaurant]
@RestaurantID int=0, -- 0 then Add mode 
@OwnerName nvarchar(200),
@RestaurantName nvarchar(200),
@Email nvarchar(200) = null,  
@MobileNo nvarchar(20), 
@Password nvarchar(50),  
@ShopPlotNumber nvarchar(50),
@Floor nvarchar(50),     
@BuildingName nvarchar(500)= null,   
@ZipCode nvarchar(50)= null
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
            Password=@Password,
            ShopPlotNumber=@ShopPlotNumber,
            Floor=@Floor,
            BuildingName=@BuildingName,
            ZipCode=@ZipCode,
            ModifiedDate=GetDate()
		WHERE RestaurantID=@RestaurantID
	END
END

GO
/****** Object:  StoredProcedure [dbo].[restaurant_DeleteFood]    Script Date: 16-02-2024 22:36:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[restaurant_DeleteFood]
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
/****** Object:  StoredProcedure [dbo].[restaurant_GetFoodItemDetailsById]    Script Date: 16-02-2024 22:36:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Exec restaurant_GetFoodItemDetailsById 3,1
CREATE PROCEDURE [dbo].[restaurant_GetFoodItemDetailsById]
@FoodID int,
@RestaurantID int
AS 
BEGIN  

	SELECT 
		FoodID,
		FoodName, 
		Price, 
		Ingredient, 
		IsJainAvailable, 
		IsBestSeller,
		IsVegetarian,
		ImageName as FoodImageName,
		DiscountInPercentage,
		DisplayOrder,
		IsAvailable
	FROM dbo.Food f
	WHERE ISNULL(f.IsDeleted,0)=0 AND f.FoodID=@FoodID AND RestaurantID=@RestaurantID
    
         
END


GO
/****** Object:  StoredProcedure [dbo].[restaurant_GetFoodItemGrid]    Script Date: 16-02-2024 22:36:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
Declare @noOfRecords int  
Exec [restaurant_GetFoodItemGrid] 1,NULL,NULL,-1,-1,-1,0,10,NULL,'DESC',@noOfRecords out
Select @noOfRecords 
*/
CREATE PROCEDURE [dbo].[restaurant_GetFoodItemGrid]
@RestaurantID int,
@Search NVARCHAR(50) = NULL,    
@DiscountInPercentage int=0, 
@IsAvailable INT=-1, -- -1: ALL, 0: InActive 1:Active   
@IsVegetarian INT=-1,
@IsBestSeller INT=-1,
@DisplayStart INT,    
@PageSize INT,    
@SortColumnName VARCHAR(50)=NULL,    
@SortOrder VARCHAR(50)='asc',    
@noOfRecords INT Out     
As    
BEGIN    
	DECLARE @QRY NVARCHAR(MAX)= '';      
    DECLARE @QRYStatus NVARCHAR(MAX)= '';    
    DECLARE @QRYWHERE NVARCHAR(MAX)= '';    
    DECLARE @PAGINATION NVARCHAR(MAX)= '';    
    DECLARE @CountRecord NVARCHAR(MAX)= '';     

    SET @QRY = 'SELECT  FoodID,
						FoodName,
						Price,
						DiscountInPercentage,
						IsJainAvailable,
						IsBestSeller,
						IsVegetarian,
						IsAvailable
                FROM(    
                    Select     
                        FoodID,
						FoodName,
						Price,
						DiscountInPercentage,
						IsJainAvailable,
						IsBestSeller,
						IsVegetarian,
						IsAvailable
                    FROM FOOD';     

    SET @QRYWHERE = ' WHERE ISNULL(IsDeleteD,0)=0 AND RestaurantID='+cast(@RestaurantID as nvarchar(MAX) ); 

	IF(@Search IS NOT NULL AND @Search<>'' AND LEN(@Search)>0)
    BEGIN    
        SET @QRYWHERE+=' AND FoodName like ''%'+COALESCE(@Search, '')+'%'''+'';    
    END;
    IF @DiscountInPercentage IS NOT NULL AND @DiscountInPercentage > 0
    BEGIN 
        SET @QRYWHERE+=' AND DiscountInPercentage=' + cast(@DiscountInPercentage as nvarchar(20))
    END   
    
    IF @IsAvailable IS NOT NULL AND @IsAvailable > -1
    BEGIN 
        SET @QRYWHERE+=' AND IsAvailable=' + cast(@IsAvailable as nvarchar(20))
    END 
	IF @IsVegetarian IS NOT NULL AND @IsVegetarian > -1
    BEGIN 
        SET @QRYWHERE+=' AND IsVegetarian=' + cast(@IsVegetarian as nvarchar(20))
    END 
	IF @IsBestSeller IS NOT NULL AND @IsBestSeller > -1
    BEGIN 
        SET @QRYWHERE+=' AND IsBestSeller=' + cast(@IsBestSeller as nvarchar(20))
    END 

    SET @QRYWHERE += ')TempTable'    
    SET @CountRecord = 'SELECT @noOfRecord=COUNT(*) FROM ('+(@QRY  + @QRYWHERE)+') countRecord';    
    SET @PAGINATION = ' ORDER BY '+@SortColumnName+' '+@SortOrder+' OFFSET '+CONVERT(VARCHAR, @DisplayStart)+' ROWS FETCH NEXT '+CONVERT(VARCHAR, @PageSize)+' ROWS ONLY'; 
    EXECUTE sp_executesql    
    @CountRecord,    
    N'@noOfRecord int OUTPUT',    
    @noOfRecord = @noOfRecords OUTPUT;    
    --PRINT (@QRY+@QRYWHERE+@PAGINATION);  
    EXEC (@QRY+@QRYWHERE+@PAGINATION);   
END


GO
/****** Object:  StoredProcedure [dbo].[restaurant_GetResataurantDetailsById]    Script Date: 16-02-2024 22:36:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Exec restaurant_GetFoodItemDetailsById 3,1
CREATE PROCEDURE [dbo].[restaurant_GetResataurantDetailsById]
@RestaurantID int
AS 
BEGIN  

	SELECT 
		RestaurantID,
		OwnerName, 
		RestaurantName, 
		MobileNo, 
		Email, 
		ShopPlotNumber,
		Floor,
		BuildingName,
		ZipCode
	FROM dbo.Restaurant r
	WHERE RestaurantID=@RestaurantID
    
         
END



GO
/****** Object:  StoredProcedure [dbo].[restaurant_RestaurantLogin]    Script Date: 16-02-2024 22:36:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC restaurant_RestaurantLogin 'test@gmail.com','123456@Aa'
CREATE PROCEDURE [dbo].[restaurant_RestaurantLogin]
@EmailOrMobileNo  NVARCHAR(100),
@Password NVARCHAR(50) 
AS
BEGIN
    DECLARE @IsRestaurantAvailable INT= 0, @MatchPassword INT= 0;
    DECLARE @Result INT =0

	SELECT @IsRestaurantAvailable = COUNT(1) FROM Restaurant WHERE (Email = @EmailOrMobileNo OR MobileNo = @EmailOrMobileNo) AND ISNULL(IsDelete,0)=0  Group By RestaurantID
		 
    IF(@IsRestaurantAvailable = 0)
    BEGIN
        SET @Result = 1; 
		SELECT @Result AS Result;
	END
	ELSE
    BEGIN 
        SELECT @MatchPassword = COUNT(1) FROM Restaurant WHERE (Email = @EmailOrMobileNo OR MobileNo = @EmailOrMobileNo) AND [Password] = @Password AND ISNULL(IsDelete,0)=0 ;

		IF(@MatchPassword = 0)
		BEGIN
			SET @Result = 4;
			SELECT @Result AS Result;
		END
		ELSE
		BEGIN
            DECLARE @IsRestaurantActive INT= (SELECT COUNT(1) FROM Restaurant WHERE (Email = @EmailOrMobileNo OR MobileNo = @EmailOrMobileNo) 
                AND [Password] = @Password AND ISNULL(IsDelete,0)=0 AND IsActive=1)

			DECLARE @RestaurantStatus INT= (SELECT RestaurantStatus FROM Restaurant WHERE (Email = @EmailOrMobileNo OR MobileNo = @EmailOrMobileNo) 
                AND [Password] = @Password AND ISNULL(IsDelete,0)=0 AND IsActive=1)

            IF(@IsRestaurantActive=1)
            BEGIN
                SET @Result = 5;
                SELECT @Result AS Result;
                SELECT 
					r.RestaurantID,
					r.OwnerName,
					r.RestaurantName,
					r.Email as Email,
					r.MobileNo as MobileNo,
					r.ShopPlotNumber +' '+r.[Floor] +' '+ r.BuildingName as Address,
					r.ZipCode,
					r.RestaurantStatus
				FROM Restaurant AS r 
				WHERE (r.Email = @EmailOrMobileNo) OR (r.MobileNo = @EmailOrMobileNo)
            END
			ELSE IF(@RestaurantStatus=1)
			BEGIN
				SET @Result = 3; 
                SELECT @Result AS Result;
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1--Pending for Approval ,2--Approved' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Restaurant', @level2type=N'COLUMN',@level2name=N'RestaurantStatus'
GO
USE [master]
GO
ALTER DATABASE [FoodDelivery] SET  READ_WRITE 
GO
