using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.DAC
{
    public class MessageDAC
    {
        private readonly string SELECT_ALL_Messages = "Select * from Message ORDER BY Id DESC";
        private readonly string SELECT_By_ID = "Select * from Message where Id=@Id";

        public Message SelectById(int id)
        {
            SqlCommand cmd = new SqlCommand(SELECT_By_ID, DACHelper.GetConnection());
            cmd.Parameters.AddWithValue("@Id", id);
            List<Message> temp = fetchMessages(cmd);
            return (temp != null) ? temp[0] : null;
        }

        public List<Message> SelectAllMessages()
        {
            SqlCommand cmd = new SqlCommand(SELECT_ALL_Messages, DACHelper.GetConnection());
            return fetchMessages(cmd);
        }

        public void DeleteMessage(int id)
        {
            SqlCommand cmd = new SqlCommand("Message_Delete", DACHelper.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlConnection con = cmd.Connection;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void InsertMessage(Message m)
        {
            SqlConnection con = DACHelper.GetConnection();
            SqlCommand cmd = new SqlCommand("User_Message_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", m.Name);
            cmd.Parameters.AddWithValue("@Subject", m.Subject);
            cmd.Parameters.AddWithValue("@Description", m.Description);
            if(m.UserId.HasValue)
            {
                cmd.Parameters.AddWithValue("@FK_UserId", m.UserId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FK_UserId", Convert.DBNull);
            }
            
            con.Open();
            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }

        private List<Message> fetchMessages(SqlCommand cmd)
        {
            SqlConnection con = cmd.Connection;
            List<Message> ulist = null;
            con.Open();
            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    ulist = new List<Message>();
                    while (dr.Read())
                    {
                        Message u = new Message();
                        u.Id = Convert.ToInt32(dr["Id"]);
                        u.Name = Convert.ToString(dr["Name"]);
                        u.Subject = Convert.ToString(dr["Subject"]);
                        u.Description = Convert.ToString(dr["Description"]);
                        if (Convert.IsDBNull(dr["FK_UserId"]))
                        {
                            u.UserId = null;
                        }
                        else
                        {
                            u.UserId = Convert.ToInt32(dr["FK_UserId"]);
                        }
                        ulist.Add(u);
                    }
                    ulist.TrimExcess();
                }
            }
            return ulist;
        }
    }
}