using System.Data;
using System.Data.SqlClient;
using FoodDelivery.Models;
using FoodDelivery.Areas.Restaurant.Models;

namespace FoodDelivery.Areas.AdminUser.Models
{
    public class DatabaseAdminUser
    {
        public List<RestaurantListModel> GetRestaurantList(JQueryDataTableParamModel param, string Name, out int noOfRecords)
        {
            List<RestaurantListModel> restaurantListModel = new List<RestaurantListModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.admin_GetRestaurantList, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 100)).Value = Name;
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
                        restaurantListModel = UserDefineExtensions.DataReaderMapToList<RestaurantListModel>(dataReader);
                    }
                    noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                    con.Close();
                }
            }
            return restaurantListModel;
        }
        public List<TiffinServicesListModel> GetTiffinServicesList(JQueryDataTableParamModel param, string Name, out int noOfRecords)
        {
            List<TiffinServicesListModel> tiffinServicesListModel = new List<TiffinServicesListModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.admin_GetTiffinServicesList, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 100)).Value = Name;
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
                        tiffinServicesListModel = UserDefineExtensions.DataReaderMapToList<TiffinServicesListModel>(dataReader);
                    }
                    noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                    con.Close();
                }
            }
            return tiffinServicesListModel;
        }

        public RestaurantViewModel GetRestaurantDetailByRestaurantId(int RestaurantID)
        {
            RestaurantViewModel restaurantViewModel = new RestaurantViewModel();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.admin_GetRestaurantDetailByRestaurantId, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            restaurantViewModel = UserDefineExtensions.DataReaderMapToEntity<RestaurantViewModel>(datareader);
                        }
                        con.Close();
                    }
                }
                return restaurantViewModel;
            }
            catch
            {
                throw;
            }
        }
        public RestaurantStatusResponse ChangeOrderStatus(RestaurantViewModel restaurantViewModel, int RestaurantID)
        {
            try
            {
                RestaurantStatusResponse orderStatusResponse = new RestaurantStatusResponse();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.admin_ChangeRestaurantStatus, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RestaurantID", restaurantViewModel.RestaurantID);
                        cmd.Parameters.AddWithValue("@RestaurantStatus", restaurantViewModel.RestaurantStatus);

                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            orderStatusResponse = UserDefineExtensions.DataReaderMapToEntity<RestaurantStatusResponse>(datareader);
                        }
                        con.Close();
                    }
                    return orderStatusResponse;
                }
            }
            catch
            {
                throw;
            }
        }
        public AdminLoginResult AdminUserLogin(AdminUserLoginModel adminLoginModel)
        {
            try
            {
                AdminLoginResult adminLoginResult = new AdminLoginResult();
                LoginStatus objLoginStatus = new LoginStatus();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.admin_AdminUserLogin, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmailOrMobileNo", adminLoginModel.EmailOrMobileNo);
                        cmd.Parameters.AddWithValue("@Password", adminLoginModel.Password.Trim());
                        con.Open();
                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            objLoginStatus = UserDefineExtensions.DataReaderMapToEntity<LoginStatus>(dataReader);
                            adminLoginResult.Flag = objLoginStatus.Result;
                            if (adminLoginResult.Flag == Convert.ToInt32(RestaurantLoginEnum.Active))
                            {
                                dataReader.NextResult();
                                adminLoginResult.AdminUserData = UserDefineExtensions.DataReaderMapToEntity<AdminUserSession>(dataReader);
                            }
                        }
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return adminLoginResult;
            }
            catch (Exception ex) { throw; }
        }

        public List<UserListModel> GetUserList(JQueryDataTableParamModel param, string Name, out int noOfRecords)
        {
            List<UserListModel> userListModel = new List<UserListModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.admin_GetUserList, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 100)).Value = Name;
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
                        userListModel = UserDefineExtensions.DataReaderMapToList<UserListModel>(dataReader);
                    }
                    noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                    con.Close();
                }
            }
            return userListModel;
        }

        public List<FoodListModel> GetFoodList(JQueryDataTableParamModel param, string RestaurantID, string Name, out int noOfRecords)
        {
            List<FoodListModel> foodListModel = new List<FoodListModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.admin_GetAllFoodByRestaurantID, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 100)).Value = Name;
                    cmd.Parameters.Add(new SqlParameter("@DisplayStart", SqlDbType.Int)).Value = param.iDisplayStart;
                    cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int)).Value = param.iDisplayLength;
                    cmd.Parameters.Add(new SqlParameter("@SortColumnName", SqlDbType.VarChar, 50)).Value = param.iSortCol_0;
                    cmd.Parameters.Add(new SqlParameter("@SortOrder", SqlDbType.VarChar, 50)).Value = param.sSortDir_0;
                    cmd.Parameters.Add(new SqlParameter("@RestaurantID", SqlDbType.Int)).Value = Convert.ToInt32(RestaurantID);
                    con.Open();
                    SqlParameter resultOutPut = new SqlParameter("@noOfRecords", SqlDbType.VarChar);
                    resultOutPut.Size = 50;
                    resultOutPut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultOutPut);
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        foodListModel = UserDefineExtensions.DataReaderMapToList<FoodListModel>(dataReader);
                    }
                    noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                    con.Close();
                }
            }
            return foodListModel;
        }
        public RestaurantViewModel GetTiffinServicesDetailByOrderId(int TiffinServicesID)
        {
            RestaurantViewModel restaurantViewModel = new RestaurantViewModel();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.admin_GetRestaurantDetailByRestaurantId, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RestaurantID", TiffinServicesID);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            restaurantViewModel = UserDefineExtensions.DataReaderMapToEntity<RestaurantViewModel>(datareader);
                        }
                        con.Close();
                    }
                }
                return restaurantViewModel;
            }
            catch
            {
                throw;
            }
        }
    }
}
