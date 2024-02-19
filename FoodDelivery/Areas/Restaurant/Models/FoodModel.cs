using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FoodDelivery.Areas.Restaurant.Models
{
    public class FoodModel
    {
    }
    public class GetRestaurantDetailById
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
        [StringLength(200,ErrorMessage = "Name length can't be more than 200.")]
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
    public class DeleteFoodResponse
    {
        public string ImageName { get; set; }
        public bool AllowToDelete { get; set; }
    }
}
