using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.Models
{
    public class CustomerOrderViewModel
    {
        public OrderDetails orderdetails { get; set; }

        public FoodItem fooditem { get; set; }

        public Payment payment { get; set; }
        
    }
}