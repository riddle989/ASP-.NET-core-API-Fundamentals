using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /* We won't use this class outside of this class, so we just created a nested class */
        public class AuthenticationRequestBody
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        private class CityInfoUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; } = null!;
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string City { get; set; } = null!;

            public CityInfoUser(int userId, 
                string userName, 
                string firstName, 
                string lastName, 
                string city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }
        }

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(
            AuthenticationRequestBody authenticationRequestBody) /* It will automatically deserilize from the request "Body", as class is complex obj */
        {
            /* Step 1. Validate the username/pass */
            /* As we dont have a user table right now, we are using mock */
            var user = ValidateUserCredentials(
                authenticationRequestBody.UserName,
                authenticationRequestBody.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            /* Step 2. Create a token */
            /* We need to sign the token, and for that we need a security key,so lets create it from the "secret" */
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));// Don't store this secret here
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            /* Claims - identity info about the user, we can add any field here*/
            var claimForToken = new List<Claim>();
            claimForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimForToken.Add(new Claim("given_name", user.FirstName));
            claimForToken.Add(new Claim("family_name", user.LastName));
            claimForToken.Add(new Claim("city", user.City));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimForToken,
                DateTime.UtcNow, // the time, before it the token can't be used
                DateTime.UtcNow.AddHours(1), // the validity of the token
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private CityInfoUser  ValidateUserCredentials(string userName, string password)
        {
            /* If you have a user database, check the username and password aginst it */
            /* Just return a mock CityInfoUser */
            return new CityInfoUser(
                1,
                userName ?? "",
                "Tura",
                "Code",
                "Sylhet");
        }
    }
}
