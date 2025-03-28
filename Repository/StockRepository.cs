using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }


        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _context.Stocks
                .Include(x => x.Comments)
                .ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _context.Stocks
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id  == id) as Stock;
        }

        public async Task<Stock?> CreateStockRequest(CreateStockDto stockModel)
        {
            var stock = stockModel.StockFromCreateStockDto();
            
            if(stock == null) return null;

            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();

            return stock;
        }

        public async Task<Stock?> UpdateStockRequest(long id, UpdateDto updateDto)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id) as Stock;

            if( stock == null) return null;

            stock.Symbol = updateDto.Symbol;
            stock.CompanyName = updateDto.CompanyName;
            stock.Puchaste = updateDto.Puchaste;
            stock.LastDiv =  updateDto.LastDiv;
            stock.Industry = updateDto.Industry;
            stock.MarketCap = updateDto.MarketCap;

           await _context.SaveChangesAsync();

           return stock;
        }
        
        public async Task<Stock?> RemoveStock(long id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id) as Stock;

            if(stock == null) return null;

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return stock;
        }
    }
}