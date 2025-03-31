using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
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


        public async Task<List<Stock>> GetAllStocksAsync(QueryObject query)
        {
            var stocks = _context.Stocks
                .Include(x => x.Comments)
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(x => x.CompanyName.Contains(query.CompanyName));
            }
            if(!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(x => x.Symbol.Contains(query.Symbol));
            }
            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDecsending ? 
                        stocks.OrderByDescending(x => x.Symbol) : 
                        stocks.OrderBy(x => x.Symbol);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks
                .Skip(skipNumber)
                .Take(query.PageSize)
                .ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int? id)
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

        public async Task<Stock?> UpdateStockRequest(int? id, UpdateDto updateDto)
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
        
        public async Task<Stock?> RemoveStock(int? id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id) as Stock;

            if(stock == null) return null;

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return stock;
        }

        public async Task<bool> StockExists(int? id)
        {
            return await _context.Stocks.AnyAsync(x => x.Id == id);
        }
    }
}