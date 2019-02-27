using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class FoodItemDAC
    {
        private readonly string SELECT_ALL = "Select * from FoodItems";
        private readonly string SELECT_BY_MenuCatId = "Select * from FoodItems where FK_MenuCatId = @Id";
        private readonly string SELECT_AllItems_BY_CatId = "Select * from FoodItems where FK_MenuCatId = @FK_MenuCatId";
        private readonly string SELECT_BY_ID = "Select * from FoodItems where Id=@Id";


        public FoodItem SelectById(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<FoodItem> temp = fetchFoodItems(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public FoodItem SELECT_BY_CatId(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_MenuCatId, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<FoodItem> temp = fetchFoodItems(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<FoodItem> SelectAllItems(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_AllItems_BY_CatId, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_MenuCatId", id);
            return fetchFoodItems(cmd);
        }


        public List<FoodItem> SelectAll()
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL, DACHelper.GetConnection());
            return fetchFoodItems(cmd);
        }


        public void Insert(FoodItem f)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("FoodItem_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", f.Name);
            cmd.Parameters.AddWithValue("@Price", f.Price);
            cmd.Parameters.AddWithValue("@Discount", f.Discount);
            cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(f.Description) ? Convert.DBNull : f.Description);
            cmd.Parameters.AddWithValue("@FK_MenuCatId", f.MenuCatId);
            con.Open();
            using (con)
            {
                int row = cmd.ExecuteNonQuery();
            }
        }

        public void Update(FoodItem f)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("FoodItem_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", f.Id);
            cmd.Parameters.AddWithValue("@Name", f.Name);
            cmd.Parameters.AddWithValue("@Price", f.Price);
            cmd.Parameters.AddWithValue("@Discount", f.Discount);
            cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(f.Description) ? Convert.DBNull : f.Description);
            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteFoodItem(int id)
        {
            SqlCommand cmd = new SqlCommand("FoodItem_Delete", DACHelper.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlConnection con = cmd.Connection;
            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        private List<FoodItem> fetchFoodItems(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<FoodItem> f = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    f = new List<FoodItem>();
                    while (dr.Read())
                    {
                        FoodItem fi = new FoodItem();
                        fi.Id = Convert.ToInt32(dr["Id"]);
                        fi.Name = Convert.ToString(dr["Name"]);
                        fi.Price = Convert.ToInt32(dr["Price"]);
                        fi.Discount = Convert.ToSingle(dr["Discount"]);
                        fi.Description = Convert.ToString(dr["Description"]);
                        fi.MenuCatId = Convert.ToInt32(dr["FK_MenuCatId"]);
                        f.Add(fi);
                    }
                    f.TrimExcess();
                }
            }
            return f;
        }
    }
}