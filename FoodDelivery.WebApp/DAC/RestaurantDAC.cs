using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class RestaurantDAC
    {
        private readonly string SELECT_ALL = "Select * from Restaurant";
        private readonly string SELECT_BY_Phone_PASSWORD = "Select * from Restaurant where Phone=@Phone AND Password=@Password";
        private readonly string SELECT_BY_ID = "Select * from Restaurant where Id=@Id";
        private readonly string SELECT_Restaurant_BY_Name = "Select * from Restaurant where Name=@Name";

        public List<Restaurant> SelectRestaurantByName(string resName)
        {
            SqlCommand cmd = new SqlCommand(SELECT_Restaurant_BY_Name, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Name", resName);
            return fetchRestaurants(cmd);
        }

        public List<Restaurant> SelectRestaurantByTerm(string term)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Auto_Res_Search", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@term", term);
            List<Restaurant> r = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    r = new List<Restaurant>();
                    while (dr.Read())
                    {
                        Restaurant res = new Restaurant();
                        res.Name = Convert.ToString(dr["Name"]);
                        r.Add(res);
                    }
                    r.TrimExcess();
                }
            }
            return r;
        }

        public Restaurant SelectById(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<Restaurant> temp = fetchRestaurants(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public Restaurant SelectByPhonePassword(string phone, string password)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_Phone_PASSWORD, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Password", password);
            List<Restaurant> temp = fetchRestaurants(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<Restaurant> SelectAll()
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL, DACHelper.GetConnection());
            return fetchRestaurants(cmd);
        }


        public void Insert(Restaurant res)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Restaurant_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", res.Name);
            cmd.Parameters.AddWithValue("@Phone", res.Phone);
            cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(res.Email) ? Convert.DBNull : res.Email);
            cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(res.Address) ? Convert.DBNull : res.Address);
            cmd.Parameters.AddWithValue("@Password", res.Password);
            cmd.Parameters.AddWithValue("@Tax", res.Tax);
            cmd.Parameters.AddWithValue("@Opening_Time", string.IsNullOrEmpty(res.OpeningTime) ? Convert.DBNull : res.OpeningTime);
            cmd.Parameters.AddWithValue("@Closing_Time", string.IsNullOrEmpty(res.ClosingTime) ? Convert.DBNull : res.ClosingTime);
            cmd.Parameters.AddWithValue("@Area", res.Area);
            cmd.Parameters.AddWithValue("@Review", string.IsNullOrEmpty(res.Review) ? Convert.DBNull : res.Review);
            cmd.Parameters.AddWithValue("@ImageURL", string.IsNullOrEmpty(res.ImageURL) ? Convert.DBNull : res.ImageURL);

            con.Open();
            using (con)
            {
                int row = cmd.ExecuteNonQuery();
            }
        }

        public int Insert_And_GetID(Restaurant res)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Restaurant_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", res.Name);
            cmd.Parameters.AddWithValue("@Phone", res.Phone);
            cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(res.Email) ? Convert.DBNull : res.Email);
            cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(res.Address) ? Convert.DBNull : res.Address);
            cmd.Parameters.AddWithValue("@Password", res.Password);
            cmd.Parameters.AddWithValue("@Tax", res.Tax);
            cmd.Parameters.AddWithValue("@Opening_Time", string.IsNullOrEmpty(res.OpeningTime) ? Convert.DBNull : res.OpeningTime);
            cmd.Parameters.AddWithValue("@Closing_Time", string.IsNullOrEmpty(res.ClosingTime) ? Convert.DBNull : res.ClosingTime);
            cmd.Parameters.AddWithValue("@Area", res.Area);
            cmd.Parameters.AddWithValue("@Review", string.IsNullOrEmpty(res.Review) ? Convert.DBNull : res.Review);
            cmd.Parameters.AddWithValue("@ImageURL", string.IsNullOrEmpty(res.ImageURL) ? Convert.DBNull : res.ImageURL);

            con.Open();
            using (con)
            {
                int row = Convert.ToInt32(cmd.ExecuteScalar());
                return row;
            }

        }
        public void Update(Restaurant res)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Restaurant_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", res.Id);
            cmd.Parameters.AddWithValue("@Name", res.Name);
            cmd.Parameters.AddWithValue("@Phone", res.Phone);
            cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(res.Email) ? Convert.DBNull : res.Email);
            cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(res.Address) ? Convert.DBNull : res.Address);
            cmd.Parameters.AddWithValue("@Password", res.Password);
            cmd.Parameters.AddWithValue("@Tax", res.Tax);
            cmd.Parameters.AddWithValue("@Opening_Time", string.IsNullOrEmpty(res.OpeningTime) ? Convert.DBNull : res.OpeningTime);
            cmd.Parameters.AddWithValue("@Closing_Time", string.IsNullOrEmpty(res.ClosingTime) ? Convert.DBNull : res.ClosingTime);
            cmd.Parameters.AddWithValue("@Area", res.Area);
            cmd.Parameters.AddWithValue("@Review", string.IsNullOrEmpty(res.Review) ? Convert.DBNull : res.Review);
            cmd.Parameters.AddWithValue("@ImageURL", string.IsNullOrEmpty(res.ImageURL) ? Convert.DBNull : res.ImageURL);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteRestaurant(int id)
        {
            SqlCommand cmd = new SqlCommand("Restaurant_Delete", DACHelper.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id",id);
            SqlConnection con = cmd.Connection;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void DeleteRestaurantImage(int id)
        {
            SqlCommand cmd = new SqlCommand("Restaurant_Image_Delete", DACHelper.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlConnection con = cmd.Connection;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void SetCookTime(int orderId, string cooktime)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("CookTime_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("Id", orderId);
            cmd.Parameters.AddWithValue("CookTime", cooktime);
            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        private List<Restaurant> fetchRestaurants(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<Restaurant> r = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    r = new List<Restaurant>();
                    while (dr.Read())
                    {
                        Restaurant res = new Restaurant();
                        res.Id = Convert.ToInt32(dr["Id"]);
                        res.Name = Convert.ToString(dr["Name"]);
                        res.Phone = Convert.ToString(dr["Phone"]);
                        res.Email = Convert.ToString(dr["Email"]);
                        res.Address = Convert.ToString(dr["Address"]);
                        res.Password = Convert.ToString(dr["Password"]);
                        res.Tax = Convert.ToSingle(dr["Tax"]);
                        res.OpeningTime = Convert.ToString(dr["Opening_Time"]);
                        res.ClosingTime = Convert.ToString(dr["Closing_Time"]);
                        res.Area = Convert.ToString(dr["Area"]);
                        res.Review = Convert.ToString(dr["Review"]);
                        res.ImageURL = Convert.ToString(dr["ImageURL"]);
                        r.Add(res);
                    }
                    r.TrimExcess();
                }
            }
            return r;
        }

    }
}