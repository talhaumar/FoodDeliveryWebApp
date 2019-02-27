using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class MenuCategoryDAC
    {
        private readonly string SELECT_ALL = "Select * from MenuCategory";
        private readonly string SELECT_Categories_By_Restaurant = "Select * from MenuCategory where FK_ResId = @FK_ResId";
        private readonly string SELECT_BY_Restaurant = "Select * from MenuCategory where FK_ResId = @Id";
        private readonly string SELECT_BY_ID = "Select * from MenuCategory where Id=@Id";


        public MenuCategory SelectById(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<MenuCategory> temp = fetchCategories(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public MenuCategory SelectByRestaurant(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_BY_Restaurant, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_ResId", id);
            List<MenuCategory> temp = fetchCategories(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<MenuCategory> SelectCategoriesByRestaurant(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_Categories_By_Restaurant, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@FK_ResId", id);
            List<MenuCategory> temp = fetchCategories(cmd);
            return temp;
        }

     
        public List<MenuCategory> SelectAll()
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL, DACHelper.GetConnection());
            return fetchCategories(cmd);
        }


        public void Insert(MenuCategory mc)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("MenuCategory_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", mc.Name);
            cmd.Parameters.AddWithValue("@FK_ResId",mc.ResId);

            con.Open();
            using (con)
            {
                int row = cmd.ExecuteNonQuery();
            }
        }

        public void Update(MenuCategory mc)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("MenuCategory_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", mc.Id);
            cmd.Parameters.AddWithValue("@Name", mc.Name);

            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteMenuCategory(int id)
        {
            SqlCommand cmd = new SqlCommand("MenuCategory_Delete", DACHelper.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlConnection con = cmd.Connection;
            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        private List<MenuCategory> fetchCategories(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<MenuCategory> mc = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    mc = new List<MenuCategory>();
                    while (dr.Read())
                    {
                        MenuCategory m = new MenuCategory();
                        m.Id = Convert.ToInt32(dr["Id"]);
                        m.Name = Convert.ToString(dr["Name"]);
                        m.ResId = Convert.ToInt32(dr["FK_ResId"]);
                        mc.Add(m);
                    }
                    mc.TrimExcess();
                }
            }
            return mc;
        }
    }
}