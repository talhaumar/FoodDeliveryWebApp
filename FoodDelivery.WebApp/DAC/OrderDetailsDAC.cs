using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class OrderDetailsDAC
    {
        private readonly string SELECT_ALL_Order_Details = "Select * from OrderDetails where FK_OrderId = @FK_OrderId";

        public void Insert(OrderDetails o)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Order_Details_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Quantity", o.Quantity);
            cmd.Parameters.AddWithValue("@FK_OrderId", o.OrderId);
            cmd.Parameters.AddWithValue("@FK_FoodItemId", o.FoodItemId);

            con.Open();
            using (con)
            {
                int row = cmd.ExecuteNonQuery();

            }

        }

        public List<OrderDetails> SelectAllOrderDetails(int orderid)
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL_Order_Details, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_OrderId",orderid);
            List<OrderDetails> temp = fetchOrderDetails(cmd);
            return temp;
        }

        private List<OrderDetails> fetchOrderDetails(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<OrderDetails> olist = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    olist = new List<OrderDetails>();
                    while (dr.Read())
                    {
                        OrderDetails o = new OrderDetails();
                        o.Id = Convert.ToInt32(dr["Id"]);
                        o.Quantity = Convert.ToInt32(dr["Quantity"]);
                        o.OrderId = Convert.ToInt32(dr["FK_OrderId"]);
                        o.FoodItemId = Convert.ToInt32(dr["FK_FoodItemId"]);
                        olist.Add(o);
                    }
                    olist.TrimExcess();
                }
            }
            return olist;
        }
    }
}