using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{

    public class OrderDAC
    {
        private readonly string SELECT_ALL_Orders = "Select * from [Order] ORDER BY [OrderDateTime]";
        private readonly string SELECT_BY_ID = "Select * from [Order] where Id=@Id";
        private readonly string SELECT_BY_RIDER_ID = "Select * from [Order] Where FK_RiderId=@FK_RiderId AND [FK_OrderStatusId]=@FK_OrderStatusId";
        private readonly string SELECT_BY_RESTAURANT_ID = "Select * from [Order] Where FK_ResId=@FK_ResId ORDER BY [OrderDateTime]";
        private readonly string SELECT_BY_CUSTOMER_ID = "Select * from [Order] Where FK_UserId=@FK_UserId ORDER BY [FK_OrderStatusId]";
        private readonly string SELECT_BY_PHONE = "Select * from [Order] Where Phone=@Phone AND FK_OrderStatusId=@FK_OrderStatusId";

        public List<Order> SelectByPhone(string phone,int osid)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_PHONE, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@FK_OrderStatusId", osid);
            List<Order> temp = fetchOrders(cmd);
            return temp;
        }

        public int Insert_And_GetID(Order o)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Order_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StreetAddress", o.StreetAddress);
            cmd.Parameters.AddWithValue("@Phone", o.Phone);
            cmd.Parameters.AddWithValue("@OrderDateTime", o.OrderDateTime);
            cmd.Parameters.AddWithValue("@CookTime", string.IsNullOrEmpty(o.CookTime) ? Convert.DBNull : o.CookTime);
            cmd.Parameters.AddWithValue("@DeliveryCharges", o.DeliveryCharges);
            cmd.Parameters.AddWithValue("@FK_UserId", o.UserId);
            cmd.Parameters.AddWithValue("@FK_OrderStatusId", o.OrderStatusId);
            cmd.Parameters.AddWithValue("@FK_ResId", o.ResId);

            con.Open();
            using (con)
            {
                int row = Convert.ToInt32(cmd.ExecuteScalar());
                return row;
            }

        }

        public int Insert_GuestOrder_And_GetID(Order o)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Guest_Order_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StreetAddress", o.StreetAddress);
            cmd.Parameters.AddWithValue("@Phone", o.Phone);
            cmd.Parameters.AddWithValue("@OrderDateTime", o.OrderDateTime);
            cmd.Parameters.AddWithValue("@CookTime", string.IsNullOrEmpty(o.CookTime) ? Convert.DBNull : o.CookTime);
            cmd.Parameters.AddWithValue("@DeliveryCharges", o.DeliveryCharges);
            cmd.Parameters.AddWithValue("@FK_OrderStatusId", o.OrderStatusId);
            cmd.Parameters.AddWithValue("@FK_ResId", o.ResId);

            con.Open();
            using (con)
            {
                int row = Convert.ToInt32(cmd.ExecuteScalar());
                return row;
            }

        }

        public Order SelectById(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<Order> temp = fetchOrders(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<Order> SelectAllOrders()
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL_Orders, DACHelper.GetConnection());
            List<Order> temp = fetchOrders(cmd);
            return temp;
        }

        public List<Order> SelectByRiderId(int riderId)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_RIDER_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_RiderId", riderId);
            cmd.Parameters.AddWithValue("@FK_OrderStatusId", 2);
            List<Order> temp = fetchOrders(cmd);
            return temp;
        }

        public List<Order> SelectByResturantId(int resId)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_RESTAURANT_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_ResId", resId);
            List<Order> temp = fetchOrders(cmd);
            return temp;
        }

        public List<Order> SelectByCustomerId(int customerId)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_CUSTOMER_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_UserId", customerId);
            List<Order> temp = fetchOrders(cmd);
            return temp;
        }

        public void UpdateOrderStatus(Order o)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("OrderStatus_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", o.Id);
            cmd.Parameters.AddWithValue("@FK_OrderStatusId", o.OrderStatusId);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateOrderRider(Order o)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Order_RiderAssign_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", o.Id);
            cmd.Parameters.AddWithValue("@FK_RiderId", o.RiderId);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteOrder(int id)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Order_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        private List<Order> fetchOrders(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<Order> olist = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    olist = new List<Order>();
                    while (dr.Read())
                    {
                        Order o = new Order();
                        o.Id = Convert.ToInt32(dr["Id"]);
                        o.StreetAddress = Convert.ToString(dr["StreetAddress"]);
                        o.Phone = Convert.ToString(dr["Phone"]);
                        o.OrderDateTime = Convert.ToDateTime(dr["OrderDateTime"]);
                        o.CookTime = Convert.ToString(dr["CookTime"]);
                        o.DeliveryCharges = Convert.ToInt32(dr["DeliveryCharges"]);
                        
                        if (Convert.IsDBNull(dr["FK_UserId"]))
                        {
                            o.UserId = null;
                        }
                        else
                        {
                            o.UserId = Convert.ToInt32(dr["FK_UserId"]);
                        }
                        o.OrderStatusId = Convert.ToInt32(dr["FK_OrderStatusId"]);
                        if (Convert.IsDBNull(dr["FK_RiderId"]))
                        {
                            o.RiderId = null;
                        }
                        else
                        {
                            o.RiderId = Convert.ToInt32(dr["FK_RiderId"]);
                        }
                        o.ResId = Convert.ToInt32(dr["FK_ResId"]);
                        olist.Add(o);
                    }
                    olist.TrimExcess();
                }
            }
            return olist;
        }
    }
}