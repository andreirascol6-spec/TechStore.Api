using System.Data;
using TechStore.Api.Models;

namespace TechStore.Api.Services
{
    public interface ITechStoreOrdersServices
    {
        Task<List<Comanda>> GetAllOrders();
        Task<Comanda?> GetOrderById(int id);
        Task<Comanda> AddOrder(Comanda comanda);
        Task<bool> DeleteOrder(int id);
    }
}
