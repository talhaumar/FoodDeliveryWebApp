using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodDelivery.WebApp.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(11, ErrorMessage = "Enter a valid phone number")]
        [RegularExpression(@"\b\d{4}[-.]?\d{3}[-.]?\d{4}\b", ErrorMessage = "Enter a valid phone number")]
        [Remote("doPhoneNumberExist", "Home", ErrorMessage = "You are not registered signup to continue")]
        public String Phone { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Your password is too short")]
        public String Password { get; set; }
    }
}