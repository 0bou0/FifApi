using FifApi.Models.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly FifaDBContext _context;

        public CategoriesController(FifaDBContext context)
        {
            _context = context;
        }

        // GET: api/Coategories/Nations
        [HttpGet("Nations")]
        public async Task<ActionResult<IEnumerable<object>>> GetNations()
        {
            if (_context.Produits == null)
            {
                return NotFound();
            }

            return await (from p in _context.Pays
                          select new
                          {
                              id = p.IdPays,
                              nom = p.NomPays
                          }).ToListAsync();
        }

        // GET: api/Coategories/Categories
        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<object>>> GetCategories()
        {
            if (_context.TypeProduits == null)
            {
                return NotFound();
            }

            return await (from tp in _context.TypeProduits
                          select new
                          {
                              id = tp.Id,
                              nom = tp.Nom
                          }).ToListAsync();
        }

        // GET: api/Coategories/Couleurs
        [HttpGet("Couleurs")]
        public async Task<ActionResult<IEnumerable<object>>> GetCouleurs()
        {
            if (_context.Couleurs == null)
            {
                return NotFound();
            }

            return await (from c in _context.Couleurs
                          select new
                          {
                              id = c.Id,
                              nom = c.Nom
                          }).ToListAsync();
        }

        // GET: api/Coategories/Couleurs
        [HttpGet("Tailles")]
        public async Task<ActionResult<IEnumerable<object>>> GetTailles()
        {
            if (_context.Tailles == null)
            {
                return NotFound();
            }

            return await (from c in _context.Tailles
                          select new
                          {
                              id = c.IdTaille,
                              nom = c.NomTaille
                          }).ToListAsync();
        }
    }
}
