using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2204M_API.ViewModels;
using BCrypt.Net;
using T2204M_API.Entities;
using T2204M_API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace T2204M_API.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly T2204mApiContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(T2204mApiContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserRegister register) 
        {
            try
            {
                var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
                var hashed = BCrypt.Net.BCrypt.HashPassword(register.Password, salt);
                var user = new User
                {
                    Email = register.Email,
                    Fullname = register.Fullname,
                    Password = hashed
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok(new UserDTO
                {
                    Email = user.Email,
                    Fullname = user.Fullname
                    ,
                    Token = GenJWT(user)
                });
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        private string GenJWT(User user)
        {
            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signatureKey = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            var payload = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,"user")
            };
            var token = new JwtSecurityToken(
                    _configuration["JWT:Issuer"],
                    _configuration["JWT:Audience"],
                    payload,
                    expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["Jwt:LifeTime"])),
                    signingCredentials:signatureKey
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserLogin login)
        {
            try
            {
                var user = _context.Users.Where(u => u.Email.Equals(login.Email)).First();
                if (user == null)
                    return Unauthorized();
                bool verifiedPassword = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
                if (!verifiedPassword)
                    return Unauthorized();
                return Ok(new UserDTO()
                {
                    Email = user.Email,
                    Fullname = user.Fullname,
                    Token = GenJWT(user)
                });
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpGet]
        [Route("profile")]
        //[Authorize(Roles = "user")]
        [Authorize(Policy = "ValidYearOld")]
        public IActionResult Profile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (!identity.IsAuthenticated)
                return Unauthorized("Not Authorized");
            try
            {
                var userClaims = identity.Claims;
                var userId = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = _context.Users.Find(Convert.ToInt32(userId));
                return Ok(new UserDTO{Email=user.Email,Fullname=user.Fullname });
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

