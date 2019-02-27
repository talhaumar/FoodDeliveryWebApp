using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class RiderDAC
    {
        private readonly string SELECT_ALL = "Select * from Rider";
        private readonly string SELECT_BY_Phone_PASSWORD = "Select * from Rider where Phone=@Phone AND Password=@Password";
        private readonly string SELECT_BY_ID = "Select * from Rider where Id=@Id";


        public Rider SelectById(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<Rider> temp = fetchRiders(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public Rider SelectByPhonePassword(string phone, string password)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_Phone_PASSWORD, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Password", password);
            List<Rider> temp = fetchRiders(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<Rider> SelectAll()
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL, DACHelper.GetConnection());
            return fetchRiders(cmd);
        }


        public void Insert(Rider r)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Rider_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", r.Name);
            cmd.Parameters.AddWithValue("@Phone", r.Phone);
            cmd.Parameters.AddWithValue("@Password", r.Password);
            cmd.Parameters.AddWithValue("@RiderStatus", r.RiderStatus);
            cmd.Parameters.AddWithValue("@Longitude", string.IsNullOrEmpty(r.Longitude) ? Convert.DBNull : r.Longitude);
            cmd.Parameters.AddWithValue("@Latitude", string.IsNullOrEmpty(r.Latitude) ? Convert.DBNull : r.Latitude);

            con.Open();
            using (con)
            {
                int row = cmd.ExecuteNonQuery();
            }
        }

        public int Insert_And_GetID(Rider r)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Rider_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", r.Name);
            cmd.Parameters.AddWithValue("@Phone", r.Phone);
            cmd.Parameters.AddWithValue("@Password", r.Password);
            cmd.Parameters.AddWithValue("@RiderStatus", r.RiderStatus);
            cmd.Parameters.AddWithValue("@Longitude", string.IsNullOrEmpty(r.Longitude) ? Convert.DBNull : r.Longitude);
            cmd.Parameters.AddWithValue("@Latitude", string.IsNullOrEmpty(r.Latitude) ? Convert.DBNull : r.Latitude);


            con.Open();
            using (con)
            {
                int row = Convert.ToInt32(cmd.ExecuteScalar());
                return row;
            }

        }
        
        public void Update(Rider r)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Rider_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", r.Id);
            cmd.Parameters.AddWithValue("@Name", r.Name);
            cmd.Parameters.AddWithValue("@Phone", r.Phone);
            cmd.Parameters.AddWithValue("@Password", r.Password);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateriderLatLon(Rider r)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Rider_Lat_Lon_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", r.Id);
            cmd.Parameters.AddWithValue("@Longitude", r.Longitude);
            cmd.Parameters.AddWithValue("@Latitude", r.Latitude);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("Rider_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        private List<Rider> fetchRiders(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<Rider> r = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    r = new List<Rider>();
                    while (dr.Read())
                    {
                        Rider rider = new Rider();
                        rider.Id = Convert.ToInt32(dr["Id"]);
                        rider.Name = Convert.ToString(dr["Name"]);
                        rider.Phone = Convert.ToString(dr["Phone"]);
                        rider.Password = Convert.ToString(dr["Password"]);
                        rider.RiderStatus = Convert.ToInt32(dr["RiderStatus"]);
                        rider.Longitude = Convert.ToString(dr["Longitude"]);
                        rider.Latitude = Convert.ToString(dr["Latitude"]);
                        r.Add(rider);
                    }
                    r.TrimExcess();
                }
            }
            return r;
        }
    }
}