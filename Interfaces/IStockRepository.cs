using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocksAsync();
        Task<Stock?> GetStockByIdAsync(int id);
        Task<Stock?> CreateStockRequest(CreateStockDto createStockDto);
        Task<Stock?> UpdateStockRequest(long id, UpdateDto updateStockDto);
        Task<Stock?> RemoveStock(long id);
    }
}