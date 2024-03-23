namespace FoodDelivery.Areas.AdminUser.Models
{
    public class RestaurantListModel
    {
        public int RestaurantID { get; set; }
        public string OwnerName { get; set; }
        public string RestaurantName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public bool RestaurantStatus { get; set; }
    }
    public class RestaurantViewModel
    {
        public int RestaurantID { get; set; }
        public string OwnerName { get; set; }
        public string RestaurantName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string ShopPlotNumber { get; set; }
        public string Floor { get; set; }
        public string BuildingName { get; set; }
        public string ZipCode { get; set; }
        public bool RestaurantStatus { get; set; }
        public string RestaurantImageName { get; set; }
    }
    public class RestaurantStatusResponse
    {
        public int status { get; set;}
    }
    public class UserListModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
    }
    public class FoodListModel
    {
        public int RestaurantID { get; set; }
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public int Price { get; set; }
        public string Ingredient { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsVegetarian { get; set; }
        public decimal Rate { get; set; }
    }
    public class TiffinServicesListModel
    {
        public int TiffinServicesID { get; set; }
        public string OwnerName { get; set; }
        public string TiffinServicesName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public bool TiffinServicesStatus { get; set; }
    }
}
