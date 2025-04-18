using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();

            return portfolio;
        }

        public async Task<Portfolio?> DeleteAsync(AppUser AppUser, string symbol)
        {
            var portfolio = await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == AppUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

            if(portfolio == null) return null;
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();

            return portfolio;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(p => p.AppUserId == user.Id)
                .Select(stock => new Stock {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap,
                }).ToListAsync();
        }
    }
}