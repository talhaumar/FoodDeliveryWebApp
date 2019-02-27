using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class OrderStatusDAC
    {
        private readonly string SELECT_BY_ID = "Select * from OrderStatus where Id=@Id";
        private readonly string SELECT_All_Status = "Select * from OrderStatus";


        public OrderStatus SelectById(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<OrderStatus> temp = fetchStatus(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<OrderStatus> SelectAll()
        {
            SqlCommand cmd = new SqlCommand(SELECT_All_Status, DACHelper.GetConnection());
            List<OrderStatus> temp = fetchStatus(cmd);
            return temp;
        }
        private List<OrderStatus> fetchStatus(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<OrderStatus> olist = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    olist = new List<OrderStatus>();
                    while (dr.Read())
                    {
                        OrderStatus os = new OrderStatus();
                        os.Id = Convert.ToInt32(dr["Id"]);
                        os.Name = Convert.ToString(dr["Name"]);
                        olist.Add(os);
                    }
                    olist.TrimExcess();
                }
            }
            return olist;
        }
    }
}