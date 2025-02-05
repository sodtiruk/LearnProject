using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace LearnProject.Utils
{
    public class JwtUtil
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly TimeSpan _expiryDuration;

        public JwtUtil(IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection("JwtSettings");
            _secretKey = jwtSetting["SecretKey"] ?? throw new Exception("secret key not found");
            _issuer = jwtSetting["Issuer"] ?? throw new Exception("issuer not found");
            _expiryDuration = TimeSpan.FromSeconds(int.Parse(jwtSetting["ExpiryDurationInSeconds"] ?? "1"));
        }

        // Generate JWT Token
        public string GenerateToken(string userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, userId),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, unixTimestamp.ToString(), ClaimValueTypes.Integer64)
                };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                claims: claims,
                expires: DateTime.UtcNow.Add(_expiryDuration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Validate JWT Token
        //public ClaimsPrincipal? ValidateToken(string token)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var validationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = false,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = _issuer,
        //        IssuerSigningKey = securityKey,
        //        ClockSkew = TimeSpan.Zero // ลดเวลาให้ตรวจสอบแบบเข้มงวด
        //    };
            
        //    try
        //    {
        //        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
        //        return principal;
        //    }
        //    catch (SecurityTokenExpiredException ex)
        //    {
        //        throw new SecurityTokenExpiredException("Token has expired.", ex); 
        //    }
        //    catch (SecurityTokenException ex)
        //    {
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null; // Token ไม่ถูกต้อง
        //    }
        //}
    }
}
