﻿using System;
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

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly IDataRepository<Produit> _produitRepository;
        private readonly IDataRepository<Album> _albumRepository;
        private readonly IDataRepository<AlbumPhoto> _albumPhotoRepository;
        private readonly IDataRepository<Photo> _photoRepository;
        private readonly IDataRepository<CouleurProduit> _couleurProduitRepository;
        private readonly IDataRepository<Couleur> _couleurRepository;
        private readonly IDataRepository<Stock> _stockRepository;
        private readonly IDataRepository<Taille> _tailleRepository;



        public ProduitsController(
         IDataRepository<Produit> produitRepository,
         IDataRepository<Album> albumRepository,
         IDataRepository<AlbumPhoto> albumPhotoRepository,
         IDataRepository<Photo> photoRepository,
         IDataRepository<CouleurProduit> couleurProduitRepository,
         IDataRepository<Couleur> couleurRepository,
         IDataRepository<Stock> stockRepository,
         IDataRepository<Taille> tailleRepository)
        {
            _produitRepository = produitRepository;
            _albumRepository = albumRepository;
            _albumPhotoRepository = albumPhotoRepository;
            _photoRepository = photoRepository;
            _couleurProduitRepository = couleurProduitRepository;
            _couleurRepository = couleurRepository;
            _stockRepository = stockRepository;
            _tailleRepository = tailleRepository;
        }

        
        // GET: api/Produits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProduits()
        {
            var produits = await _produitRepository.GetAllAsync();

            if (produits == null || !produits.Any())
            {
                return NotFound();
            }

            return Ok(_produitRepository.GetAll().Select(produit => new
            {
                id = produit.Id,
                titre = produit.Name,
                description = produit.Description,
                caracteristiques = produit.Caracteristiques,
                image = _albumRepository.GetAll().Join
                        (
                            _albumPhotoRepository.GetAllAsEnumerable(),
                            x => x.IdAlbum,
                            x => x.IdAlbum,
                            (album, albumPhoto) => new
                            {
                                albumid = album.IdAlbum,
                                url = albumPhoto.PhotoDesAlbums.URL
                            }
                        ).Where(album => album.albumid == produit.AlbumId).FirstOrDefault(),
                couleur = _couleurProduitRepository.GetAll().Join
                        (
                            _couleurRepository.GetAllAsEnumerable(),
                            x => x.IdCouleur,
                            x => x.Id,
                            (couleurproduit, couleur) => new
                            {
                                idproduit = couleurproduit.IdProduit,
                                couleurproduitId = couleurproduit.IdCouleurProduit,
                                prix = couleurproduit.Prix,
                                couleur = couleur.Nom,
                                hexa = couleur.Hexa,
                                taille = _stockRepository.GetAll().Join
                                    (
                                        _tailleRepository.GetAllAsEnumerable(),
                                        x => x.TailleId,
                                        x => x.IdTaille,
                                        (stock, taille) => new
                                        {
                                            stock.CouleurProduitId,
                                            stock.Quantite,
                                            taille.NomTaille
                                        }).Where(stock => stock.CouleurProduitId == couleurproduit.IdCouleurProduit).ToList()
                            }).Where(couleur => couleur.idproduit == produit.Id).ToList()
            }).ToList());
        }

        //private readonly FifaDBContext _context;

        //public ProduitsController( FifaDBContext context)
        //{
        //    _context = context;
        //}

        // GET: api/Produits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetProduitById(int id)
        {
            try
            {
                var product = await _produitRepository.GetByIdAsync(x => x.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(_produitRepository.Where(x => x.Id == id).Select(produit => new
                {
                    id = produit.Id,
                    titre = produit.Name,
                    description = produit.Description,
                    caracteristiques = produit.Caracteristiques,
                    image = _albumRepository.GetAllAsEnumerable().Where(album => album.IdAlbum == produit.AlbumId).Join
                        (
                            _albumPhotoRepository.GetAllAsEnumerable(),
                            x => x.IdAlbum,
                            x => x.IdAlbum,
                            (album, albumPhoto) => albumPhoto.PhotoDesAlbums.URL
                        ).FirstOrDefault(),
                    couleur = _couleurProduitRepository.GetAllAsEnumerable().Where(couleur => couleur.IdProduit == produit.Id).Join
                        (
                            _couleurRepository.GetAllAsEnumerable(),
                            x => x.IdCouleur,
                            x => x.Id,
                            (couleurproduit, couleur) => new
                            {
                                prix = couleurproduit.Prix,
                                couleur = couleur.Nom,
                                hexa = couleur.Hexa,
                                taille = _stockRepository.GetAllAsEnumerable().Where(stock => stock.CouleurProduitId == couleurproduit.IdCouleurProduit).Join
                                    (
                                        _tailleRepository.GetAllAsEnumerable(),
                                        x => x.TailleId,
                                        x => x.IdTaille,
                                        (stock, taille) => new
                                        {

                                            stock.Quantite,
                                            taille.NomTaille
                                        }).ToList()
                            }).ToList()
                }).FirstOrDefault());

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }

        /*
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
        */
        
    }
}
