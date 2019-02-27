using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodDelivery.WebApp.Models
{
    public class SignupModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Enter a valid name")]
        [RegularExpression(@"^[a-zA-Z ]*[a-zA-Z]+", ErrorMessage = "Your name should contain alphabets only")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(11, ErrorMessage = "Enter a valid phone number")]
        [RegularExpression(@"\b\d{4}[-.]?\d{3}[-.]?\d{4}\b", ErrorMessage = "Enter a valid phone number")]
        [Remote("isPhoneNumberAvailable", "Home", ErrorMessage = "You are already registered, login to continue")]
        public String Phone { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50 , MinimumLength = 6, ErrorMessage = "Your password is too short")]
        public String Password { get; set; }

        [RegularExpression(@"^[a-z0-9][-a-z0-9.!#$%&'*+-=?^_`{|}~\/]+@([-a-z0-9]+\.)+[a-z]{2,5}", ErrorMessage = "Enter a valid email address")]
        public String Email { get; set; }

        //[Required(ErrorMessage = "Enter your address")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Address is too short")]
        public String Address { get; set; }

        public int RoleId { get; set; }
    }
}