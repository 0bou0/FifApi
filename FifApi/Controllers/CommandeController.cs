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

        // GET: api/Commandes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCommandes()
        {
            if (_context.Commandes == null)
            {
                return NotFound();
            }



            return await(from co in _context.Commandes
                         join ut in _context.Utilisateurs on co.IdUtilisateur equals ut.IdUtilisateur
                         join lc in _context.LigneCommandes on co.IdCommande equals lc.IdCommande
                         join st in _context.Stocks on lc.IdStock equals st.IdStock
                         group new { co, ut, lc, st } by co.IdCommande into g
                         select new
                         {
                            IdCommande = g.Key,
                            Pseudo = g.FirstOrDefault()!.ut.PseudoUtilisateur,
                            NomUtilisateur = g.FirstOrDefault()!.ut.NomUtilisateur,
                            PrenomUtilisateur = g.FirstOrDefault()!.ut.PrenomUtilisateur,
                            LigneCommandes = (
                                from lc in _context.LigneCommandes
                                join st in _context.Stocks on lc.IdStock equals st.IdStock
                                where lc.IdCommande == g.Key
                                    select new
                                    {
                                        LigneCommande = lc.CommandeLigne,
                                        Stock = st.IdStock,
                                        Quantite = st.Quantite
                                    }
                            ).ToList()


                         }

                ).ToListAsync();

          
        }
    }
}
