using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class AreaDAC
    {
        private readonly string SELECT_ALL = "Select * from Area";
        private readonly string InsertArea = "Area_Insert";
        private readonly string SELECT_BY_ID = "Select * from Area where Id=@Id";
        private readonly string Insert_Rider_Area = "Rider_Area_Insert";
        private readonly string Update_Area_ID = "Rider_Area_Update";
        private readonly string RiderArea_Ids_Select_By_RiderId = "Select * from RiderArea where FK_RiderId=@FK_RiderId";
        private readonly string Select_Area_Record_ID = "Select * from RiderArea where FK_AreaId=@FK_AreaId";
        private readonly string Delete_Rider_Area = "Rider_Area_Delete";


        public Area SelectById(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<Area> temp = fetchAreas(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<Area> SelectAll()
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL, DACHelper.GetConnection());
            return fetchAreas(cmd);
        }


        public void Insert(Area a)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand(InsertArea, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", a.Name);

            con.Open();
            using (con)
            {
                int row = cmd.ExecuteNonQuery();
            }
        }

        private List<Area> fetchAreas(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<Area> a = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    a = new List<Area>();
                    while (dr.Read())
                    {
                        Area ra = new Area();
                        ra.Id = Convert.ToInt32(dr["Id"]);
                        ra.Name = Convert.ToString(dr["Name"]);
                        a.Add(ra);
                    }
                    a.TrimExcess();
                }
            }
            return a;
        }


        public void RiderAreaInsert(int r, int t)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand(Insert_Rider_Area, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_RiderId", r);
            cmd.Parameters.AddWithValue("@FK_AreaId", t);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void RiderAreaIdUpdate(int r, int t)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand(Update_Area_ID, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_RiderId", r);
            cmd.Parameters.AddWithValue("@FK_AreaId", t);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public List<Area> SelectRiderAreaIdsByRiderId(int id)
        {
            SqlCommand cmd = new SqlCommand(RiderArea_Ids_Select_By_RiderId, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_RiderId", id);
            List<Area> temp = fetchRiderAreaIds(cmd);
            return temp;
        }

        public Area SelectRiderAreaRecordIdByAreaId(int id)
        {
            SqlCommand cmd = new SqlCommand(Select_Area_Record_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_AreaId", id);
            List<Area> temp = fetchRideAreaRecordIds(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public void DeleteRider_AreaRecordById(int id)
        {
            SqlCommand cmd = new SqlCommand(Delete_Rider_Area, DACHelper.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlConnection con = cmd.Connection;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


        }

        private List<Area> fetchRiderAreaIds(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<Area> rt = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    rt = new List<Area>();
                    while (dr.Read())
                    {
                        Area a = new Area();
                        a.Id = Convert.ToInt32(dr["FK_AreaId"]);
                        rt.Add(a);
                    }
                    rt.TrimExcess();
                }
            }
            return rt;
        }

        private List<Area> fetchRideAreaRecordIds(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<Area> rt = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    rt = new List<Area>();
                    while (dr.Read())
                    {
                        Area a = new Area();
                        a.Id = Convert.ToInt32(dr["Id"]);
                        rt.Add(a);
                    }
                    rt.TrimExcess();
                }
            }
            return rt;
        }
    }
}