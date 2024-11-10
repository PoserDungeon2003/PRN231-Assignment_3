using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverPE_Repository.Request
{
    public class AccountLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
