using System.Data.SqlClient;
using System.Data;
using FoodDelivery.Areas.Restaurant.Models;

namespace FoodDelivery.Models
{
    public class Database
    {
        public string UpdateRestaurantTrusted() {
            
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_UpdateRestaurantTrusted, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return "";
        }
        public LoginUserInfo UserLogin(LoginUserModel loginUserModel)
        {
            LoginUserInfo loginUser = new LoginUserInfo();
            UserLoginStatus userLoginStatus = new UserLoginStatus();    
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_UserLogin, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MobileNo", loginUserModel.MobileNo);
                    cmd.Parameters.AddWithValue("@Password", loginUserModel.LoginPassword.Trim());
                    con.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        userLoginStatus = UserDefineExtensions.DataReaderMapToEntity<UserLoginStatus>(dataReader);
                        loginUser.RetStatus = userLoginStatus.RetStatus;
                        if (loginUser.RetStatus == 1)
                        {
                            dataReader.NextResult();
                            loginUser.SessionUser = UserDefineExtensions.DataReaderMapToEntity<SessionUser>(dataReader);
                        }
                    }
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return loginUser;
        }

        public RegisterUser UserRegister(RegisterUserModel registerUserModel)
        {
            try
            {
                RegisterUser registerUser = new RegisterUser();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_UserRegister, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", registerUserModel.UserId);
                        cmd.Parameters.AddWithValue("@Name", registerUserModel.Name);
                        cmd.Parameters.AddWithValue("@MobileNo", registerUserModel.RegisterMobileNo);
                        cmd.Parameters.AddWithValue("@Password", registerUserModel.ConfirmPassword.Trim());
                        cmd.Parameters.AddWithValue("@Address", registerUserModel.Address);
                        con.Open();
                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            registerUser = UserDefineExtensions.DataReaderMapToEntity<RegisterUser>(dataReader);                           
                        }
                        con.Close();
                    }
                }
                return registerUser;
            }
            catch (Exception ex) { throw; }
        }
        public List<RestaurantDetailModel> GetAllRestaurantDetail()
        {
            List<RestaurantDetailModel> restaurantDetailModel = new List<RestaurantDetailModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_GetAllRestaurant, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        restaurantDetailModel = UserDefineExtensions.DataReaderMapToList<RestaurantDetailModel>(dataReader);
                    }
                    con.Close();
                }
            }
            return restaurantDetailModel;
        }
        public FoodListbyRestaurantIdMainModel GetFoodListByRestaurant(int RestaurantID)
        {
            FoodListbyRestaurantIdMainModel foodListbyRestaurantIdMainModel = new FoodListbyRestaurantIdMainModel();
            List<FoodListByRestaurantId> foodListByRestaurantId = new List<FoodListByRestaurantId>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_GetAllFoodByRestaurantID, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                    con.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        foodListByRestaurantId = UserDefineExtensions.DataReaderMapToList<FoodListByRestaurantId>(dataReader);
                    }
                    con.Close();
                }
            }
            foodListbyRestaurantIdMainModel.foodListByRestaurantId = foodListByRestaurantId;
            return foodListbyRestaurantIdMainModel;
        }
        public GetFoodDetailsById GetFoodItemDetailsById(int FoodId)
        {
            GetFoodDetailsById getFoodDetailsById = new GetFoodDetailsById();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_GetFoodItemDetailsById, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FoodID", FoodId);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            getFoodDetailsById = UserDefineExtensions.DataReaderMapToEntity<GetFoodDetailsById>(datareader);
                        }
                        con.Close();
                    }
                }
                return getFoodDetailsById;
            }
            catch
            {
                throw;
            }
        }
        #region GetCart deatils By UserId
        public CartListModel GetCartsDetailsByUserId(int UserId)
        {
            CartListModel cartListModel = new CartListModel();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_GetCartsDetailsByUserId, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = UserId;
                    con.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        cartListModel.CartDetailList = UserDefineExtensions.DataReaderMapToList<CartDetailModel>(dataReader);
                        dataReader.NextResult();
                        cartListModel.cartTotalPrice = UserDefineExtensions.DataReaderMapToEntity<CartTotalPriceModel>(dataReader);
                    }
                    con.Close();
                }
            }
            return cartListModel;
        }

        #endregion
        #region Add Design To Cart
        public RetriveDeatilFromCartModel AddFoodToCart(int UserId, int FoodId, int Qauntity)
        {
            RetriveDeatilFromCartModel retriveDeatilFromCart = new RetriveDeatilFromCartModel();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_AddFoodToCart, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = UserId;
                    cmd.Parameters.Add(new SqlParameter("@FoodID", SqlDbType.Int)).Value = FoodId;
                    cmd.Parameters.Add(new SqlParameter("@Qauntity", SqlDbType.Int)).Value = Qauntity;
                    con.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        retriveDeatilFromCart = UserDefineExtensions.DataReaderMapToEntity<RetriveDeatilFromCartModel>(dataReader);
                    }
                    con.Close();
                }
            }
            return retriveDeatilFromCart;
        }
        public int RemoveFoodFromCart(int UserId, int FoodId,bool IsDeleteFromCart)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_RemoveFoodFromCart, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = UserId;
                    cmd.Parameters.Add(new SqlParameter("@FoodId", SqlDbType.Int)).Value = FoodId;
                    cmd.Parameters.Add(new SqlParameter("@IsDeleteFromCart", SqlDbType.Bit)).Value = IsDeleteFromCart;
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        result = Convert.ToInt32(sdr["result"]);
                    }
                    con.Close();
                }
            }
            return result;
        }
        #endregion
        public AddOrderResoponse AddOrder(string FoodIds, int UserId)
        {
            AddOrderResoponse AddOrderResoponse = new AddOrderResoponse();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_AddOrder, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = UserId;
                    cmd.Parameters.Add(new SqlParameter("@FoodIds", SqlDbType.NVarChar,2000)).Value = FoodIds;
                    con.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        AddOrderResoponse = UserDefineExtensions.DataReaderMapToEntity<AddOrderResoponse>(dataReader);
                    }
                    con.Close();
                }
            }
            return AddOrderResoponse;
        }
        public List<GetUserOrderModel> GetUserOrderList(JQueryDataTableParamModel param, int UserId, out int noOfRecords)
        {
            List<GetUserOrderModel> getUserPurchaseDesignModels = new List<GetUserOrderModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_GetUserOrder, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = UserId;
                    cmd.Parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 50)).Value = param.sSearch;
                    cmd.Parameters.Add(new SqlParameter("@DisplayStart", SqlDbType.Int)).Value = param.iDisplayStart;
                    cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int)).Value = param.iDisplayLength;
                    cmd.Parameters.Add(new SqlParameter("@SortColumnName", SqlDbType.VarChar, 50)).Value = param.iSortCol_0;
                    cmd.Parameters.Add(new SqlParameter("@SortOrder", SqlDbType.VarChar, 50)).Value = param.sSortDir_0;
                    con.Open();
                    SqlParameter resultOutPut = new SqlParameter("@noOfRecords", SqlDbType.VarChar);
                    resultOutPut.Size = 50;
                    resultOutPut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultOutPut);
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        getUserPurchaseDesignModels = UserDefineExtensions.DataReaderMapToList<GetUserOrderModel>(dataReader);
                    }
                    noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                    con.Close();
                }
            }
            return getUserPurchaseDesignModels;
        }
        public SearchFoodAndRestaurant GetSearchRestaurantAndFood(string SearchBy)
        {
            SearchFoodAndRestaurant searchFoodAndRestaurant = new SearchFoodAndRestaurant();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_GetSearchData, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(SearchBy))
                    {
                        cmd.Parameters.AddWithValue("@SearchBy", SearchBy);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SearchBy", DBNull.Value);
                    }
                   
                    con.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        searchFoodAndRestaurant.searchFoodList = UserDefineExtensions.DataReaderMapToList<SearchFoodList>(dataReader);
                        dataReader.NextResult();
                        searchFoodAndRestaurant.searchRestaurantList = UserDefineExtensions.DataReaderMapToList<SearchRestaurantList>(dataReader);
                        
                    }
                    con.Close();
                }
            }
            return searchFoodAndRestaurant;
        }
        public FoodListbyRestaurantIdMainModel GetFoodListByRestaurant(bool IsVegFood, bool IsJainFood, bool IsRatingCheck, decimal PriceMinVal, decimal PriceMaxVal)
        {
            FoodListbyRestaurantIdMainModel foodListbyRestaurantIdMainModel = new FoodListbyRestaurantIdMainModel();
            List<FoodListByRestaurantId> foodListByRestaurantId = new List<FoodListByRestaurantId>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_GetFoodList, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IsVegFood", SqlDbType.Bit)).Value = IsVegFood;
                    cmd.Parameters.Add(new SqlParameter("@IsJainFood", SqlDbType.Bit)).Value = IsJainFood;
                    cmd.Parameters.Add(new SqlParameter("@IsRatingCheck", SqlDbType.Bit)).Value = IsRatingCheck;
                    cmd.Parameters.Add(new SqlParameter("@PriceMinVal", SqlDbType.Decimal)).Value = PriceMinVal;
                    cmd.Parameters.Add(new SqlParameter("@PriceMaxVal", SqlDbType.Decimal)).Value = PriceMaxVal;
                    con.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        foodListByRestaurantId = UserDefineExtensions.DataReaderMapToList<FoodListByRestaurantId>(dataReader);
                    }
                    con.Close();
                }
            }
            foodListbyRestaurantIdMainModel.foodListByRestaurantId = foodListByRestaurantId;
            return foodListbyRestaurantIdMainModel;
        }
        public RateFoodResoponse RateToFood(int OrderDetailId, string Rate, int UserId)
        {
            RateFoodResoponse rateFoodResoponse = new RateFoodResoponse();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_RateToFood, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@OrderDetailId", SqlDbType.Int)).Value = OrderDetailId;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = UserId;
                    cmd.Parameters.Add(new SqlParameter("@Rate", SqlDbType.Decimal)).Value =Convert.ToDecimal(Rate);
                    con.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        rateFoodResoponse = UserDefineExtensions.DataReaderMapToEntity<RateFoodResoponse>(dataReader);
                    }
                    con.Close();
                }
            }
            return rateFoodResoponse;
        }
    }
}
