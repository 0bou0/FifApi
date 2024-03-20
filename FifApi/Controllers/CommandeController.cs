using FifApi.Models;
using FifApi.Models.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandeController : ControllerBase
    {
        private readonly FifaDBContext _context;

        public CommandeController(FifaDBContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCommandes()
        {
            if (_context.Commandes == null)
            {
                return NotFound();
            }



            return await (from co in _context.Commandes
                          join ut in _context.Utilisateurs on co.IdUtilisateur equals ut.IdUtilisateur
                          select new
                          {
                              IdCommande = co.IdCommande,
                              IdUtilisateur = ut.IdUtilisateur,
                              Date = co.DateCommande,
                              LigneCommandes = (
                                 from lc in _context.LigneCommandes
                                 join st in _context.Stocks on lc.IdStock equals st.IdStock
                                 where lc.IdCommande == co.IdCommande
                                 select new
                                 {
                                     LigneCommande = lc.CommandeLigne,
                                     Stock = st.IdStock,
                                     Quantite = lc.QuantiteAchat
                                 }
                             ).ToList()


                          }

                ).ToListAsync();


        }

        // GET: api/Commande/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCommandeById(int id)
        {
            if (_context.Produits == null)
            {
                return NotFound();
            }

            var produit = await (from co in _context.Commandes
                          join ut in _context.Utilisateurs on co.IdUtilisateur equals ut.IdUtilisateur
                          select new
                          {
                              IdCommande = co.IdCommande,
                              IdUtilisateur = ut.IdUtilisateur,
                              LigneCommandes = (
                                 from lc in _context.LigneCommandes
                                 join st in _context.Stocks on lc.IdStock equals st.IdStock
                                 join cp in _context.CouleurProduits on st.CouleurProduitId equals cp.IdCouleurProduit
                                 join tl in _context.Tailles on st.TailleId equals tl.IdTaille
                                 join pr in _context.Produits on cp.IdProduit equals pr.Id
                                 join cl in _context.Couleurs on cp.IdCouleur equals cl.Id
                                 where lc.IdCommande == co.IdCommande
                                 select new
                                 {
                                     Stock = st.IdStock,
                                     Quantite = st.Quantite,
                                     Couleur = cl.Nom,
                                     Produit = pr.Name,
                                     Taille = tl.NomTaille
                                 }
                             ).ToList()


                          }

                ).Where(x => x.IdUtilisateur == id).ToListAsync();

            if (produit == null)
            {
                return NotFound();
            }

            return produit;
        }


        [HttpPost]
        public async Task<ActionResult<Commande>> PostCommande(int IdUtilisateur, List<CommandLine> commandLines)
        {

            if (_context.Commandes == null)
            {
                return Problem("Entity set 'FifaDBContext.Commandes' is null.");
            }

            // Créer une nouvelle commande
            Commande commande = new Commande
            {
                IdUtilisateur = IdUtilisateur,
                DateCommande = DateTime.Now
            };
            _context.Commandes.Add(commande);
            await _context.SaveChangesAsync();

            // Récupérer l'identifiant de la commande créée
            int commandeId = commande.IdCommande;

            // Parcourir chaque ligne de commande
            foreach (CommandLine commandLine in commandLines)
            {
                // Vérifier si une ligne de commande avec le même stock et la même commande existe déjà
                if (_context.LigneCommandes.Any(x => x.IdStock == commandLine.IdStock && x.IdCommande == commandeId))
                {
                    // Si une telle ligne de commande existe, lever une exception
                    //throw new ArgumentOutOfRangeException("La ligne commande existe déjà.");
                }



                LigneCommande ligneCommande = new LigneCommande()
                {
                    IdStock = commandLine.IdStock,
                    IdCommande = commande.IdCommande,
                    QuantiteAchat = commandLine.quantite
                };
                _context.LigneCommandes.Add(ligneCommande);



                if (_context.Stocks == null)
                {
                    throw new ArgumentOutOfRangeException("Le stock nexiste pas ou est null");
                }
                var stock = await _context.Stocks.FindAsync(ligneCommande.IdStock);

                if (stock == null)
                {
                    return NotFound();
                }
                stock = new Stock
                {
                    IdStock = stock.IdStock,
                    Quantite = stock.Quantite - commandLine.quantite,
                    CouleurProduitId = stock.CouleurProduitId,
                    TailleId = stock.TailleId
                };

                if (stock.Quantite < 0)
                {
                    return Unauthorized("quantité du stock est null ou inferieur à 0");
                }


            }

            await _context.SaveChangesAsync();


            return NoContent();

        }
    }
}