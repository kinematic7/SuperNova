using Microsoft.VisualBasic;
using SuperNova.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Constants = SuperNova.Helpers.Constants;
using SuperNova.Data;

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

                using (SuperNovaDBContext db = new SuperNovaDBContext())
                {
                    Accounts =((from acc in db.Accounts where acc.LoginId == loginid select acc).OrderBy(x => x.Name)).ToList();
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

                using(SuperNovaDBContext db = new SuperNovaDBContext())
                {
                    switch(account.Id)
                    {
                        case 0:
                            db.Accounts.Add(account);
                            break;
                        default:
                            var dbAccount = (from a in db.Accounts where a.Id == account.Id && a.LoginId == account.LoginId select a).Single();
                            dbAccount.Name = account.Name;
                            dbAccount.UserName = account.UserName;
                            dbAccount.Password = account.Password;
                            dbAccount.Url = account.Url;
                            dbAccount.Comment = account.Comment;
                            break;
                    }

                    await db.SaveChangesAsync();
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

                using(SuperNovaDBContext db = new SuperNovaDBContext())
                {
                    var dbAccount = (from a in db.Accounts where a.Id == id && a.LoginId == LoginId select a).Single();
                    db.Accounts.Remove(dbAccount);
                    await db.SaveChangesAsync();
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
