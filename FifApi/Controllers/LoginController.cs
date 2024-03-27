using FifApi.Models;
using FifApi.Models.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IDataRepository<Utilisateur> _userRepository;
        private readonly IConfiguration _config;

        public LoginController(IDataRepository<Utilisateur> userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] User login)
        {
            IActionResult response = Unauthorized();
            Utilisateur user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = GenerateJwtToken(user);
                response = Ok(new
                {
                    token = tokenString,
                    //userDetails = user,
                });
            }
            return response;
        }

        private Utilisateur? AuthenticateUser(User utilisateur)
        {
            var usersTask = _userRepository.GetAllAsync();
            var users = usersTask.Result;
            return users.SingleOrDefault(x => x.PseudoUtilisateur.ToLower() == utilisateur.UserName.ToLower() && x.MotDePasse == utilisateur.Password);
        }



        private string GenerateJwtToken(Utilisateur utilisateur)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, utilisateur.IdUtilisateur.ToString()),
                new Claim("role", utilisateur.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
