using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class RestaurantTypeDAC
    {
        private readonly string SELECT_ALL = "Select * from Type";
        private readonly string InsertType = "Type_Insert";
        private readonly string SELECT_BY_ID = "Select * from Type where Id=@Id";
        private readonly string Insert_Res_Type = "Res_Type_Insert";
        private readonly string Update_Res_TypeID = "Res_Type_Update";
        private readonly string ResType_Ids_Select_By_ResId = "Select * from Res_Type where FK_ResId=@FK_ResId";
        private readonly string ResIds_Select_By_TypeId = "Select * from Res_Type where FK_TypeId=@FK_TypeId";
        private readonly string Select_Res_Type_Record_ID = "Select * from Res_Type where FK_TypeId=@FK_TypeId";


        public RestaurantType SelectById(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<RestaurantType> temp = fetchTypes(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<RestaurantType> SelectAll()
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL, DACHelper.GetConnection());
            return fetchTypes(cmd);
        }


        public void Insert(RestaurantType rt)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand(InsertType, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", rt.Name);

            con.Open();
            using (con)
            {
                int row = cmd.ExecuteNonQuery();
            }
        }

        private List<RestaurantType> fetchTypes(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<RestaurantType> rt = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    rt = new List<RestaurantType>();
                    while (dr.Read())
                    {
                        RestaurantType rtype = new RestaurantType();
                        rtype.Id = Convert.ToInt32(dr["Id"]);
                        rtype.Name = Convert.ToString(dr["Name"]);
                        rt.Add(rtype);
                    }
                    rt.TrimExcess();
                }
            }
            return rt;
        }


        public void ResTypeInsert(int r, int t)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand(Insert_Res_Type, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_ResId", r);
            cmd.Parameters.AddWithValue("@FK_TypeId", t);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void ResTypeIdUpdate(int r, int t)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand(Update_Res_TypeID, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_ResId", r);
            cmd.Parameters.AddWithValue("@FK_TypeId", t);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public List<RestaurantType> SelectResTypeIdsByResId(int id)
        {
            SqlCommand cmd = new SqlCommand(ResType_Ids_Select_By_ResId, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_ResId", id);
            List<RestaurantType> temp = fetchResIds(cmd);
            return temp;
        }

        public List<RestaurantType> SelectResIdsByTypeId(int id)
        {
            SqlCommand cmd = new SqlCommand(ResIds_Select_By_TypeId, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_TypeId", id);
            List<RestaurantType> temp = fetchResIds(cmd);
            return temp;
        }

        public RestaurantType SelectResTypeRecordIdByTypeId(int id)
        {
            SqlCommand cmd = new SqlCommand(Select_Res_Type_Record_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_TypeId", id);
            List<RestaurantType> temp = fetchResTypeRecordIds(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public void DeleteRes_TypeRecordById(int id)
        {
            SqlCommand cmd = new SqlCommand("Res_Type_Delete", DACHelper.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlConnection con = cmd.Connection;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


        }
        private List<RestaurantType> fetchResIds(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<RestaurantType> rt = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    rt = new List<RestaurantType>();
                    while (dr.Read())
                    {
                        RestaurantType rtype = new RestaurantType();
                        rtype.Id = Convert.ToInt32(dr["FK_TypeId"]);
                        rt.Add(rtype);
                    }
                    rt.TrimExcess();
                }
            }
            return rt;
        }

        private List<RestaurantType> fetchResTypeIds(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<RestaurantType> rt = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    rt = new List<RestaurantType>();
                    while (dr.Read())
                    {
                        RestaurantType rtype = new RestaurantType();
                        rtype.Id = Convert.ToInt32(dr["FK_TypeId"]);
                        rt.Add(rtype);
                    }
                    rt.TrimExcess();
                }
            }
            return rt;
        }

        private List<RestaurantType> fetchResTypeRecordIds(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<RestaurantType> rt = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    rt = new List<RestaurantType>();
                    while (dr.Read())
                    {
                        RestaurantType rtype = new RestaurantType();
                        rtype.Id = Convert.ToInt32(dr["Id"]);
                        rt.Add(rtype);
                    }
                    rt.TrimExcess();
                }
            }
            return rt;
        }
    }
}