using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Reflection.Metadata;
using SuperNova.Models;

namespace SuperNova.Data
{
    public class SuperNovaDBContext : DbContext
    {
        public SuperNovaDBContext() : base(Helpers.Constants.CONNECTIONSTRING)
        {

        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
    }
}
