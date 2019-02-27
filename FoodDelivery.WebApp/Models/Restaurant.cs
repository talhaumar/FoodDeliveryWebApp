using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FoodDelivery.WebApp.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Enter a valid name")]
        [RegularExpression(@"^[a-zA-Z ]*[a-zA-Z]+", ErrorMessage = "Your name should contain alphabets only")]
        public String Name { get; set; }

        [StringLength(11, ErrorMessage = "Enter a valid phone number")]
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"\b\d{4}[-.]?\d{3}[-.]?\d{4}\b", ErrorMessage = "Enter a valid phone number")]
        [Remote("isPhoneNumberAvailable", "Restaurant", ErrorMessage = "Phone Number Already Available")]
        public String Phone { get; set; }

        [RegularExpression(@"([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}", ErrorMessage = "Enter a valid email address")]
        public String Email { get; set; }

        [StringLength(2000,MinimumLength=5,ErrorMessage="Address is too short")]
        [Required(ErrorMessage="Enter your address")]
        public String Address { get; set; }

        [StringLength(10, ErrorMessage = "your password should not be more than 10 characters")]
        [Required(ErrorMessage="Password is required")]
        public String Password { get; set; }

        [RegularExpression(@"^[0-9]\d*(\.\d+)?", ErrorMessage = "Enter a valid Tax value")]
        public float ?Tax { get; set; }

        [RegularExpression(@"^(0?[1-9]|1[012])(:[0-5]\d)[APap][mM]", ErrorMessage = "Select from the given time")]
        public String OpeningTime { get; set; }

        [RegularExpression(@"^(0?[1-9]|1[012])(:[0-5]\d)[APap][mM]", ErrorMessage = "Select from the given time")]
        public String ClosingTime { get; set; }


        [Required(ErrorMessage = "Area is required")]
        public String Area { get; set; }

        [StringLength(2000, MinimumLength=10, ErrorMessage="Review should contain atleast 10 characters")]
        public String Review { get; set; }

        public String ImageURL { get; set; }
    }
}
