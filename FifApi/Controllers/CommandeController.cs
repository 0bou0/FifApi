using FifApi.Models.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                              IdCommande = co.IdUtilisateur,
                              IdUtilisateur = ut.IdUtilisateur,
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
                              IdCommande = co.IdUtilisateur,
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
        public async Task<ActionResult<Produit>> PostCommande(Commande commande)
        {
            if (_context.Commandes == null)
            {
                return Problem("Entity set 'FifaDBContext.Commandes'  is null.");
            }
            _context.Commandes.Add(commande);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommande", new { id = commande.IdCommande }, commande);
        }
    }
}
