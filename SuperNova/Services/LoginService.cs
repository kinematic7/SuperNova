using SuperNova.Helpers;
using SuperNova.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace SuperNova.Services
{
    public class LoginService : Login
    {
        public static List<Login> logins = new List<Login>();

        public async Task<Login> SetAuthenticationToken(Models.Login login)
        {
            var dbLogin = await GetLoginDetailsByLoginId(login.LoginId);
            if(login.LoginId == dbLogin.LoginId && login.Password == dbLogin.Password)
            {
                login.Password = "";
                login.SessionKey = DateTime.Now.Ticks.ToString();

                var getLogin = (from l in logins where l.LoginId == login.LoginId select l).SingleOrDefault();

                if (getLogin == null)
                    logins.Add(login);
                else
                {
                    logins.Remove(getLogin);
                    logins.Add(login);
                }
            }
            else
            {
                return null;
            }

            return login;
        }

        public async Task<Login> GetAuthenticationToken(Models.Login login)
        {
            return (from l in logins where l.SessionKey == login.SessionKey select l).SingleOrDefault();            
        }


        public async Task<Login> GetLoginDetailsByLoginId(string loginid)
        {
            Login login = new Login();
            try
            {
                using (SqlConnection connection = new SqlConnection(Constants.CONNECTIONSTRING))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * from Logins WHERE loginid = '" + loginid + "'", connection);
                    var reader = await command.ExecuteReaderAsync();
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {                        
                        login.LoginId = (string)dataTable.Rows[i]["LoginId"];
                        login.Password = (string)dataTable.Rows[i]["Password"];
                    }
                }
            }
            catch (Exception)
            {
                login = null;
            }

            return login;
        }       

    }
}
