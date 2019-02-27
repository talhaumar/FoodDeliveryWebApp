using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class AdminDAC
    {
        private readonly string SELECT_ALL_Admins = "Select * from [User] Where FK_RoleId=@FK_RoleId";
        private readonly string SELECT_BY_Phone_PASSWORD = "Select * from User where Phone=@Phone AND Password=@Password";
        private readonly string SELECT_BY_ID = "Select * from [User] where Id=@Id AND FK_RoleId=@FK_RoleId";


        public User SelectById(int id, int roleid)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@FK_RoleId", roleid);
            List<User> temp = fetchAdmins(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public User SelectByPhonePassword(string phone, string password)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_Phone_PASSWORD, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Password", password);
            List<User> temp = fetchAdmins(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<User> SelectAllAdmins()
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL_Admins, DACHelper.GetConnection());
            List<User> temp = fetchAdmins(cmd);
            return temp;
        }

        public List<User> SelectAdminbyRoleId(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL_Admins, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_RoleId", id);
            return fetchAdmins(cmd);
        }

        public void Insert(User u)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Admin_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", u.Name);
            cmd.Parameters.AddWithValue("@Phone", u.Phone);
            cmd.Parameters.AddWithValue("@Password", u.Password);
            cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(u.Email) ? Convert.DBNull : u.Email);
            cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(u.Address) ? Convert.DBNull : u.Address);
            cmd.Parameters.AddWithValue("@SecurityQuestion", string.IsNullOrEmpty(u.SecurityQuestion) ? Convert.DBNull : u.SecurityQuestion);
            cmd.Parameters.AddWithValue("@SecurityAnswer", string.IsNullOrEmpty(u.SecurityAnswer) ? Convert.DBNull : u.SecurityAnswer);
            cmd.Parameters.AddWithValue("@FK_RoleId", u.RoleId);

            con.Open();
            using (con)
            {
                int row = cmd.ExecuteNonQuery();
            }
        }

        public void Update(User u)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Admin_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", u.Id);
            cmd.Parameters.AddWithValue("@Name", u.Name);
            cmd.Parameters.AddWithValue("@Phone", u.Phone);
            cmd.Parameters.AddWithValue("@Password", u.Password);
            cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(u.Email) ? Convert.DBNull : u.Email);
            cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(u.Address) ? Convert.DBNull : u.Address);
            cmd.Parameters.AddWithValue("@SecurityAnswer", string.IsNullOrEmpty(u.SecurityAnswer) ? Convert.DBNull : u.SecurityAnswer);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Admin_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        private List<User> fetchAdmins(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<User> ulist = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
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