using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using TechStore.Api.Data;
using TechStore.Api.Models;

namespace TechStore.Api.Services
{
    public class TechStoreOrdersServices : ITechStoreOrdersServices
    {
        private readonly AppDbContext _context;
        public TechStoreOrdersServices(AppDbContext context)
        {
            _context= context;
        }

        public async Task<Comanda?> AddOrder(Comanda comanda)
        {
            
            var produs = await _context.Produse.FindAsync(comanda.ProdusId);

           
            if (produs == null || produs.Stoc < comanda.Cantitate)
            {
                return null; // Semnalăm că comanda nu se poate procesa
            }

         
            produs.Stoc -= comanda.Cantitate;
            comanda.Total_Plata = comanda.Cantitate * produs.Pret;

          
            _context.Comenzi.Add(comanda);
            await _context.SaveChangesAsync();

            return comanda;
        }


        public async Task<bool> DeleteOrder(int id)
        {
            var comanda = await _context.Comenzi
                .Include(c => c.Produs)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comanda == null) return false;

            if (comanda.Produs != null)
            {
                comanda.Produs.Stoc += comanda.Cantitate;
            }

            _context.Comenzi.Remove(comanda);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Comanda>> GetAllOrders()
        {
            return await _context.Comenzi.Include(c => c.Produs).ToListAsync();
        }

        public async Task<Comanda?> GetOrderById(int id)
        {
            return await _context.Comenzi.Include(c => c.Produs).FirstOrDefaultAsync(c => c.Id == id);
            
        }

    }
}
