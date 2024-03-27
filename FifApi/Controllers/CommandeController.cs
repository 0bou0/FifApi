using FifApi.Models;
using FifApi.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandeController : ControllerBase
    {
        private readonly IDataRepository<Commande> _commandeRepository;
        private IDataRepository<LigneCommande> _lignecommanderepo;

       

        public CommandeController(IDataRepository<Commande> commandeRepository, IDataRepository<LigneCommande> lignecommanderepo)
        {
            _commandeRepository = commandeRepository;
            _lignecommanderepo = lignecommanderepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Commande>>> GetCommandes()
        {
            var commandes = await _commandeRepository.GetAllAsync(); // Assurez-vous d'ajouter 'await' ici
            if (commandes == null)
            {
                return NotFound();
            }
            return Ok(commandes);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Commande>> GetCommandeById(int id)
        {
            var commande = await _commandeRepository.GetByIdAsync(id);
            if (commande == null)
            {
                return NotFound();
            }

            return Ok(commande);
        }

        [HttpPost]
        public async Task<ActionResult<Commande>> PostCommande(int IdUtilisateur, List<CommandLine> commandLines)
        {
            // Création d'une nouvelle commande
            Commande commande = new Commande
            {
                IdUtilisateur = IdUtilisateur,
                DateCommande = DateTime.Now
            };

            // Ajouter la commande en utilisant le référentiel
            var newCommande = await _commandeRepository.AddAsync(commande);

            // Traiter les lignes de commande
            foreach (CommandLine commandLine in commandLines)
            {
                // Créer une nouvelle ligne de commande
                LigneCommande ligneCommande = new LigneCommande()
                {
                    IdStock = commandLine.IdStock,
                    IdCommande = newCommande.IdCommande, // Utiliser l'ID de la nouvelle commande
                    QuantiteAchat = commandLine.quantite
                };

                // Ajouter la ligne de commande en utilisant le référentiel
                await _lignecommanderepo.AddAsync(ligneCommande);
            }

            return CreatedAtAction("GetStock", newCommande);
        }

        private bool CommandeExists(int id)
        {
            // Vérifier si une commande existe en utilisant le référentiel
            return _commandeRepository.GetByIdAsync(id) != null;
        }
    }
}
