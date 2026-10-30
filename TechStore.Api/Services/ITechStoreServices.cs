using TechStore.Api.Models;

namespace TechStore.Api.Services
{
    public interface ITechStoreServices
    {
        Task<List<Produs>> GetAllProduses();
        Task<Produs?> GetProdusById(int id);
        Task<Produs> AddProdus(Produs produs);
        Task<bool> UpdateProdus(int id, Produs produs);
        Task<bool> DeleteProdus(int id);
    }
}
