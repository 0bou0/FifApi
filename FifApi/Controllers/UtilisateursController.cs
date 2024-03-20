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

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly FifaDBContext _context;

        public UtilisateursController(FifaDBContext context)
        {
            _context = context;
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

        // GET: api/Utilisateurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilisateur>> GetUtilisateur(int id)
        {
            if (_context.Utilisateurs == null)
            {
                return NotFound();
            }
            var utilisateur = await _context.Utilisateurs.FindAsync(id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return utilisateur;
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

        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.IdUtilisateur)
            {
                return BadRequest();
            }

            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateurExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(User user)
        {
            if (_context.Utilisateurs == null)
            {
                return Problem("Entity set 'FifaDBContext.Utilisateurs'  is null.");
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

            return CreatedAtAction("GetUtilisateur", new { id = utilisateur.IdUtilisateur }, utilisateur);
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            if (_context.Utilisateurs == null)
            {
                return NotFound();
            }
            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            _context.Utilisateurs.Remove(utilisateur);
            await _context.SaveChangesAsync();

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
    }
}
