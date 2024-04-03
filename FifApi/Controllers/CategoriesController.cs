using FifApi.Models.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using FifApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly FifaDBContext _dbContext;

        public CategoriesController(FifaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Categories/Nations
        [HttpGet("Nations")]
        public async Task<ActionResult<IEnumerable<object>>> GetNations()
        {
            var nations = await _dbContext.Pays.ToListAsync();
            if (nations == null || !nations.Any())
            {
                return NotFound();
            }

            return nations.Select(p => new
            {
                id = p.IdPays,
                nom = p.NomPays
            }).ToList();
        }

        [HttpPost("Nations")]
        public async Task<ActionResult> PostNations([FromBody]Pays pays)
        {
            if (_dbContext.Pays == null)
                return NotFound();
            if (_dbContext.Pays.Where(x => x.NomPays == pays.NomPays).Select(x => x.NomPays).FirstOrDefault() == pays.NomPays ||
                _dbContext.Pays.Where(x => x.IdPays == pays.IdPays).Select(x => x.IdPays).FirstOrDefault() == pays.IdPays)
                return BadRequest("nation already in base");
            
            _dbContext.Pays.Add(pays);

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        // GET: api/Categories/Categories
        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<object>>> GetCategories()
        {
            var categories = await _dbContext.TypeProduits.ToListAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound();
            }

            return categories.Select(tp => new
            {
                id = tp.Id,
                nom = tp.Nom
            }).ToList();
        }

        [HttpPost("Categories")]
        public async Task<ActionResult> PostCategories([FromBody]TypeProduit categorie)
        {
            if (_dbContext.TypeProduits == null)
                return NotFound();
            if (_dbContext.TypeProduits.Where(x => x.Nom == categorie.Nom).Select(x => x.Nom).FirstOrDefault() == categorie.Nom ||
                _dbContext.TypeProduits.Where(x => x.Description == categorie.Description).Select(x => x.Description).FirstOrDefault() == categorie.Description)
                return BadRequest("nation already in base");

            _dbContext.TypeProduits.Add(categorie);

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        // GET: api/Categories/Couleurs
        [HttpGet("Couleurs")]
        public async Task<ActionResult<IEnumerable<object>>> GetCouleurs()
        {
            var couleurs = await _dbContext.Couleurs.ToListAsync();
            if (couleurs == null || !couleurs.Any())
            {
                return NotFound();
            }

            return couleurs.Select(c => new
            {
                id = c.Id,
                nom = c.Nom,
                hexa = c.Hexa
            }).ToList();
        }

        // GET: api/Categories/Tailles
        [HttpGet("Tailles")]
        public async Task<ActionResult<IEnumerable<object>>> GetTailles()
        {
            var tailles = await _dbContext.Tailles.ToListAsync();
            if (tailles == null || !tailles.Any())
            {
                return NotFound();
            }

            return tailles.Select(t => new
            {
                id = t.IdTaille,
                nom = t.NomTaille
            }).ToList();
        }
    }
}
