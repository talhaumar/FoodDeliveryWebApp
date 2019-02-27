using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.Models
{
    public class RiderModel
    {
        public RiderModel()
        {
            rider = new Rider();
            arealist = new List<Area>();
            riderlist = new List<Rider>();
        }

        public Rider rider { get; set; }

        public List<Rider> riderlist { get; set; }
        
        public List<Area> arealist { get; set; }
    }
}