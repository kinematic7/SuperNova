using SuperNova.Data;
using SuperNova.Helpers;
using SuperNova.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace SuperNova.Services
{
    public class LoginService
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
            //using (SuperNovaDBContext db = new SuperNovaDBContext())
            //{
            //    return await (from l in db.Logins where l.LoginId == loginid select l).SingleOrDefaultAsync();
            //}

            Login login = null;

            using (SuperNovaDBContext db = new SuperNovaDBContext())
            {
                login = await (from l in db.Logins where l.LoginId == loginid select l).SingleOrDefaultAsync();
            }

            //using (SqlConnection connection = new SqlConnection(Constants.CONNECTIONSTRING))
            //{
            //    connection.Open();
            //    SqlCommand command = new SqlCommand("SELECT * from Logins WHERE loginid = '" + loginid + "'", connection);
            //    var reader = await command.ExecuteReaderAsync();
            //    var dataTable = new DataTable();
            //    dataTable.Load(reader);

            //    for (int i = 0; i < dataTable.Rows.Count; i++)
            //    {
            //        login.LoginId = (string)dataTable.Rows[i]["LoginId"];
            //        login.Password = (string)dataTable.Rows[i]["Password"];
            //    }
            //}

            return login;
        }
        
        public async Task<string> Register(Login login)
        {
            var retVal = "You have registered successfully";

            try
            {
                var isLoginExist = (await GetLoginDetailsByLoginId(login.LoginId)) != null ? true : false;
                if (isLoginExist)
                    throw new Exception("Login Id is already taken");

                using(SuperNovaDBContext db = new SuperNovaDBContext())
                {
                    db.Logins.Add(login);
                    await db.SaveChangesAsync();
                }

                // Old Style

                //using (SqlConnection connection = new SqlConnection(Constants.CONNECTIONSTRING))
                //{
                //    connection.Open();
                //    SqlCommand command = new SqlCommand("INSERT INTO Logins(LoginId, Password) VALUES('" + login.LoginId + "','" + login.Password +"')", connection);
                //    await command.ExecuteNonQueryAsync();            
                //}
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }

            return retVal;
        }

    }
}
