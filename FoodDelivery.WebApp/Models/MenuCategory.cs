using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.WebApp.Models
{
    public class MenuCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is Required")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "Enter a valid  category name")]
        [RegularExpression(@"^[a-zA-Z ]*[a-zA-Z]", ErrorMessage = "Category name should contain alphabets only")]
        public String Name { get; set; }

        public int ResId { get; set; }
    }
}
