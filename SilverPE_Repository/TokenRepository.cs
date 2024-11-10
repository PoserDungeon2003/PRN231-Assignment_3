using Microsoft.IdentityModel.Tokens;
using SilverPE_BOs.Models;
using SilverPE_Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SilverPE_Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _durationInMinutes;

        public TokenRepository(string secretKey, string issuer, string audience, int durationInMinutes)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
            _durationInMinutes = durationInMinutes;
        }

        public string GenerateToken(BranchAccount branchAccount)
        {
            var claims = new[]
{
        new Claim(ClaimTypes.Name, branchAccount.FullName),
        new Claim(ClaimTypes.Role, branchAccount.Role.ToString() ?? "2"),
        new Claim(ClaimTypes.NameIdentifier, branchAccount.AccountId.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_durationInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
