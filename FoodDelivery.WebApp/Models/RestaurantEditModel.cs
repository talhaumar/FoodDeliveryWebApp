using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.Models
{
    public class RestaurantEditModel
    {
        public Restaurant res { get; set; }
        public MenuCategory Menu_Cat { get; set; }
        public List<FoodItem> items { get; set; }
    }
}