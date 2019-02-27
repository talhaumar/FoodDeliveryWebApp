using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.WebApp.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public String PaymentTime { get; set; }

        public float Amount { get; set; }

        public int PaymentStatus { get; set; }

        public int OrderId { get; set; }

        public int SelectPaymentStatusId { get; set; }
    }
}
