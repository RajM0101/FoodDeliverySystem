using System.Data.SqlClient;
using System.Data;
using FoodDelivery.Areas.Restaurant.Models;

namespace FoodDelivery.Models
{
    public class Database
    {
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
    }
}
