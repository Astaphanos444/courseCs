using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Stock>>> GetAllStocks()
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stocksDtos = stocks.Select(stock => stock.ToStockDto());

            return Ok(stocksDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Stock>> GetStockById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync() as Stock;

            if (stock == null) return NotFound("Invalid Id");
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<ActionResult<Stock>> CreateStockRequest([FromBody] CreateStockDto stockModel)
        {
            var stock = stockModel.StockFromCreateStockDto();

            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStockById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Stock>> UpdateStock(long id, [FromBody] UpdateDto updateDto){
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id) as Stock;

            if( stock == null) return NotFound();

            stock.Symbol = updateDto.Symbol;
            stock.CompanyName = updateDto.CompanyName;
            stock.Puchaste = updateDto.Puchaste;
            stock.LastDiv =  updateDto.LastDiv;
            stock.Industry = updateDto.Industry;
            stock.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();

            return Ok(stock.ToStockDto());
        }
    }
}