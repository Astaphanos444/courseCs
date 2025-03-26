using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
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
        public async Task<ActionResult<List<Stock>>> GetAllStocks()
        {
            var stocks = await _stockRepository.GetAllStocksAsync();

            var stocksDtos = stocks.Select(stock => stock.ToStockDto());

            return Ok(stocksDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Stock>> GetStockById([FromRoute] int id)
        {
            var stock = await _stockRepository.GetStockByIdAsync(id);

            if (stock == null) return NotFound("Invalid Id");

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<ActionResult<Stock>> CreateStockRequest([FromBody] CreateStockDto stockModel)
        {
            var stock = await _stockRepository.CreateStockRequest(stockModel);

            if (stock == null) return NotFound();

            return CreatedAtAction(nameof(GetStockById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Stock>> UpdateStock([FromRoute] long id, [FromBody] UpdateDto updateDto){

            var stock = await _stockRepository.UpdateStockRequest(id, updateDto);

            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Stock>> DeleteStock([FromRoute] long id){
            var stock = await _stockRepository.RemoveStock(id);

            if(stock == null) return NotFound();

            return Ok();
        }
    }
}