using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.WebApp.Models
{
    public class RiderArea
    {
        public int Id { get; set; }

        public Rider RiderId { get; set; }

        public Area AreaId { get; set; }
    }
}
