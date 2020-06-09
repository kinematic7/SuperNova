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
    public class AccountService
    {
        public Login AuthorizationToken { get; set; }

        public async Task<List<Account>> GetAccounts()
        {
            List<Account> Accounts = new List<Account>();
            try
            {
                var loginid = new LoginService().GetAuthenticationToken(AuthorizationToken).Result.LoginId;
                using (SqlConnection connection = new SqlConnection(Constants.CONNECTIONSTRING))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * from Accounts WHERE loginid = '" + loginid + "' ORDER BY Name", connection);
                    var reader = await command.ExecuteReaderAsync();
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        Account account = new Account()
                        {
                            Id = (int)(dataTable.Rows[i]["Id"]),
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

        public async Task<bool> UpdateAccount(Account account)
        {
            var retVal = false;
            try
            {
                account.LoginId = new LoginService().GetAuthenticationToken(AuthorizationToken).Result.LoginId;
                using (SqlConnection connection = new SqlConnection(Constants.CONNECTIONSTRING))
                {
                    connection.Open();
                    if (account.Id == 0)
                    {
                        SqlCommand command = new SqlCommand(@"INSERT INTO Accounts(LoginId, Name,Url,UserName,Password,Comment) VALUES('" +
                                                             account.LoginId + "','" +
                                                             account.Name + "','" +
                                                             account.Url + "','" +
                                                             account.UserName + "','" +
                                                             account.Password + "','" +
                                                             account.Comment + "')",
                                                             connection);
                        await command.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        SqlCommand command = new SqlCommand(@"UPDATE Accounts SET " +
                                                             "Name = '" +
                                                             account.Name +
                                                             "', Url = '" +
                                                             account.Url +
                                                             "', UserName = '" +
                                                             account.UserName +
                                                             "', Password = '" +
                                                             account.Password +
                                                             "', Comment = '" +
                                                             account.Comment +
                                                             "' WHERE Id='" + account.Id + "' AND LoginId = '" + account.LoginId + "'",
                                                             connection);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                retVal = true;
            }
            catch (Exception ex)
            {
                retVal = false;
            }

            return retVal;
        }

        public async Task<bool> DeleteAccount(int id)
        {
            var retVal = false;
            try
            {
                var LoginId = new LoginService().GetAuthenticationToken(AuthorizationToken).Result.LoginId;
                using (SqlConnection connection = new SqlConnection(Constants.CONNECTIONSTRING))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE from Accounts WHERE Id=" + id, connection);
                    await command.ExecuteNonQueryAsync();
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                retVal = false;
            }

            return retVal;

        }
    }
}
