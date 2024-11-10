using SilverPE_BOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverPE_Repository.Response
{
    public class AccountLoginResponse
    {
        public string Token { get; set; } = null!;
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int? Role { get; set; }
    }
}
