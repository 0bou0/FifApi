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

            // Post: api/Categories/Nations
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

        // PUT: api/Categories/Nations/4
        [HttpPut("Nations")]
        public async Task<ActionResult> PutNations([FromBody]Pays pays)
        {
            if (_dbContext.Pays == null)
                return NotFound();


            if (_dbContext.Pays.Where(x => x.IdPays == pays.IdPays).Select(x => x.IdPays).FirstOrDefault() != pays.IdPays)
                return BadRequest();

            _dbContext.Pays.Where(x => x.IdPays == pays.IdPays).FirstOrDefault()!.NomPays = pays.NomPays;

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

        // Post: api/Categories/Categories
        [HttpPost("Categories")]
        public async Task<ActionResult> PostCategories([FromBody]TypeProduit categorie)
        {
            if (_dbContext.TypeProduits == null)
                return NotFound();
            if (_dbContext.TypeProduits.Where(x => x.Nom == categorie.Nom).Select(x => x.Nom).FirstOrDefault() == categorie.Nom ||
                _dbContext.TypeProduits.Where(x => x.Description == categorie.Description).Select(x => x.Description).FirstOrDefault() == categorie.Description)
                return BadRequest("category already in base");

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

        // POST: api/Categories/Couleurs
        [HttpPost("Couleurs")]
        public async Task<ActionResult> PostCouleur([FromBody] Couleur couleur)
        {
            if (_dbContext.Couleurs == null)
                return NotFound();
            if (_dbContext.Couleurs.Where(x => x.Nom == couleur.Nom).Select(x => x.Nom).FirstOrDefault() == couleur.Nom)
                return BadRequest("color already in base");

            _dbContext.Couleurs.Add(couleur);

            await _dbContext.SaveChangesAsync();
            return Ok();
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

        // POST: api/Categories/Tailles
        [HttpPost("Tailles")]
        public async Task<ActionResult> PostTailles([FromBody] Taille taille)
        {
            if (_dbContext.Tailles == null)
                return NotFound();
            if (_dbContext.Tailles.Where(x => x.IdTaille == taille.IdTaille).Select(x => x.IdTaille).FirstOrDefault() == taille.IdTaille)
                return BadRequest("size already in base");

            _dbContext.Tailles.Add(taille);

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("Marques")]
        public async Task<ActionResult<IEnumerable<object>>> GetMarques()
        {
            var marque = await _dbContext.Marques.ToListAsync();
            if (marque == null || !marque.Any())
            {
                return NotFound();
            }

            return marque.Select(m => new
            {
                id = m.IdMarque,
                nom = m.NomMarque
            }).ToList();
        }
    }
}
