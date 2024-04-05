
using System.Data;
using System.Data.SqlClient;
using FoodDelivery.Areas.Restaurant.Models;
using FoodDelivery.Areas.TiffinServices.Models;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Http;

namespace FoodDelivery.Areas.Restaurant.Models
{
    public class DatabaseRestaurant
    {
        #region Login Registration
        public RestaurantLoginResult RestaurantLogin(RestaurantLoginModel adminLoginModel)
        {
            try
            {
                RestaurantLoginResult adminLoginResult = new RestaurantLoginResult();
                LoginStatus objLoginStatus = new LoginStatus();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_RestaurantLogin, con))
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
                                adminLoginResult.RestaurantData = UserDefineExtensions.DataReaderMapToEntity<RestaurantSession>(dataReader);
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
        public string ChangeRestaurantDetail(EditRestaurantModel editRestaurantModel, string RestaurantImageName)
        {
            try
            {
                var Result = "";
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_AddEditRestaurant, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RestaurantID", editRestaurantModel.RestaurantID);
                        cmd.Parameters.AddWithValue("@OwnerName", editRestaurantModel.OwnerName);
                        cmd.Parameters.AddWithValue("@RestaurantName", editRestaurantModel.RestaurantName);
                        cmd.Parameters.AddWithValue("@MobileNo", editRestaurantModel.MobileNo);
                        cmd.Parameters.AddWithValue("@Email", editRestaurantModel.Email);
                        cmd.Parameters.AddWithValue("@ShopPlotNumber", editRestaurantModel.ShopPlotNumber);
                        cmd.Parameters.AddWithValue("@Floor", editRestaurantModel.Floor);
                        cmd.Parameters.AddWithValue("@BuildingName", editRestaurantModel.BuildingName);
                        cmd.Parameters.AddWithValue("@ZipCode", editRestaurantModel.ZipCode);
                        cmd.Parameters.AddWithValue("@RestaurantImageName", RestaurantImageName);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return Result;
            }
            catch (Exception ex) { throw; }
        }

        public string RestaurantRegistration(RestaurantRegistrationModel restaurantRegistrationModel)
        {
            try
            {
                var Result = "";
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_AddEditRestaurant, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RestaurantID", restaurantRegistrationModel.RestaurantID);
                        cmd.Parameters.AddWithValue("@OwnerName", restaurantRegistrationModel.OwnerName);
                        cmd.Parameters.AddWithValue("@RestaurantName", restaurantRegistrationModel.RestaurantName);
                        cmd.Parameters.AddWithValue("@Password", restaurantRegistrationModel.ConfirmPassword.Trim());
                        cmd.Parameters.AddWithValue("@MobileNo", restaurantRegistrationModel.MobileNo);
                        cmd.Parameters.AddWithValue("@Email", restaurantRegistrationModel.Email);
                        cmd.Parameters.AddWithValue("@ShopPlotNumber", restaurantRegistrationModel.ShopPlotNumber);
                        cmd.Parameters.AddWithValue("@Floor", restaurantRegistrationModel.Floor);
                        cmd.Parameters.AddWithValue("@BuildingName", restaurantRegistrationModel.BuildingName);
                        cmd.Parameters.AddWithValue("@ZipCode", restaurantRegistrationModel.ZipCode);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return Result;
            }
            catch (Exception ex) { throw; }
        }

        #endregion
        public bool CheckCertificateIsAllow(int RestaurantID) {
            bool IsCertificateIsAllow = false;
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_CheckCertificateIsAllow, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@RestaurantID", SqlDbType.NVarChar, 100)).Value = RestaurantID;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        IsCertificateIsAllow = Convert.ToBoolean(dr["IsCertificateIsAllow"]);
                    }
                    con.Close();
                }
            }
            return IsCertificateIsAllow;
        }
        public GetRestaurantDetailById GetRestaurantDetailsById(int RestaurantID)
        {
            GetRestaurantDetailById getRestaurantDetailById = new GetRestaurantDetailById();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_GetResataurantDetailsById, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            getRestaurantDetailById = UserDefineExtensions.DataReaderMapToEntity<GetRestaurantDetailById>(datareader);
                        }
                        con.Close();
                    }
                }
                return getRestaurantDetailById;
            }
            catch
            {
                throw;
            }
        }

        #region Food 
        public List<FoodItemListModel> GetFoodItemList(JQueryDataTableParamModel param, string Name, string DiscountInPercentage, string IsAvailable ,string IsVegetarian, string IsBestSeller, int RestaurantID,out int noOfRecords)
        {
            List<FoodItemListModel> foodItemListModel = new List<FoodItemListModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_GetFoodItemGrid, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@RestaurantID", SqlDbType.NVarChar, 100)).Value = RestaurantID;
                    cmd.Parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 100)).Value = Name;
                    cmd.Parameters.Add(new SqlParameter("@DiscountInPercentage", SqlDbType.Int)).Value = Convert.ToInt32(DiscountInPercentage);
                    cmd.Parameters.Add(new SqlParameter("@IsAvailable", SqlDbType.Int)).Value = Convert.ToInt32(IsAvailable);
                    cmd.Parameters.Add(new SqlParameter("@IsVegetarian", SqlDbType.Int)).Value = Convert.ToInt32(IsVegetarian);
                    cmd.Parameters.Add(new SqlParameter("@IsBestSeller", SqlDbType.Int)).Value = Convert.ToInt32(IsBestSeller);
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
                        foodItemListModel = UserDefineExtensions.DataReaderMapToList<FoodItemListModel>(dataReader);
                    }
                    noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                    con.Close();
                }
            }
            return foodItemListModel;
        }
        public GetFoodDetailsById GetFoodItemDetailsById(int FoodId, int RestaurantID)
        {
            GetFoodDetailsById getFoodDetailsById = new GetFoodDetailsById();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_GetFoodItemDetailsById, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FoodID", FoodId);
                        cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
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

        public AddEditFoodResponse AddEditFood(AddEditFoodViewModel addEditFoodViewModel, int RestaurantID,string FoodImageName)
        {
            try
            {
                AddEditFoodResponse addEditFoodResponse = new AddEditFoodResponse();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_AddEditFoodItem, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FoodID", addEditFoodViewModel.FoodId);
                        cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        cmd.Parameters.AddWithValue("@FoodName", addEditFoodViewModel.Name);
                        cmd.Parameters.AddWithValue("@Price", addEditFoodViewModel.Price);
                        cmd.Parameters.AddWithValue("@Ingredient", addEditFoodViewModel.Ingredient);
                        cmd.Parameters.AddWithValue("@IsJainAvailable", addEditFoodViewModel.IsJainAvailable);
                        cmd.Parameters.AddWithValue("@IsBestSeller", addEditFoodViewModel.IsBestSeller);
                        cmd.Parameters.AddWithValue("@IsVegetarian", addEditFoodViewModel.IsVegetarian);
                        cmd.Parameters.AddWithValue("@ImageName", FoodImageName);
                        cmd.Parameters.AddWithValue("@DisplayOrder", addEditFoodViewModel.DisplayOrder);
                        cmd.Parameters.AddWithValue("@IsAvailable", addEditFoodViewModel.IsAvailable);
                        cmd.Parameters.AddWithValue("@DiscountInPercentage", addEditFoodViewModel.DiscountInPercentage);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            addEditFoodResponse = UserDefineExtensions.DataReaderMapToEntity<AddEditFoodResponse>(datareader);
                        }
                        con.Close();
                    }
                    return addEditFoodResponse;
                }
            }
            catch
            {
                throw;
            }
        }

        public DeleteFoodResponse DeleteFood(int FoodItemId)
        {
            DeleteFoodResponse deleteFoodResponse=new DeleteFoodResponse();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_DeleteFood, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FoodID", FoodItemId);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            deleteFoodResponse = UserDefineExtensions.DataReaderMapToEntity<DeleteFoodResponse>(datareader);
                        }
                        con.Close();
                    }
                }
                return deleteFoodResponse;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        public List<OrderListModel> GetRestaurantOrderList(JQueryDataTableParamModel param, string Name, int RestaurantID, out int noOfRecords)
        {
            List<OrderListModel> orderListModel = new List<OrderListModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_GetRestaurantOrders, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@RestaurantID", SqlDbType.NVarChar, 100)).Value = RestaurantID;
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
                        orderListModel = UserDefineExtensions.DataReaderMapToList<OrderListModel>(dataReader);
                    }
                    noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                    con.Close();
                }
            }
            return orderListModel;
        }

        public OrderViewModel GetOrderDetailByOrderId(int OrderDetailID, int RestaurantID)
        {
            OrderViewModel getFoodDetailsById = new OrderViewModel();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_GetOrderDetailByOrderId, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);
                        cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            getFoodDetailsById = UserDefineExtensions.DataReaderMapToEntity<OrderViewModel>(datareader);
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
        public OrderStatusResponse ChangeOrderStatus(OrderViewModel orderViewModel, int RestaurantID)
        {
            try
            {
                OrderStatusResponse orderStatusResponse = new OrderStatusResponse();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_ChangeOrderStatus, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OrderDetailID", orderViewModel.OrderDetailID);
                        cmd.Parameters.AddWithValue("@OrderStatusID", orderViewModel.OrderStatus);

                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            orderStatusResponse = UserDefineExtensions.DataReaderMapToEntity<OrderStatusResponse>(datareader);
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

        public List<OrderstatusList> GetOrderstatusList()
        {
            List<OrderstatusList> OrderstatusList = new List<OrderstatusList>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.restaurant_OrderstatusList, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        OrderstatusList = UserDefineExtensions.DataReaderMapToList<OrderstatusList>(dataReader);
                    }
                    con.Close();
                }
            }
            return OrderstatusList;
        }
    }
}