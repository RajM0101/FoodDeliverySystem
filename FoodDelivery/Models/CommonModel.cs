using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models
{
    public class RestaurantRegistrationModel
    {
        public int RestaurantID { get; set; }
        [Required(ErrorMessage = "Please enter Owner's name")]
        public string OwnerName { get; set; }
        [Required(ErrorMessage = "Please enter restaurant name")]
        public string RestaurantName { get; set; }
        [Display(Name = "Mobile number")]
        [Required(ErrorMessage = "Please enter mobile number.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Mobile number")]
        [MinLength(4, ErrorMessage = "Enter minimum 4 digit Mobile number.")]
        public string MobileNo { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email address.")]
        [Required(ErrorMessage = "Please enter email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        [StringLength(16, ErrorMessage = "Please enter value between 8 to 16 character.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.{8,32})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$", ErrorMessage = "Minimum 8 characters at least 1 Uppercase Alphabet,1 Lowercase Alphabet,1 Number and 1 Special character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter confirm password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Please enter confirm password.")]
        public string ShopPlotNumber { get; set; }
        public string Floor { get; set; }
        public string BuildingName { get; set; }
        public string ZipCode { get; set; }

    }
    public class RestaurantLoginModel
    {

        [Required(ErrorMessage = "Please enter email or mobile number")]
        public string EmailOrMobileNo { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        [StringLength(16, ErrorMessage = "Please enter value between 8 to 16 character.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.{8,32})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$", ErrorMessage = "Minimum 8 characters at least 1 Uppercase Alphabet,1 Lowercase Alphabet,1 Number and 1 Special character.")]
        public string Password { get; set; }
    }
    public class RestaurantLoginResult
    {
        public RestaurantSession RestaurantData { get; set; }
        public int Flag { get; set; }
    }
    public class RestaurantSession
    {
        public int RestaurantID { get; set; }
        public string OwnerName { get; set; }
        public string RestaurantName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public int RestaurantStatus { get; set; }
    }

    public class LoginStatus
    {
        public int Result { get; set; }
    }
    public enum RestaurantLoginEnum
    {
        UserNotAvailable = 1,
        ContactAdmin = 3,
        PasswordMissmatch = 4,
        Active = 5
    }
    public class RestaurantAllowedIPAddress
    {
        public string IPAddress { get; set; }
    }
    #region Data Table parameter
    public class JQueryDataTableParamModel
    {
        public string draw { get; set; }
        public string sEcho { get; set; }
        public string sSearch { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iColumns { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; }
        public string iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }
        public int UserId { get; set; }
    }
    #endregion
    public class CountryModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

    }
    public class LoginRegisterModel
    {
        public LoginUserModel loginUserModel { get; set; } = new LoginUserModel();
        public RegisterUserModel registerUserModel { get; set; } = new RegisterUserModel();
    }
    public class LoginUserModel
    {
        [Display(Name = "Mobile number")]
        [Required(ErrorMessage = "Please enter mobile number")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Mobile Number")]
        [MinLength(4, ErrorMessage = "Enter minimum 4 digit Mobile number.")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Please enter password.")]
        public string LoginPassword { get; set; }      
    }

    public class RegisterUserModel
    {
        public int UserId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is Required.")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Invalid Name, it should be alphabetical only")]
        [StringLength(50, ErrorMessage = "Max 50 characters are allowed.", MinimumLength = 1)]
        public string Name { get; set; }
        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Mobile Number")]
        [MinLength(4, ErrorMessage = "Enter minimum 4 digit Mobile Number.")]
        public string RegisterMobileNo { get; set; }
        [Required(ErrorMessage = "Please enter password.")]
        [StringLength(16, ErrorMessage = "Please enter value between 8 to 16 character.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.{8,32})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$", ErrorMessage = "Minimum 8 characters at least 1 Uppercase Alphabet,1 Lowercase Alphabet,1 Number and 1 Special character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter confirm password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public bool TermsCondition { get; set; }
    }
    public class RegisterUser
    {
        public int RetStatus { get; set; }
        public int UserId { get; set; }

    }
    public class SessionUser
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
    }
    public class LoginUserInfo
    {
        public int RetStatus { get; set; }
        public SessionUser SessionUser { get; set; }

    }
    public class UserLoginStatus
    {
        public int RetStatus { get; set; }
    }
    public class DashboardMainModel {
       public List<RestaurantDetailModel> restaurantDetailModel { get; set; } = new List<RestaurantDetailModel>();
    }
    public class RestaurantDetailModel { 
        public int RestaurantID { get; set; }
        public string RestaurantName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string ImageName { get; set; }
    }
    public class CommonModel
    {
    }
}
