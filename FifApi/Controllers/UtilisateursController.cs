using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FifApi.Models.EntityFramework;
using NuGet.Protocol;
using FifApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using NuGet.Common;

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly FifaDBContext _context;
        private readonly IConfiguration _config;

        public UtilisateursController(FifaDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/Utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
          if (_context.Utilisateurs == null)
          {
              return NotFound();
          }
            return await _context.Utilisateurs.ToListAsync();
        }


        // GET: api/Utilisateurs/checkusername/bou
        [HttpGet("checkusername/{username}")]
        public async Task<ActionResult<object>> ChekUsername(string username)
        {
            if (_context.Utilisateurs == null)
            {
                return NotFound();
            }
            return new { username = (await _context.Utilisateurs.Where(x => x.PseudoUtilisateur == username).CountAsync()) == 0 }.ToJson();
        }

        // GET: api/Utilisateurs/checkemail/bou.bou@gmail.com
        [HttpGet("checkemail/{email}")]
        public async Task<ActionResult<object>> ChekEmail(string email)
        {
            if (_context.Utilisateurs == null)
            {
                return NotFound();
            }
            return new { email = (await _context.Utilisateurs.Where(x => x.MailUtilisateur == email).CountAsync()) == 0 }.ToJson();
        }

        // PUT: api/Utilisateurs/ViewUtilisateur
        [HttpPut("ViewUtilisateur")]
        public async Task<ActionResult<object>> ViewUtilisateur([FromBody] User user)
        {
            if (_context.Utilisateurs == null)
            {
                return NotFound();
            }
            try
            {
                SecurityToken token;

                ClaimsPrincipal claims = CheckToken(user, out token);

                var utilisateur = await _context.Utilisateurs.Where(x => x.IdUtilisateur.ToString() == claims.Claims.ToList()[0].Value).FirstOrDefaultAsync();

                if (utilisateur == null)
                {
                    return NotFound();
                }

                return new
                {
                    UserName = utilisateur.PseudoUtilisateur,
                    Email = utilisateur.MailUtilisateur,
                    FirstName = utilisateur.PrenomUtilisateur,
                    LastName = utilisateur.NomUtilisateur
                };
            }
            catch(Exception e)
            {
                return Unauthorized(e);
            }



        }

        // PUT: api/Utilisateurs/ChangeUserName
        [HttpPut("ChangeUserName")]
        public async Task<ActionResult<object>> ChangeUserName([FromBody] User user)
        {
            if (_context.Utilisateurs == null)
            {
                return NotFound();
            }
            try
            {
                SecurityToken token;

                ClaimsPrincipal claims = CheckToken(user, out token);

                var utilisateur = await _context.Utilisateurs.Where(x => x.IdUtilisateur.ToString() == claims.Claims.ToList()[0].Value.ToString()).FirstAsync();

                if (utilisateur == null)
                {
                    return NotFound();
                }

                utilisateur.PseudoUtilisateur = user.UserName ?? utilisateur.PseudoUtilisateur;

                await _context.SaveChangesAsync();

                return new { changed = true };

            }
            catch
            {
                return new { changed = false };
            }



        }

        // PUT: api/Utilisateurs/ChangeEmail
        [HttpPut("ChangeEmail")]
        public async Task<ActionResult<object>> ChangeEmail([FromBody] User user)
        {
            if (_context.Utilisateurs == null)
            {
                return NotFound();
            }
            try
            {
                SecurityToken token;

                ClaimsPrincipal claims = CheckToken(user, out token);

                var utilisateur = await _context.Utilisateurs.Where(x => x.IdUtilisateur.ToString() == claims.Claims.ToList()[0].Value.ToString()).FirstAsync();

                if (utilisateur == null)
                {
                    return NotFound();
                }

                utilisateur.MailUtilisateur = user.Email ?? utilisateur.MailUtilisateur;

                await _context.SaveChangesAsync();

                return new { changed = true };

            }
            catch
            {
                return new { changed = false };
            }



        }

        // PUT: api/Utilisateurs/ChangePassword
        [HttpPut("ChangePassword")]
        public async Task<ActionResult<object>> ChangePassword([FromBody] User user)
        {
            if (_context.Utilisateurs == null)
            {
                return NotFound();
            }
            try
            {
                SecurityToken token;

                ClaimsPrincipal claims = CheckToken(user, out token);

                var utilisateur = await _context.Utilisateurs.Where(x => x.IdUtilisateur.ToString() == claims.Claims.ToList()[0].Value.ToString()).FirstAsync();

                if (utilisateur == null)
                {
                    return NotFound();
                }

                if (user.Password != utilisateur.MotDePasse)
                {
                    return BadRequest();
                }

                utilisateur.MotDePasse = user.NewPassword ?? utilisateur.MotDePasse;

                await _context.SaveChangesAsync();

                return new { changed = true };

            }
            catch
            {
                return new { changed = false };
            }



        }

        // PUT: api/Utilisateurs/ChangeLastName
        [HttpPut("ChangeLastName")]
        public async Task<ActionResult<object>> ChangeLastName([FromBody] User user)
        {
            if (_context.Utilisateurs == null)
            {
                return NotFound();
            }
            try
            {
                SecurityToken token;

                ClaimsPrincipal claims = CheckToken(user, out token);

                var utilisateur = await _context.Utilisateurs.Where(x => x.IdUtilisateur.ToString() == claims.Claims.ToList()[0].Value.ToString()).FirstAsync();

                if (utilisateur == null)
                {
                    return NotFound();
                }

                utilisateur.NomUtilisateur = user.LastName ?? utilisateur.NomUtilisateur;

                await _context.SaveChangesAsync();

                return new { changed = true };

            }
            catch
            {
                return new { changed = false };
            }



        }

        // PUT: api/Utilisateurs/ChangeFirstName
        [HttpPut("ChangeFirstName")]
        public async Task<ActionResult<object>> ChangeFirstName([FromBody] User user)
        {
            if (_context.Utilisateurs == null)
            {
                return NotFound();
            }
            try
            {
                SecurityToken token;

                ClaimsPrincipal claims = CheckToken(user, out token);

                var utilisateur = await _context.Utilisateurs.Where(x => x.IdUtilisateur.ToString() == claims.Claims.ToList()[0].Value.ToString()).FirstAsync();

                if (utilisateur == null)
                {
                    return NotFound();
                }

                utilisateur.PrenomUtilisateur = user.FirstName ?? utilisateur.PrenomUtilisateur;

                await _context.SaveChangesAsync();

                return new { changed = true };

            }
            catch
            {
                return new { changed = false };
            }



        }


        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> PutUtilisateur(int id, User user)
        {
            Utilisateur utilisateur = await _context.Utilisateurs.FindAsync(id);

            utilisateur.PseudoUtilisateur = user.UserName ?? utilisateur.PseudoUtilisateur;
            utilisateur.MailUtilisateur = user.Email ?? utilisateur.MailUtilisateur;
            utilisateur.MotDePasse = user.Password ?? utilisateur.MotDePasse;

            await _context.SaveChangesAsync();

            return utilisateur;
        }

        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<object>> PostUtilisateur(User user)
        {
            if (_context.Utilisateurs == null)
            {
                return Problem("Entity set 'FifaDBContext.Utilisateurs'  is null.");
            }

            if (user.UserName == null || user.Email == null || user.Password == null)
            {
                return BadRequest();
            }

            Utilisateur utilisateur = new Utilisateur
            {
                PseudoUtilisateur = user.UserName,
                MailUtilisateur = user.Email,
                MotDePasse = user.Password,
                Role = "user"
            };

            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();

            return new { created = CreatedAtAction("GetUtilisateur", new { id = utilisateur.IdUtilisateur }, utilisateur).StatusCode == StatusCode(201).StatusCode };
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete]
        public async Task<IActionResult> DeleteUtilisateur([FromBody] User user)
        {
            if (_context.Utilisateurs == null)
            {
                return NotFound();
            }
            try
            {
                SecurityToken token;

                ClaimsPrincipal claims = CheckToken(user, out token);

                var utilisateur = await _context.Utilisateurs.Where(x => x.IdUtilisateur.ToString() == claims.Claims.ToList()[0].Value.ToString()).FirstAsync();

                if (utilisateur == null)
                {
                    return NotFound();
                }

                List<Commande> commandes = await _context.Commandes.Where(x => x.IdUtilisateur.ToString() == claims.Claims.ToList()[0].Value).ToListAsync();
                List<InfoCB> infoscb = await _context.InfoCBs.Where(x => x.UtilisateurId.ToString() == claims.Claims.ToList()[0].Value).ToListAsync();
                List<Vote> votes = await _context.Votes.Where(x => x.IdUtilisateur.ToString() == claims.Claims.ToList()[0].Value).ToListAsync();

                foreach (Commande commande in commandes)
                {
                    commande.IdUtilisateur = null;
                }
                foreach (InfoCB infocb in infoscb)
                {
                    _context.InfoCBs.Remove(infocb);
                }
                foreach(Vote vote in votes)
                {
                    _context.Votes.Remove(vote);
                }

                await _context.SaveChangesAsync();


                _context.Utilisateurs.Remove(utilisateur);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Unauthorized();
            }

            return NoContent();
        }





        [HttpGet]
        [Route("GetUserData")]
        [Authorize(Policy = Policies.User)]
        public IActionResult GetUserData()
        {
            return Ok("This is a response from user method");
        }
        [HttpGet]
        [Route("GetAdminData")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult GetAdminData()
        {
            return Ok("This is a response from Admin method");
        }


        private bool UtilisateurExists(int id)
        {
            return (_context.Utilisateurs?.Any(e => e.IdUtilisateur == id)).GetValueOrDefault();
        }

        private ClaimsPrincipal CheckToken(User user, out SecurityToken token)
        {
            return new JwtSecurityTokenHandler().ValidateToken(user.token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"])),
                ClockSkew = TimeSpan.Zero
            }, out token);
        }

    }
}
