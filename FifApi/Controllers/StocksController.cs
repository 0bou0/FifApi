using FifApi.Models.EntityFramework;
using FifApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FifApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IDataRepository<Stock> _repository;

        public StocksController(IDataRepository<Stock> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
        {
            var stocks = await _repository.GetAllAsync(); // Assurez-vous d'ajouter 'await' ici
            if (stocks == null)
            {
                return NotFound();
            }
            return Ok(stocks);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Stock>> GetStock(int id)
        {
            var stock = await _repository.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStock(int id, Stock stock)
        {
            if (id != stock.IdStock)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateAsync(id, stock);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Stock>> PostStock(Stock stock)
        {
            await _repository.AddAsync(stock);
            return CreatedAtAction("GetStock", new { id = stock.IdStock }, stock);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stock = await _repository.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }

        private bool StockExists(int id)
        {
            // You may need to implement this method depending on your IDataRepository<T> interface.
            // Example: return _repository.AnyAsync(id);
            return false;
        }
    }
}
