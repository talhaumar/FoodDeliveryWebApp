using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.WebApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Enter a valid name")]
        [RegularExpression(@"^[a-zA-Z ]*[a-zA-Z]", ErrorMessage = "Your name should contain alphabets only")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Enter a subject")]
        [RegularExpression(@"^[a-zA-Z ]*[a-zA-Z]", ErrorMessage = "Subject should contain alphabets only")]
        public String Subject { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description should contain atleast 10 characters or max 2000")]
        public String Description { get; set; }

        public Nullable<int> UserId { get; set; }
    }
}
