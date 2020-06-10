using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SuperNova.Models
{
    public class Login
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string SessionKey { get; set; }
    }
}
