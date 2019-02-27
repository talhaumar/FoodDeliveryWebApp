using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.WebApp.Models
{
    public class Order
    {

        public int Id { get; set; }

        public String StreetAddress { get; set; }

        public DateTime OrderDateTime { get; set; }
        public String Phone { get; set; }



        public String CookTime { get; set; }



        public int DeliveryCharges { get; set; }

        public Nullable<int> UserId { get; set; }

        public int OrderStatusId { get; set; }

        public Nullable<int> RiderId { get; set; }

        public int ResId { get; set; }


    }
}
