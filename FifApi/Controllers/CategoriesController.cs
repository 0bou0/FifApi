using FifApi.Models.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using FifApi.Models;

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IDataRepository<Pays> _paysRepository;
        private readonly IDataRepository<TypeProduit> _typeProduitRepository;
        private readonly IDataRepository<Couleur> _couleurRepository;
        private readonly IDataRepository<Taille> _tailleRepository;

        public CategoriesController(IDataRepository<Pays> paysRepository, IDataRepository<TypeProduit> typeProduitRepository, IDataRepository<Couleur> couleurRepository, IDataRepository<Taille> tailleRepository)
        {
            _paysRepository = paysRepository;
            _typeProduitRepository = typeProduitRepository;
            _couleurRepository = couleurRepository;
            _tailleRepository = tailleRepository;
        }

        // GET: api/Categories/Nations
        [HttpGet("Nations")]
        public async Task<ActionResult<IEnumerable<object>>> GetNations()
        {
            var nations = await _paysRepository.GetAllAsync();
            if (nations == null)
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
            var categories = await _typeProduitRepository.GetAllAsync();
            if (categories == null)
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
            var couleurs = await _couleurRepository.GetAllAsync();
            if (couleurs == null)
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
            var tailles = await _tailleRepository.GetAllAsync();
            if (tailles == null)
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
