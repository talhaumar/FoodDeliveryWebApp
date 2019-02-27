using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.Models
{
    public class EmailFormModel
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-z0-9][-a-z0-9.!#$%&'*+-=?^_`{|}~\/]+@([-a-z0-9]+\.)+[a-z]{2,5}", ErrorMessage = "Enter a valid email address")]
        public String Email { get; set; }
    }
}