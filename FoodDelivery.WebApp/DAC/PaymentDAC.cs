using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class PaymentDAC
    {
        private readonly string SELECT_ALL_Payments = "Select * from Payment";
        private readonly string SELECT_BY_ID = "Select * from Payment where Id=@Id";
        private readonly string SELECT_BY_Order_ID = "Select * from Payment where FK_OrderId=@FK_OrderId";

        public void Insert(Payment o)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Payment_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PaymentTime", string.IsNullOrEmpty(o.PaymentTime) ? Convert.DBNull : o.PaymentTime);
            cmd.Parameters.AddWithValue("@Amount", o.Amount);
            cmd.Parameters.AddWithValue("@PaymentStatus", o.PaymentStatus);
            cmd.Parameters.AddWithValue("@FK_OrderId", o.OrderId);


            con.Open();
            using (con)
            {
                int row = cmd.ExecuteNonQuery();

            }

        }

        public Payment SelectById(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<Payment> temp = fetchPayments(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public Payment SelectByOrderId(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_Order_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_OrderId", id);
            List<Payment> temp = fetchPayments(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<Payment> SelectAllPayments(int OrderStatusId)
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL_Payments, DACHelper.GetConnection());
            List<Payment> temp = fetchPayments(cmd);
            return temp;
        }


        public void UpdatePaymentStatus(Payment p)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("PaymentStatus_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PaymentStatus", p.PaymentStatus);
            cmd.Parameters.AddWithValue("@FK_OrderId", p.OrderId);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        private List<Payment> fetchPayments(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<Payment> plist = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    plist = new List<Payment>();
                    while (dr.Read())
                    {
                        Payment p = new Payment();
                        p.Id = Convert.ToInt32(dr["Id"]);
                        p.PaymentTime = Convert.ToString(dr["PaymentTime"]);
                        p.Amount = Convert.ToSingle(dr["Amount"]);
                        p.PaymentStatus = Convert.ToInt32(dr["PaymentStatus"]);
                        p.OrderId = Convert.ToInt32(dr["FK_OrderId"]);
                        plist.Add(p);
                    }
                    plist.TrimExcess();
                }
            }
            return plist;
        }
    }
}