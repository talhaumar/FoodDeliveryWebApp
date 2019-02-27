using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.Models
{
    public class OrderViewModel
    {
        public Order order { get; set; }

        public Rider rider { get; set; }

        public List<Area> rAreas { get; set; }

        public Payment payment { get; set; }

    }
}