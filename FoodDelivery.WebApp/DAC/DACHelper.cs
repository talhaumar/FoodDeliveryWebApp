using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class DACHelper
    {
        internal static SqlConnection GetConnection()
        {
            //Connection string should be read from database
            return new SqlConnection(@"Data Source = (localdb)\PRFTInstance; Initial Catalog = LazyLahore; Integrated Security = True");
        }
    }
}