using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FifApi.Models.EntityFramework;
using Microsoft.EntityFrameworkCore.Internal;
using NuGet.Protocol;
using Microsoft.AspNetCore.Routing.Constraints;

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly FifaDBContext _context;

        public ProduitsController(FifaDBContext context)
        {
            _context = context;
        }

        // GET: api/Produits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProduits()
        {
            if (_context.Produits == null)
            {
                return NotFound();
            }

            return await (from p in _context.Produits
                join cp in _context.CouleurProduits on p.Id equals cp.IdProduit
                join a in _context.Albums on p.AlbumId equals a.IdAlbum into aGroup
                from a in aGroup.DefaultIfEmpty()
                join aph in _context.AlbumPhotos on a.IdAlbum equals aph.IdAlbum into aphGroup
                from aph in aphGroup.DefaultIfEmpty()
                join ph in _context.Photos on aph.IdPhoto equals ph.IdPhoto into phGroup
                from ph in phGroup.DefaultIfEmpty()
                group new { p, cp, a, aph, ph } by p.Id into g
                select new
                {
                    idProduct = g.Key,
                    title = g.FirstOrDefault()!.p.Name,
                    price = g.Min(x => x.cp.Prix),
                    image = g.FirstOrDefault()!.ph.URL,
                    couleurs = (from cp in _context.CouleurProduits
                    join c in _context.Couleurs on cp.IdCouleur equals c.Id
                    where cp.IdProduit == g.Key
                    select new {
                        prix = cp.Prix,
                        codebarre = cp.CodeBarre,
                        couleur = c.Nom
                    }).ToList()
                }).ToListAsync();
        }

        // GET: api/Produits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetProduitById(int id)
        {
            if (_context.Produits == null)
            {
                return NotFound();
            }

            var produit = await (from p in _context.Produits
                join a in _context.Albums on p.AlbumId equals a.IdAlbum into aGroup
                from a in aGroup.DefaultIfEmpty()
                join aph in _context.AlbumPhotos on a.IdAlbum equals aph.IdAlbum into aphGroup
                from aph in aphGroup.DefaultIfEmpty()
                join ph in _context.Photos on aph.IdPhoto equals ph.IdPhoto into phGroup
                from ph in phGroup.DefaultIfEmpty()
                group new { p, a, aph, ph } by p.Id into g
                select new
                {
                    idProduct = g.Key,
                    title = g.FirstOrDefault()!.p.Name,
                    description = g.FirstOrDefault()!.p.Description,
                    caracteristiques = g.FirstOrDefault()!.p.Caracteristiques,
                    image = g.FirstOrDefault()!.ph.URL,
                    couleurs = (from cp in _context.CouleurProduits
                    join c in _context.Couleurs on cp.IdCouleur equals c.Id
                    where cp.IdProduit == g.Key
                    select new {
                        prix = cp.Prix,
                        codebarre = cp.CodeBarre,
                        couleur = c.Nom
                    }).ToList()
                }).Where(x => x.idProduct == id).ToListAsync();

            if (produit == null)    
            {
                return NotFound();
            }

            return produit;
        }

        [HttpGet("Couleur/{couleur}")]
        public async Task<ActionResult<object>> GetProduitsByColor(string couleur)
        {
            if (_context.Produits == null)
            {
                return NotFound();
            }

            return await (from p in _context.Produits
                    where _context.Couleurs
                        .Where(c => c.Nom == couleur)
                        .SelectMany(c => _context.CouleurProduits
                            .Where(cp => cp.IdCouleur == c.Id && cp.IdProduit == p.Id))
                        .Any()
                    select new
                    {
                        idProduct = p.Id,
                        title = p.Name,
                        price = _context.CouleurProduits
                            .Where(cp => cp.IdProduit == p.Id)
                            .Min(cp => cp.Prix),
                        image = _context.Photos
                            .Join
                            (_context.AlbumPhotos, alb => alb.IdPhoto, pth => pth.IdAlbum, (pth, alb) => new { photo = pth.URL })
                            .Select(aph => aph.photo)
                            .FirstOrDefault(),
                        couleurs = _context.Couleurs
                            .Select(c => new
                            {
                                prix = _context.CouleurProduits
                                    .Where(cp => cp.IdProduit == p.Id && cp.IdCouleur == c.Id)
                                    .Select(cp => cp.Prix)
                                    .First(),
                                codebarre = _context.CouleurProduits
                                    .Where(cp => cp.IdProduit == p.Id && cp.IdCouleur == c.Id)
                                    .Select(cp => cp.CodeBarre)
                                    .FirstOrDefault(),
                                couleur = c.Nom
                            }).Where(c => c.prix != null).ToList()
                    }).ToListAsync();


        }

        [HttpGet("Filter")]
        public async Task<ActionResult<object>> GetProduitsByFilter(string? couleur, string? nation, string? categorie)
        {
            if (_context.Produits == null)
            {
                return NotFound();
            }

            return await (from p in _context.Produits
                          where _context.Couleurs
                              .Where(c => c.Nom == couleur || couleur == null)
                              .SelectMany(c => _context.CouleurProduits
                                  .Where(cp => cp.IdCouleur == c.Id && cp.IdProduit == p.Id))
                              .Any()
                          where _context.TypeProduits
                              .Where(tp => tp.Nom == categorie || categorie == null)
                                  .Where(tp => p.TypeId == tp.Id)
                              .Any()
                          select new
                          {
                              idProduct = p.Id,
                              title = p.Name,
                              price = _context.CouleurProduits
                                  .Where(cp => cp.IdProduit == p.Id)
                                  .Min(cp => cp.Prix),
                              image = _context.Photos
                                  .Join
                                  (_context.AlbumPhotos, alb => alb.IdPhoto, pth => pth.IdAlbum, (pth, alb) => new { photo = pth.URL })
                                  .Select(aph => aph.photo)
                                  .FirstOrDefault(),
                              couleurs = _context.Couleurs
                                  .Select(c => new
                                  {
                                      prix = _context.CouleurProduits
                                          .Where(cp => cp.IdProduit == p.Id && cp.IdCouleur == c.Id)
                                          .Select(cp => cp.Prix)
                                          .First(),
                                      codebarre = _context.CouleurProduits
                                          .Where(cp => cp.IdProduit == p.Id && cp.IdCouleur == c.Id)
                                          .Select(cp => cp.CodeBarre)
                                          .FirstOrDefault(),
                                      couleur = c.Nom
                                  }).Where(c => c.prix != null).ToList()
                          }).ToListAsync();


        }

        // PUT: api/Produits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduit(int id, Produit produit)
        {
            if (id != produit.Id)
            {
                return BadRequest();
            }

            _context.Entry(produit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduitExists(id))
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

        // POST: api/Produits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produit>> PostProduit(Produit produit)
        {
          if (_context.Produits == null)
          {
              return Problem("Entity set 'FifaDBContext.Produits'  is null.");
          }
            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduit", new { id = produit.Id }, produit);
        }

        // DELETE: api/Produits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            if (_context.Produits == null)
            {
                return NotFound();
            }
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProduitExists(int id)
        {
            return (_context.Produits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
