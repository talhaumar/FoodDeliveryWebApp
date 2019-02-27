using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.WebApp.Models
{
    public class Area
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Area Name is Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Enter a Valid  Area Name")]
        [RegularExpression(@"^[a-zA-Z ]*[a-zA-Z]", ErrorMessage = "Area Name Should Contain Alphabets Only")]
        public String Name { get; set; }

        public bool IsSelected { get; set; }
    }
}
