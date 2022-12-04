using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace DatabaseTest.Pages.Clients
{
    public class EditModel : PageModel
    {

        public ClientInfo clientInfo = new ClientInfo();
        public string errorMsg = "";
        public string successMsg = "";
        public void OnGet()
        {
            string ID = Request.Query["id"];

            try
            {
                string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=test;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string cmdText = "select * from info where ID=@id";
                    using(SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        cmd.Parameters.AddWithValue("@id", ID);
                        using(SqlDataReader sdr = cmd.ExecuteReader()) 
                        {
                            if(sdr.Read())
                            {
                                clientInfo.ID = "" + sdr.GetInt32(0);
                                clientInfo.Name = sdr.GetString(1);
                                clientInfo.Role = sdr.GetString(2);
                            }
                        }
                    }

                    con.Close();
                }
            }
            catch(Exception ex)
            {
                errorMsg = ex.Message + "\nID: " + ID;
            }
        }

        public void OnPost() 
        {
            clientInfo.ID = Request.Form["id"];
            clientInfo.Name = Request.Form["Name"];
            clientInfo.Role = Request.Form["Role"];

            if(clientInfo.ID.Length == 0 || clientInfo.Name.Length == 0 || clientInfo.Role.Length == 0)
            {
                errorMsg = "All fields are required";
                return;
            }

            try
            {
                string conString = @"Data Source=.\sqlexpress;Initial Catalog=test;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string cmdText = "Update info set Name=@name, Role=@role where ID=@id";
                    using(SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        cmd.Parameters.AddWithValue("@id", clientInfo.ID);
                        cmd.Parameters.AddWithValue("@name", clientInfo.Name);
                        cmd.Parameters.AddWithValue("@role", clientInfo.Role);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch(Exception ex)
            {
                errorMsg = ex.Message;
                return;
            }

            Response.Redirect("/Clients/Index");
        }
    }
}
