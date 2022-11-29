using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace DatabaseTest.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMsg = "";
        public string successMsg = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            clientInfo.Name = Request.Form["Name"];
            clientInfo.Role = Request.Form["Role"];

            if(clientInfo.Name.Length == 0 || clientInfo.Role.Length == 0)
            {
                errorMsg = "All the fields are required";
                return;
            }

            //save data on database
            try
            {
                string conString = @"Data Source=.\sqlexpress;Initial Catalog=test;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string cmdText = "insert into info " +
                                     "(Name, Role) values " +
                                     "(@Name, @Role);";
                    using(SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", clientInfo.Name);
                        cmd.Parameters.AddWithValue("@Role", clientInfo.Role);

                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch(Exception ex) 
            {
                errorMsg = ex.ToString();
                return;
            }
            clientInfo.Name = "";
            clientInfo.Role = "";
            successMsg = "User Inserted correctly";

            Response.Redirect("/Clients/Index");
        }
    }
}
