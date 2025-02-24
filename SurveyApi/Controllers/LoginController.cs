
using SurveyApi.Models;
using SurveyApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly AppDbContext_User _dbContext;


        public LoginController(IConfiguration config, AppDbContext_User dbContext)
        {
            _config = config;
            _dbContext = dbContext;
        }


        //public LoginController(AppDbContext_User dbContext)
        //{
        //    _dbContext = dbContext;
        //}



        //[AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserAccount login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(UserAccount userInfo)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["JwtSettings:Issuer"],
              _config["JwtSettings:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(40),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserAccount AuthenticateUser(UserAccount login_)
        {
            UserAccount user_ = null;

            //CEK USER LOGIN
            if (!string.IsNullOrEmpty(login_.User_Name) && !string.IsNullOrEmpty(login_.Password))
            {
                user_ = _dbContext.userAccount.Where(u => u.User_Name == login_.User_Name && u.Password == login_.Password).SingleOrDefault();
            }
            return user_;
        }

       
    }
}
