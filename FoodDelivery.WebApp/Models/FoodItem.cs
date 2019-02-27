using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.WebApp.Models
{
    public class FoodItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Food Item Name is Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Enter a Valid  Food Item Name")]
        [RegularExpression(@"^[a-zA-Z ]*[a-zA-Z]", ErrorMessage = "Food Item Name Should Contain Alphabets Only")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Food Item Price is Required")]
        [Range(50,3000,ErrorMessage="Price Range is 50 - 3000")]
        [RegularExpression(@"^[0-9]\d*", ErrorMessage = "Enter a Valid Price")]
        public int Price { get; set; }

        [RegularExpression(@"^[0-9]\d*(\.\d+)?", ErrorMessage = "Enter a Valid Discount")]
        public float Discount { get; set; }

        [StringLength(500, MinimumLength = 5, ErrorMessage = "Description Should Contain Atleast 10 Characters")]
        [RegularExpression(@"^[A-Za-z ]+(?:[,][ A-Za-z]+)*[A-Za-z]", ErrorMessage = "Description Should Contain Alphabets Following Comma(if needed)")]
        public String Description { get; set; }

        public int MenuCatId { get; set; }

        [Required(ErrorMessage = "Select a Category")]
        public int SelectedCategoryId { get; set; }

    }
}
