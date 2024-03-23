using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FoodDelivery.Areas.TiffinServices.Models
{
        public class FoodModel
        {
        }
        public class EditTiffinServicesModel
        {
            public int TiffinServicesID { get; set; }
            [Required(ErrorMessage = "Please enter Owner's name")]
            public string OwnerName { get; set; }
            [Required(ErrorMessage = "Please enter tiffin services name")]
            public string TiffinServicesName { get; set; }
            [Display(Name = "Mobile number")]
            [Required(ErrorMessage = "Please enter mobile number.")]
            [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Mobile number")]
            [MinLength(4, ErrorMessage = "Enter minimum 4 digit Mobile number.")]
            public string MobileNo { get; set; }
            [EmailAddress(ErrorMessage = "Invalid Email address.")]
            [Required(ErrorMessage = "Please enter email")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Please enter confirm password.")]
            public string ShopPlotNumber { get; set; }
            public string Floor { get; set; }
            public string BuildingName { get; set; }
            public string ZipCode { get; set; }
            [Required(ErrorMessage = "Please add image of tiffin service")]
            [Display(Name = "Image")]
            public List<byte[]> ImageName { get; set; }
            public string TiffinServicesImageName { get; set; }
            public string imgX1 { get; set; } = "1";
            public string imgY1 { get; set; } = "1";
            public string imgWidth { get; set; } = "1";
            public string imgHeight { get; set; } = "1";

        }
        public class GetTiffinServicesDetailById
        {
            public int TiffinServicesID { get; set; }
            public string OwnerName { get; set; }
            public string TiffinServicesName { get; set; }
            public string MobileNo { get; set; }
            public string Email { get; set; }
            public string ShopPlotNumber { get; set; }
            public string Floor { get; set; }
            public string BuildingName { get; set; }
            public string ZipCode { get; set; }
            public string TiffinServicesImageName { get; set; }
        }
        public class FoodItemListModel
        {
            public int FoodID { get; set; }
            public string FoodName { get; set; }
            public int Price { get; set; }
            public int DiscountInPercentage { get; set; }
            public bool IsJainAvailable { get; set; }
            public bool IsBestSeller { get; set; }
            public bool IsVegetarian { get; set; }
            public bool IsAvailable { get; set; }
        }
        public class GetFoodDetailsById
        {
            public int FoodID { get; set; }
            public string FoodName { get; set; }
            public int Price { get; set; }
            public string Ingredient { get; set; }
            public bool IsJainAvailable { get; set; }
            public bool IsBestSeller { get; set; }
            public bool IsVegetarian { get; set; }
            public string FoodImageName { get; set; }
            public int DisplayOrder { get; set; }
            public bool IsAvailable { get; set; }
            public int DiscountInPercentage { get; set; }
        }
        public class AddEditFoodViewModel
        {
            public int FoodId { get; set; }

            [Required(ErrorMessage = "Please enter name")]
            [StringLength(200, ErrorMessage = "Name length can't be more than 200.")]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Please enter price of food")]
            [Display(Name = "Price")]
            public int Price { get; set; }

            [Required(ErrorMessage = "Please enter ingredient of food")]
            [Display(Name = "Ingrediants")]
            public string Ingredient { get; set; }
            [Display(Name = "Is Jain Available")]
            public bool IsJainAvailable { get; set; }
            [Display(Name = "Is Best Seller")]
            public bool IsBestSeller { get; set; }
            public bool IsVegetarian { get; set; }
            [Required(ErrorMessage = "Please add image of food")]
            [Display(Name = "Image")]
            public List<byte[]> ImageName { get; set; }
            [Required(ErrorMessage = "Please enetr display order.")]
            [Display(Name = "Display Order")]
            public int DisplayOrder { get; set; }
            public bool IsAvailable { get; set; }

            [Required(ErrorMessage = "Please enter Dicount")]
            [Display(Name = "Discount In Percenatage")]
            public int DiscountInPercentage { get; set; }
            public string FoodImageName { get; set; }
            public string imgX1 { get; set; } = "1";
            public string imgY1 { get; set; } = "1";
            public string imgWidth { get; set; } = "1";
            public string imgHeight { get; set; } = "1";
        }
        public class AddEditFoodResponse
        {
            public int status { get; set; }
            public int FoodItemID { get; set; }
        }
        public class OrderListModel
        {
            public int OrderId { get; set; }
            public int OrderDetailID { get; set; }
            public int FoodId { get; set; }
            public string Name { get; set; }
            public int Qauntity { get; set; }
            public int Price { get; set; }
            public string OrderStatus { get; set; }
            public string OrderDate { get; set; }
        }

        public class OrderViewModel
        {
            public int OrderID { get; set; }
            public int OrderDetailID { get; set; }
            public int FoodId { get; set; }
            [Display(Name = "Name")]
            public string Name { get; set; }
            public int Price { get; set; }
            public int Qauntity { get; set; }
            public int TotalPrice { get; set; }
            public int OrderStatus { get; set; }
        }
        public class OrderStatusResponse
        {
            public int status { get; set; }
        }
        public class OrderstatusList
        {
            public int OrderStatusID { get; set; }
            public string OrderStatusName { get; set; }
        }
        public class DeleteFoodResponse
        {
            public string ImageName { get; set; }
            public bool AllowToDelete { get; set; }
        }
        public class SearchFoodList
        {
            public int FoodID { get; set; }
            public string FoodName { get; set; }
            public int Price { get; set; }
            public decimal Rate { get; set; }
            public string Ingredient { get; set; }
            public string ImageName { get; set; }
        }
        public class SearchTiffinServicesList
        {
            public int TiffinServicesID { get; set; }
            public string TiffinServicesName { get; set; }
            public string ImageName { get; set; }
        }
        public class SearchFoodAndTiffinServices
        {
            public List<SearchTiffinServicesList> searchTiffinServicesList { get; set; }
            public List<SearchFoodList> searchFoodList { get; set; }
        }
}
