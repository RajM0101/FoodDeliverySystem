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

        }

    }
}
