using System.Data.SqlClient;
using System.Data;
using FoodDelivery.Areas.Restaurant.Models;

namespace FoodDelivery.Models
{
    public class Database
    {
        public List<CountryList> GetCountrylist()
        {
            List<CountryList> countryListModels = new List<CountryList>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.web_GetCountryCodeList, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        countryListModels = UserDefineExtensions.DataReaderMapToList<CountryList>(dataReader);
                    }
                    con.Close();
                    return countryListModels;
                }
            }
        }
    }
}
