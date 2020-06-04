using Microsoft.VisualBasic;
using SuperNova.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Constants = SuperNova.Helpers.Constants;

namespace SuperNova.Services
{
    public class AccountService : Account
    {
        public Login AuthorizationToken { get; set; }
         
        public async Task<List<Account>> GetAccounts()
        {
            List<Account> Accounts = new List<Account>();
            var loginid = new LoginService().GetAuthenticationToken(AuthorizationToken).Result.LoginId;
            try
            {
                using (SqlConnection connection = new SqlConnection(Constants.CONNECTIONSTRING))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * from Accounts WHERE loginid = '" + loginid + "'", connection);
                    var reader = await command.ExecuteReaderAsync();
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        Account account = new Account()
                        {
                            Name = dataTable.Rows[i]["Name"].ToString(),
                            Url = dataTable.Rows[i]["Url"].ToString(),
                            UserName = dataTable.Rows[i]["UserName"].ToString(),
                            Password = dataTable.Rows[i]["Password"].ToString(),
                            Comment = dataTable.Rows[i]["Comment"].ToString()
                        };
                        Accounts.Add(account);
                    }
                }
            }
            catch (Exception)
            {
                Accounts = null;
            }

            return Accounts;
        }
    }
}
