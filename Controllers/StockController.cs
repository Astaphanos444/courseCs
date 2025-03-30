using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
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
        private readonly IStockRepository _stockRepository;
        public StockController(ApplicationDBContext context, IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Stock>>> GetAllStocks([FromQuery] QueryObject query)
        {
            var stocks = await _stockRepository.GetAllStocksAsync(query);

            var stocksDtos = stocks.Select(stock => stock.ToStockDto());

            return Ok(stocksDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Stock>> GetStockById([FromRoute] int id)
        {
            var stock = await _stockRepository.GetStockByIdAsync(id);

            if (stock == null) return NotFound("Invalid Id");

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<ActionResult<Stock>> CreateStockRequest([FromBody] CreateStockDto stockModel)
        {
            if(!ModelState.IsValid) return BadRequest();

            var stock = await _stockRepository.CreateStockRequest(stockModel);

            if (stock == null) return NotFound();

            return CreatedAtAction(nameof(GetStockById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Stock>> UpdateStock([FromRoute] int? id, [FromBody] UpdateDto updateDto)
        {
            if(!ModelState.IsValid) return BadRequest();

            var stock = await _stockRepository.UpdateStockRequest(id, updateDto);

            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Stock>> DeleteStock([FromRoute] int? id){
            var stock = await _stockRepository.RemoveStock(id);

            if(stock == null) return NotFound();

            return Ok();
        }
    }
}