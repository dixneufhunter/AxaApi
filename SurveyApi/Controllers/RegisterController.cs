using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SurveyApi.Models;
using SurveyApi.Data;
using Newtonsoft.Json;
using SurveyApi.DTO.Response;
using Microsoft.EntityFrameworkCore;

namespace SurveyApi.Controllers
{
    
    //[Route("[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class RegisterController : ControllerBase
    {
        private readonly AppDbContext_User _dbContext;

        public RegisterController(AppDbContext_User dbContext)
        {
            _dbContext = dbContext;
        }

        //private readonly IProduct _userService;
        //public UserController(IUserService userService)
        //{
        //    _userService = userService;
        //}

        
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserAccount newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Data tidak valid");
            }

            try
            {
                _dbContext.userAccount.Add(newUser);
                await _dbContext.SaveChangesAsync();
                return Ok(newUser);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Gagal menyimpan data : " + ex.Message);
            }
        }

        private bool ProductExists(string user)
        {
            return _dbContext.userAccount.Any(_user => _user.User_Name == user);
        }


        
    }
}
