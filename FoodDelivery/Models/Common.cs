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
            public const string TiffinServicesSession = "TiffinServicesSession";
        }
        public static class StoredProcedureNames
        {
            #region Admin
            public const string admin_ChangeRestaurantStatus = "admin_ChangeRestaurantStatus";
            public const string admin_GetRestaurantList = "admin_GetRestaurantList";
            public const string admin_GetTiffinServicesList = "admin_GetTiffinServicesList";
            public const string admin_GetRestaurantDetailByRestaurantId = "admin_GetRestaurantDetailByRestaurantId";
            public const string admin_AdminUserLogin = "admin_AdminUserLogin";
            public const string admin_GetUserList = "admin_GetUserList";
            public const string admin_GetAllFoodByRestaurantID = "admin_GetAllFoodByRestaurantID";
            #endregion
            #region Restaurant 

            public const string restaurant_RestaurantLogin = "restaurant_RestaurantLogin";
            public const string restaurant_AddEditRestaurant = "restaurant_AddEditRestaurant";
            public const string restaurant_GetFoodItemDetailsById = "restaurant_GetFoodItemDetailsById";
            public const string restaurant_AddEditFoodItem = "restaurant_AddEditFoodItem";
            public const string restaurant_GetFoodItemGrid = "restaurant_GetFoodItemGrid";
            public const string restaurant_GetResataurantDetailsById = "restaurant_GetResataurantDetailsById";
            public const string restaurant_DeleteFood = "restaurant_DeleteFood";
            public const string web_RemoveFoodFromCart = "web_RemoveFoodFromCart";
            public const string restaurant_GetRestaurantOrders = "restaurant_GetRestaurantOrders";
            public const string restaurant_GetOrderDetailByOrderId = "restaurant_GetOrderDetailByOrderId";
            public const string restaurant_OrderstatusList = "restaurant_OrderstatusList";
            public const string restaurant_ChangeOrderStatus = "restaurant_ChangeOrderStatus";
            public const string restaurant_CheckCertificateIsAllow = "restaurant_CheckCertificateIsAllow";
            #endregion
            #region TiffinServices 

            public const string tiffinServices_TiffinServicesLogin = "tiffinServices_TiffinServicesLogin";
            public const string tiffinServices_OrderstatusList = "tiffinServices_OrderstatusList";
            public const string tiffinServices_GettiffinServicesOrders = "tiffinServices_GettiffinServicesOrders";
            public const string tiffinServices_GetResataurantDetailsById = "tiffinServices_GetResataurantDetailsById";
            public const string tiffinServices_GetOrderDetailByOrderId = "tiffinServices_GetOrderDetailByOrderId";
            public const string tiffinServices_GetFoodItemGrid = "tiffinServices_GetFoodItemGrid";
            public const string tiffinServices_GetFoodItemDetailsById = "tiffinServices_GetFoodItemDetailsById";
            public const string tiffinServices_DeleteFood = "tiffinServices_DeleteFood";
            public const string tiffinServices_ChangeOrderStatus = "tiffinServices_ChangeOrderStatus";
            public const string tiffinServices_AddEditFoodItem = "tiffinServices_AddEditFoodItem";
            public const string tiffinServices_AddEditTiffinServices = "tiffinServices_AddEditTiffinServices";

            #endregion
            #region User
            public const string web_UpdateRestaurantTrusted = "web_UpdateRestaurantTrusted";
            public const string web_UserLogin = "web_UserLogin";
            public const string web_UserRegister = "web_UserRegister";
            public const string web_GetAllRestaurant = "web_GetAllRestaurant";
            public const string web_GetAllFoodByRestaurantID = "web_GetAllFoodByRestaurantID";
            public const string web_GetFoodItemDetailsById = "web_GetFoodItemDetailsById";
            public const string web_GetCartsDetailsByUserId = "web_GetCartsDetailsByUserId";
            public const string web_AddFoodToCart = "web_AddFoodToCart";
            public const string web_AddOrder = "web_AddOrder";
            public const string web_GetUserOrder = "web_GetUserOrder";
            public const string web_GetSearchData = "web_GetSearchData";
            public const string web_GetFoodList = "web_GetFoodList";
            public const string web_RateToFood = "web_RateToFood";
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
            public const string NotApproved = "You are currently not approved, Kindly contact on support email address.";

            public const string AdminUserIsNotActive = "You are currently not active, Kindly contact on support email address.";
            public const string TiffinServicesIsNotActive = "Your tiffin services is currently not active, Kindly contact admin or email on support email address.";
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
