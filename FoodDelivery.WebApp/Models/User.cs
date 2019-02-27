using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.WebApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Enter a valid name")]
        [RegularExpression(@"^[a-zA-Z ]*[a-zA-Z]+", ErrorMessage = "Your Name Should Contain Alphabets Only")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(11, ErrorMessage = "Enter a valid phone number")]
        [RegularExpression(@"\b\d{4}[-.]?\d{3}[-.]?\d{4}\b", ErrorMessage = "Enter a valid phone number")]
        public String Phone { get; set; }

        [StringLength(10, ErrorMessage = "your password should not be more than 10 characters")]
        [Required(ErrorMessage = "Password is required")]
        public String Password { get; set; }

        [RegularExpression(@"([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}", ErrorMessage = "Enter a valid email address")]
        public String Email { get; set; }

        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Address is too short")]
        [Required(ErrorMessage = "Enter your address")]
        public String Address { get; set; }


        public String SecurityQuestion { get; set; }
        
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Answer should contain atleast 5 characters")]
        public String SecurityAnswer { get; set; }

        public int RoleId { get; set; }
    }
}
