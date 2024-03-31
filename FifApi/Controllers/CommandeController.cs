using FifApi.Models;
using FifApi.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandeController : ControllerBase
    {
        private readonly FifaDBContext _dbContext;



        public CommandeController(FifaDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Commande>>> GetCommandes()
        {
            if (_dbContext.Commandes == null)
            {
                return NotFound();
            }
            return await _dbContext.Commandes.ToListAsync();
        }

        // GET: api/Stocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Commande>> GetCommandeById(int id)
        {
            if (_dbContext.Commandes == null)
            {
                return NotFound();
            }
            var stock = await _dbContext.Commandes.FindAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return stock;
        }

        [HttpPost]
        public async Task<ActionResult<Commande>> PostCommande(int? IdUtilisateur, List<CommandLine> commandLines)
        {
            if (!IdUtilisateur.HasValue)
            {
                // Gérer le cas où IdUtilisateur est null
                return BadRequest("IdUtilisateur cannot be null.");
            }
            // Créer une nouvelle commande
            Commande commande = new Commande
            {
                IdUtilisateur = IdUtilisateur,
                DateCommande = DateTime.Now
            };

            // Ajouter la nouvelle commande au contexte EF
            _dbContext.Commandes.Add(commande);

            // Enregistrer les modifications dans la base de données
            await _dbContext.SaveChangesAsync();

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
                _dbContext.LigneCommandes.Add(ligneCommande);

                // Mettre à jour le stock
                var stock = await _dbContext.Stocks.FindAsync(commandLine.IdStock);
                if (stock == null)
                {
                    return NotFound();
                }

                stock.Quantite -= commandLine.quantite;

                if (stock.Quantite < 0)
                {
                    return Unauthorized("Quantité du stock est nulle ou inférieure à 0");
                }
            }

            // Enregistrer les modifications dans la base de données
            await _dbContext.SaveChangesAsync();

            // Renvoyer la réponse avec l'objet Commande créé
            return CreatedAtAction(nameof(GetCommandeById), new { id = commande.IdCommande }, commande);

        }





    }
}
