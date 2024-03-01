namespace FoodDelivery.Models
{
    public static class Common
    {
        public static string SiteURL { get; set; }
        public static string SiteCDNBaseURL { get; set; }
        public static string DBConnectionString { get; set; }

        public static string SiteLogo = string.Empty;
        public static class SessionKeys
        {
            public const string RestaurantSession = "RestaurantSession";
            public const string UserSession = "UserSession";
            public const string AdminSession = "AdminSession";
        }
        public static class StoredProcedureNames
        {
            #region Admin 
            public const string restaurant_RestaurantLogin = "restaurant_RestaurantLogin";
            public const string restaurant_AddEditRestaurant = "restaurant_AddEditRestaurant";
            public const string restaurant_GetFoodItemDetailsById = "restaurant_GetFoodItemDetailsById";
            public const string restaurant_AddEditFoodItem = "restaurant_AddEditFoodItem";
            public const string restaurant_GetFoodItemGrid = "restaurant_GetFoodItemGrid";
            public const string restaurant_GetResataurantDetailsById = "restaurant_GetResataurantDetailsById";
            public const string restaurant_DeleteFood = "restaurant_DeleteFood";
            public const string web_RemoveFoodFromCart = "web_RemoveFoodFromCart";
            #endregion
            #region User
            public const string web_UserLogin = "web_UserLogin";
            public const string web_UserRegister = "web_UserRegister";
            public const string web_GetAllRestaurant = "web_GetAllRestaurant";
            public const string web_GetAllFoodByRestaurantID = "web_GetAllFoodByRestaurantID";
            public const string web_GetFoodItemDetailsById = "web_GetFoodItemDetailsById";
            public const string web_GetCartsDetailsByUserId = "web_GetCartsDetailsByUserId";
            public const string web_AddFoodToCart = "web_AddFoodToCart";
            public const string web_AddOrder = "web_AddOrder";
            public const string web_GetUserOrder = "web_GetUserOrder";
            #endregion
        }
        public static class Messages
        {
            #region Admin
            public const string LoginSuccess = "Login Successfully";
            public const string UserNotAvailable = "User Name or Email does not exists";
            public const string IncorrectPassword = "Incorrect password";
            public const string RestaurantIsNotActive = "Your restaurant is currently not active, Kindly contact admin or email on support email address.";
            public const string LoginFailed = "Failed to login";

            #endregion
            #region User
            public const string CustomerLoginSucessfully = "Login Successfully";
            public const string CustomerNotAvailable = "Mobile or Password does not exists";
            public const string CustomerPasswordIncorrect = "Password is incorrect";
            public const string CustomerIsNotActive = "Customer is not active";
            public const string UserAlreadyRegistered = "You are already registered, please try to login.";
            #endregion

        }

    }
}
