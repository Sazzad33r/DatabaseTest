using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DatabaseTest.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> clientInfoList = new List<ClientInfo>();
        public void OnGet()
        {
            try
            {
                string conString = @"Data Source=.\sqlexpress;Initial Catalog=test;Integrated Security=True";
                using(SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string cmdString = "select * from info;";
                    using (SqlCommand cmd = new SqlCommand(cmdString, con))
                    {
                        using(SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while(sdr.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.ID = "" + sdr.GetInt32(0);
                                clientInfo.Name = sdr.GetString(1);
                                clientInfo.Role = sdr.GetString(2);

                                clientInfoList.Add(clientInfo);
                            }
                        }
                    }
                    con.Close();

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class ClientInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Role { get; set;}
    }
}
