﻿using FifApi.Models;
using FifApi.Models.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly FifaDBContext _context;
        private readonly IConfiguration _config;

        public LoginController(FifaDBContext context, IConfiguration config)
        {
            _context = context;
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
        private Utilisateur? AuthenticateUser(User utlilsateur)
        {
            return _context.Utilisateurs.SingleOrDefault(x => x.PseudoUtilisateur.ToLower() == utlilsateur.UserName.ToLower() && x.MotDePasse == utlilsateur.Password);
        }

        private string GenerateJwtToken(Utilisateur utlilsateur)
        {
            var securityKey = new
            SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, utlilsateur.PseudoUtilisateur.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, utlilsateur.MailUtilisateur.ToString()),
                new Claim("role", utlilsateur.Role.ToString()),
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
