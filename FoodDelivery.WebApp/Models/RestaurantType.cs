using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.WebApp.Models
{
    public class RestaurantType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Restaurant Type Name is Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Enter a valid Restaurant Type Name")]
        [RegularExpression(@"^[a-zA-Z ]*[a-zA-Z]", ErrorMessage = "Restaurant Type Name Should Contain Alphabets Only")]
        public String Name { get; set; }

        public bool IsSelected { get; set; }
    }
}
