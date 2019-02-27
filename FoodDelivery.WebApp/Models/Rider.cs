using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.WebApp.Models
{
    public class Rider
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Enter a valid name")]
        [RegularExpression(@"^[a-zA-Z ]*[a-zA-Z]", ErrorMessage = "Your Name Should Contain Alphabets Only")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(11, ErrorMessage = "Enter a valid phone number")]
        [RegularExpression(@"\b\d{4}[-.]?\d{3}[-.]?\d{4}\b", ErrorMessage = "Enter a valid phone number")]
        public String Phone { get; set; }

        [StringLength(10, ErrorMessage = "your password should not be more than 10 characters")]
        [Required(ErrorMessage = "Password is required")]
        public String Password { get; set; }


        public int RiderStatus { get; set; }

        public String Longitude { get; set; }

        public String Latitude { get; set; }
    }
}
