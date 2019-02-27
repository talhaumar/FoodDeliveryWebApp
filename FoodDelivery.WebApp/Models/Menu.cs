using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.Models
{
    public class Menu
    {
        public Menu()
        {
            Menu_Cat = new MenuCategory();
            items = new List<FoodItem>();
        }
        public MenuCategory Menu_Cat { get; set; }
        public List<FoodItem> items { get; set; }
    }
}