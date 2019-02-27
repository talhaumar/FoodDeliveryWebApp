using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class CustomerDAC
    {
        private readonly string SELECT_ALL_Customers = "Select * from [User] where FK_RoleId=@FK_RoleId";
        private readonly string SELECT_BY_Phone_PASSWORD = "Select * from [User] where Phone=@Phone AND Password=@Password";
        private readonly string SELECT_BY_ID = "Select * from [User] where Id=@Id";

        public void PasswordUpdate(ResetPasswordModel model)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Password_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Phone", model.Phone);
            cmd.Parameters.AddWithValue("@Password", model.Password);
            con.Open();
            using (con)
            {
                Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public User SelectById(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<User> temp = fetchCustomers(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public User SelectByPhonePassword(string phone, string password)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_Phone_PASSWORD, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Password", password);
            List<User> temp = fetchCustomers(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<User> SelectAllCustomers(int RoleId)
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL_Customers, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_RoleId",RoleId);
            return fetchCustomers(cmd);
        }


        public int Insert(SignupModel u)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Customer_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", u.Name);
            cmd.Parameters.AddWithValue("@Phone", u.Phone);
            cmd.Parameters.AddWithValue("@Password", u.Password);
            cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(u.Address) ? Convert.DBNull : u.Address);
            cmd.Parameters.AddWithValue("@FK_RoleId", u.RoleId);

            con.Open();
            using (con)
            {
                int row = Convert.ToInt32(cmd.ExecuteScalar());
                return row;
            }
        }

        public void DeleteCustomer(int id)
        {
            SqlCommand cmd = new SqlCommand("Customer_Delete", DACHelper.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlConnection con = cmd.Connection;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private List<User> fetchCustomers(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<User> ulist = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    ulist = new List<User>();
                    while (dr.Read())
                    {
                        User u = new User();
                        u.Id = Convert.ToInt32(dr["Id"]);
                        u.Name = Convert.ToString(dr["Name"]);
                        u.Phone = Convert.ToString(dr["Phone"]);
                        u.Password = Convert.ToString(dr["Password"]);
                        u.Email = Convert.ToString(dr["Email"]);
                        u.Address = Convert.ToString(dr["Address"]);
                        u.SecurityQuestion = Convert.ToString(dr["SecurityQuestion"]);
                        u.SecurityAnswer = Convert.ToString(dr["SecurityAnswer"]);
                        u.RoleId = Convert.ToInt32(dr["FK_RoleId"]);
                        ulist.Add(u);
                    }
                    ulist.TrimExcess();
                }
            }
            return ulist;
        }
    }
}