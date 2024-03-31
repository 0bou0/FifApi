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
