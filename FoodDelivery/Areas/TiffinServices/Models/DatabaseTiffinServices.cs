using System.Data;
using System.Data.SqlClient;
using FoodDelivery.Areas.TiffinServices.Models;
using FoodDelivery.Models;

namespace FoodDelivery.Areas.TiffinServices.Models
{
    public class DatabaseTiffinServices
    {
        public bool CheckCertificateIsAllow(int RestaurantID)
        {
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
        #region Login Registration
        public TiffinServicesLoginResult TiffinServicesLogin(TiffinServicesLoginModel adminLoginModel)
        {
            try
            {
                TiffinServicesLoginResult adminLoginResult = new TiffinServicesLoginResult();
                LoginStatus objLoginStatus = new LoginStatus();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_TiffinServicesLogin, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmailOrMobileNo", adminLoginModel.EmailOrMobileNo);
                        cmd.Parameters.AddWithValue("@Password", adminLoginModel.Password.Trim());
                        con.Open();
                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            objLoginStatus = TiffinUserDefineExtensions.DataReaderMapToEntity<LoginStatus>(dataReader);
                            adminLoginResult.Flag = objLoginStatus.Result;
                            if (adminLoginResult.Flag == Convert.ToInt32(TiffinServicesLoginEnum.Active))
                            {
                                dataReader.NextResult();
                                adminLoginResult.TiffinServicesData = TiffinUserDefineExtensions.DataReaderMapToEntity<TiffinServicesSession>(dataReader);
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
        public string ChangeTiffinServicesDetail(EditTiffinServicesModel editTiffinServicesModel, string TiffinServicesImageName)
        {
            try
            {
                var Result = "";
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_AddEditTiffinServices, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TiffinServicesID", editTiffinServicesModel.TiffinServicesID);
                        cmd.Parameters.AddWithValue("@OwnerName", editTiffinServicesModel.OwnerName);
                        cmd.Parameters.AddWithValue("@TiffinServicesName", editTiffinServicesModel.TiffinServicesName);
                        cmd.Parameters.AddWithValue("@MobileNo", editTiffinServicesModel.MobileNo);
                        cmd.Parameters.AddWithValue("@Email", editTiffinServicesModel.Email);
                        cmd.Parameters.AddWithValue("@ShopPlotNumber", editTiffinServicesModel.ShopPlotNumber);
                        cmd.Parameters.AddWithValue("@Floor", editTiffinServicesModel.Floor);
                        cmd.Parameters.AddWithValue("@BuildingName", editTiffinServicesModel.BuildingName);
                        cmd.Parameters.AddWithValue("@ZipCode", editTiffinServicesModel.ZipCode);
                        cmd.Parameters.AddWithValue("@TiffinServicesImageName", TiffinServicesImageName);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return Result;
            }
            catch (Exception ex) { throw; }
        }

        public string TiffinServicesRegistration(TiffinServicesRegistrationModel tiffinServicesRegistrationModel)
        {
            try
            {
                var Result = "";
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_AddEditTiffinServices, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TiffinServicesID", tiffinServicesRegistrationModel.TiffinServicesID);
                        cmd.Parameters.AddWithValue("@OwnerName", tiffinServicesRegistrationModel.OwnerName);
                        cmd.Parameters.AddWithValue("@TiffinServicesName", tiffinServicesRegistrationModel.TiffinServicesName);
                        cmd.Parameters.AddWithValue("@Password", tiffinServicesRegistrationModel.ConfirmPassword.Trim());
                        cmd.Parameters.AddWithValue("@MobileNo", tiffinServicesRegistrationModel.MobileNo);
                        cmd.Parameters.AddWithValue("@Email", tiffinServicesRegistrationModel.Email);
                        cmd.Parameters.AddWithValue("@ShopPlotNumber", tiffinServicesRegistrationModel.ShopPlotNumber);
                        cmd.Parameters.AddWithValue("@Floor", tiffinServicesRegistrationModel.Floor);
                        cmd.Parameters.AddWithValue("@BuildingName", tiffinServicesRegistrationModel.BuildingName);
                        cmd.Parameters.AddWithValue("@ZipCode", tiffinServicesRegistrationModel.ZipCode);
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
        public GetTiffinServicesDetailById GetTiffinServicesDetailsById(int TiffinServicesID)
        {
            GetTiffinServicesDetailById getTiffinServicesDetailById = new GetTiffinServicesDetailById();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_GetResataurantDetailsById, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TiffinServicesID", TiffinServicesID);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            getTiffinServicesDetailById = TiffinUserDefineExtensions.DataReaderMapToEntity<GetTiffinServicesDetailById>(datareader);
                        }
                        con.Close();
                    }
                }
                return getTiffinServicesDetailById;
            }
            catch
            {
                throw;
            }
        }

        #region Food 
        public List<FoodItemListModel> GetFoodItemList(JQueryDataTableParamModel param, string Name, string DiscountInPercentage, string IsAvailable, string IsVegetarian, string IsBestSeller, int TiffinServicesID, out int noOfRecords)
        {
            List<FoodItemListModel> foodItemListModel = new List<FoodItemListModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_GetFoodItemGrid, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TiffinServicesID", SqlDbType.NVarChar, 100)).Value = TiffinServicesID;
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
                        foodItemListModel = TiffinUserDefineExtensions.DataReaderMapToList<FoodItemListModel>(dataReader);
                    }
                    noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                    con.Close();
                }
            }
            return foodItemListModel;
        }
        public GetFoodDetailsById GetFoodItemDetailsById(int FoodId, int TiffinServicesID)
        {
            GetFoodDetailsById getFoodDetailsById = new GetFoodDetailsById();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_GetFoodItemDetailsById, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FoodID", FoodId);
                        cmd.Parameters.AddWithValue("@TiffinServicesID", TiffinServicesID);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            getFoodDetailsById = TiffinUserDefineExtensions.DataReaderMapToEntity<GetFoodDetailsById>(datareader);
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

        public AddEditFoodResponse AddEditFood(AddEditFoodViewModel addEditFoodViewModel, int TiffinServicesID, string FoodImageName)
        {
            try
            {
                AddEditFoodResponse addEditFoodResponse = new AddEditFoodResponse();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_AddEditFoodItem, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FoodID", addEditFoodViewModel.FoodId);
                        cmd.Parameters.AddWithValue("@TiffinServicesID", TiffinServicesID);
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
                            addEditFoodResponse = TiffinUserDefineExtensions.DataReaderMapToEntity<AddEditFoodResponse>(datareader);
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
            DeleteFoodResponse deleteFoodResponse = new DeleteFoodResponse();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_DeleteFood, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FoodID", FoodItemId);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            deleteFoodResponse = TiffinUserDefineExtensions.DataReaderMapToEntity<DeleteFoodResponse>(datareader);
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

        public List<OrderListModel> GetTiffinServicesOrderList(JQueryDataTableParamModel param, string Name, int TiffinServicesID, out int noOfRecords)
        {
            List<OrderListModel> orderListModel = new List<OrderListModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_GettiffinServicesOrders, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TiffinServicesID", SqlDbType.NVarChar, 100)).Value = TiffinServicesID;
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
                        orderListModel = TiffinUserDefineExtensions.DataReaderMapToList<OrderListModel>(dataReader);
                    }
                    noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                    con.Close();
                }
            }
            return orderListModel;
        }

        public OrderViewModel GetOrderDetailByOrderId(int OrderDetailID, int TiffinServicesID)
        {
            OrderViewModel getFoodDetailsById = new OrderViewModel();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_GetOrderDetailByOrderId, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);
                        cmd.Parameters.AddWithValue("@TiffinServicesID", TiffinServicesID);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            getFoodDetailsById = TiffinUserDefineExtensions.DataReaderMapToEntity<OrderViewModel>(datareader);
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
        public OrderStatusResponse ChangeOrderStatus(OrderViewModel orderViewModel, int TiffinServicesID)
        {
            try
            {
                OrderStatusResponse orderStatusResponse = new OrderStatusResponse();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_ChangeOrderStatus, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OrderDetailID", orderViewModel.OrderDetailID);
                        cmd.Parameters.AddWithValue("@OrderStatusID", orderViewModel.OrderStatus);

                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            orderStatusResponse = TiffinUserDefineExtensions.DataReaderMapToEntity<OrderStatusResponse>(datareader);
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
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.tiffinServices_OrderstatusList, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        OrderstatusList = TiffinUserDefineExtensions.DataReaderMapToList<OrderstatusList>(dataReader);
                    }
                    con.Close();
                }
            }
            return OrderstatusList;
        }
    }
}
