using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
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
        public async Task<ActionResult<List<Stock>>> GetAll()
        {
            var stocks = await _context.Stocks.ToListAsync();

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Stock>> GetById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync() as Stock;

            if (stock == null) return NotFound("Invalid Id");
            return Ok(stock);
        }
    }
}