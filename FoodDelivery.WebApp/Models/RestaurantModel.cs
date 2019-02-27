using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.Models
{
    public class RestaurantModel
    {
        public RestaurantModel()
        {
            RestaurantType = new List<RestaurantType>();
            restaurantlist = new List<Restaurant>();
            restaurant = new Restaurant();
        }

        public Restaurant restaurant { get; set; }

        public List<Restaurant> restaurantlist { get; set; }
        
        public List<RestaurantType> RestaurantType { get; set; }
    }
}