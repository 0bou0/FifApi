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
using FifApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Humanizer;
using FifApi.Models.Products;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
                                                 select new
                                                 {
                                                     prix = cp.Prix,
                                                     codebarre = cp.CodeBarre,
                                                     couleur = c.Nom,
                                                     hexa = c.Hexa,
                                                     taille = (from cp2 in _context.CouleurProduits
                                                               join s in _context.Stocks on cp2.IdCouleurProduit equals s.CouleurProduitId
                                                               join t in _context.Tailles on s.TailleId equals t.IdTaille
                                                               where cp2.IdCouleurProduit == cp.IdCouleurProduit
                                                               select new
                                                               {
                                                                   taille = t.IdTaille,
                                                                   nomtaille = t.NomTaille,
                                                                   description = t.DescriptionTaille,
                                                                   quantite = s.Quantite
                                                               }).ToList(),
                                                     quantite = (from cp2 in _context.CouleurProduits
                                                                 join s in _context.Stocks on cp2.IdCouleurProduit equals s.CouleurProduitId
                                                                 join t in _context.Tailles on s.TailleId equals t.IdTaille
                                                                 where cp2.IdCouleurProduit == cp.IdCouleurProduit
                                                                 select s.Quantite
                                                                 ).Sum()
                                                 }).ToList(),
                                     quantite = (
                                         from cp in _context.CouleurProduits
                                         join c in _context.Couleurs on cp.IdCouleur equals c.Id
                                         where cp.IdProduit == g.Key
                                         select (from cp2 in _context.CouleurProduits
                                                 join s in _context.Stocks on cp2.IdCouleurProduit equals s.CouleurProduitId
                                                 join t in _context.Tailles on s.TailleId equals t.IdTaille
                                                 where cp2.IdCouleurProduit == cp.IdCouleurProduit
                                                 select s.Quantite
                                                 ).Sum()
                                             ).Sum()
                                 }).Where(x => x.idProduct == id).FirstOrDefaultAsync();

            if (produit == null)
            {
                return NotFound();
            }

            return produit;
        }

        // GET: api/Produits/Filter?
        [HttpGet("Filter")]
        public async Task<ActionResult<object>> GetProduitsByFilter(string? couleur, string? nation, string? categorie, string? taille)
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
                          where _context.Pays
                              .Where(pys => pys.NomPays == nation || nation == null)
                                  .Where(pys => p.PaysId == pys.IdPays)
                              .Any()
                          where _context.Tailles
                               .Where(t => t.IdTaille == taille || taille == null)
                               .Any(t => _context.Stocks
                                   .Any(s => s.TailleId == t.IdTaille && _context.CouleurProduits
                                       .Any(cp => cp.IdCouleurProduit == s.CouleurProduitId && cp.IdProduit == p.Id)))
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
                                          select new
                                          {
                                              prix = cp.Prix,
                                              codebarre = cp.CodeBarre,
                                              couleur = c.Nom,
                                              hexa = c.Hexa,
                                              taille = (from cp2 in _context.CouleurProduits
                                                        join s in _context.Stocks on cp2.IdCouleurProduit equals s.CouleurProduitId
                                                        join t in _context.Tailles on s.TailleId equals t.IdTaille
                                                        where cp2.IdCouleurProduit == cp.IdCouleurProduit
                                                        select new
                                                        {
                                                            taille = t.IdTaille,
                                                            nomtaille = t.NomTaille,
                                                            description = t.DescriptionTaille,
                                                            quantite = s.Quantite
                                                        }).ToList(),
                                              quantite = (from cp2 in _context.CouleurProduits
                                                          join s in _context.Stocks on cp2.IdCouleurProduit equals s.CouleurProduitId
                                                          join t in _context.Tailles on s.TailleId equals t.IdTaille
                                                          where cp2.IdCouleurProduit == cp.IdCouleurProduit
                                                          select s.Quantite
                                                          ).Sum()
                                          }).ToList(),
                              quantite = (
                                  from cp in _context.CouleurProduits
                                  join c in _context.Couleurs on cp.IdCouleur equals c.Id
                                  where cp.IdProduit == g.Key
                                  select (from cp2 in _context.CouleurProduits
                                          join s in _context.Stocks on cp2.IdCouleurProduit equals s.CouleurProduitId
                                          join t in _context.Tailles on s.TailleId equals t.IdTaille
                                          where cp2.IdCouleurProduit == cp.IdCouleurProduit
                                          select s.Quantite
                                          ).Sum()
                                      ).Sum()
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
        public async Task<ActionResult<object>> PostProduit(Product product)
        {
            if (_context.Produits == null)
            {
                return Problem("Entity set 'FifaDBContext.Produits'  is null.");
            }

            int idProduit;

            if (!_context.Photos.Any(p => p.URL == product.Image))
            {
                _context.Produits.Add(new Produit
                {
                    Name = product.NomProduit,
                    Description = product.DescriptionProduit,
                    MarqueId = product.Marque,
                    PaysId = product.Nation,
                    TypeId = product.Categorie,
                    AlbumDuProduit = new Album
                    {
                        AlbumDesPhotos = new List<AlbumPhoto> {
                            new AlbumPhoto
                            {
                                PhotoDesAlbums = new Photo
                                {
                                    URL = product.Image
                                }
                            }
                        }
                    }
                });
            }
            else
            {
                if (_context.AlbumPhotos.Any(a => a.IdPhoto == _context.Photos.Where(p => p.URL == product.Image).Select(p => p.IdPhoto).First()))
                {
                    _context.Produits.Add(new Produit
                    {
                        Name = product.NomProduit,
                        Description = product.DescriptionProduit,
                        MarqueId = product.Marque,
                        PaysId = product.Nation,
                        TypeId = product.Categorie,
                        AlbumId = _context.AlbumPhotos.Where(a => a.IdPhoto == _context.Photos.Where(p => p.URL == product.Image).Select(p => p.IdPhoto).First()).Select(a => a.IdAlbum).First()
                    });
                }
                else 
                {
                    _context.Produits.Add(new Produit
                    {
                        Name = product.NomProduit,
                        Description = product.DescriptionProduit,
                        MarqueId = product.Marque,
                        PaysId = product.Nation,
                        TypeId = product.Categorie,
                        AlbumDuProduit = new Album
                        {
                            AlbumDesPhotos = new List<AlbumPhoto>
                            {
                                new AlbumPhoto
                                {
                                    IdPhoto = _context.Photos.Where(p => p.URL == product.Image).Select(p => p.IdPhoto).First()
                                }
                            }
                        }
                    });
                }
            }
            _context.SaveChanges();

            idProduit = _context.Produits.OrderBy(p => p.Id).Last().Id;
            int idCouleurProduit;
            foreach (Color couleur in product.Couleurs)
            {
                _context.CouleurProduits.Add(new CouleurProduit
                {
                    IdCouleur = _context.Couleurs.Where(c => c.Nom == couleur.Nom).Select(c => c.Id).First(),
                    IdProduit = idProduit,
                    Prix = couleur.Prix,
                });
                _context.SaveChanges();
                idCouleurProduit = _context.CouleurProduits.OrderBy(c => c.IdCouleurProduit).Last().IdCouleurProduit;
                foreach (Size taille in couleur.Tailles)
                {
                    await new StocksController(_context).PostStock(new Stock
                    {
                        TailleId = taille.Code,
                        Quantite = taille.Quantite,
                        CouleurProduitId = idCouleurProduit
                    });
                }
            }
            return Ok();
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
