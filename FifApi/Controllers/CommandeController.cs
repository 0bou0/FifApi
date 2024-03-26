﻿using FifApi.Models;
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

       [HttpGet("{id}")]
        public async Task<ActionResult<Commande>> GetCommandeById(int id)
        {
            var commande = await (from co in _context.Commandes
                                  join ut in _context.Utilisateurs on co.IdUtilisateur equals ut.IdUtilisateur
                                  where co.IdUtilisateur == id
                                  select new Commande
                                  {
                                      IdCommande = co.IdCommande,
                                      IdUtilisateur = ut.IdUtilisateur,
                                      LigneDeLaCommande = (
                                         from lc in _context.LigneCommandes
                                         join st in _context.Stocks on lc.IdStock equals st.IdStock
                                         join cp in _context.CouleurProduits on st.CouleurProduitId equals cp.IdCouleurProduit
                                         join tl in _context.Tailles on st.TailleId equals tl.IdTaille
                                         join pr in _context.Produits on cp.IdProduit equals pr.Id
                                         join cl in _context.Couleurs on cp.IdCouleur equals cl.Id
                                         where lc.IdCommande == co.IdCommande
                                         select new LigneCommande
                                         {
                                             StockLigneCommande = new Stock
                                             {
                                                 IdStock = st.IdStock,
                                                 ProduitEncouleur = new CouleurProduit
                                                 {
                                                     Couleur_CouleurProduit = new Couleur { Id = cl.Id, Nom = cl.Nom },
                                                     Produit_CouleurProduit = new Produit { Id = pr.Id, Name = pr.Name }
                                                 },
                                                 TailleId = tl.NomTaille
                                             },
                                             QuantiteAchat = st.Quantite
                                         }
                                     ).ToList()
                                  }
                        ).FirstOrDefaultAsync();

            if (commande == null)
            {
                return NotFound();
            }

            return commande;
        }



        [HttpPost]
        public async Task<ActionResult<Commande>> PostCommande(int IdUtilisateur, List<CommandLine> commandLines)
        {
            // Créer une nouvelle commande
            Commande commande = new Commande
            {
                IdUtilisateur = IdUtilisateur,
                DateCommande = DateTime.Now
            };

            // Ajouter la nouvelle commande au contexte EF
            _context.Commandes.Add(commande);

            // Enregistrer les modifications dans la base de données
            await _context.SaveChangesAsync();

            // Récupérer l'identifiant de la commande créée
            int commandeId = commande.IdCommande;

            // Parcourir chaque ligne de commande
            foreach (CommandLine commandLine in commandLines)
            {
                // Créer une nouvelle ligne de commande
                LigneCommande ligneCommande = new LigneCommande()
                {
                    IdStock = commandLine.IdStock,
                    IdCommande = commandeId,
                    QuantiteAchat = commandLine.quantite
                };

                // Ajouter la nouvelle ligne de commande au contexte EF
                _context.LigneCommandes.Add(ligneCommande);

                // Mettre à jour le stock
                var stock = await _context.Stocks.FindAsync(ligneCommande.IdStock);
                stock.Quantite -= commandLine.quantite;

                // Enregistrer les modifications dans la base de données
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetStock", commande);
        }

    }
}