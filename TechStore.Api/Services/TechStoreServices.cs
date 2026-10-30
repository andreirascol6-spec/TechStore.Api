using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using TechStore.Api.Data;
using TechStore.Api.Models;

namespace TechStore.Api.Services
{
    public class TechStoreServices : ITechStoreServices
    {
        public readonly AppDbContext _context;
        public TechStoreServices(AppDbContext context)
        {
            _context= context;  
        }


        public async Task<Produs> AddProdus(Produs produs_nou)
        {
            var produs = new Produs
            {
                Nume = produs_nou.Nume,
                Categorie = produs_nou.Categorie,
                Pret = produs_nou.Pret,
                Stoc = produs_nou.Stoc,
                StocMinim = produs_nou.StocMinim
            };
            await _context.Produse.AddAsync(produs);
            await _context.SaveChangesAsync();
            return produs;
        }

        public async Task<bool> DeleteProdus(int id)
        {
            var produs = await _context.Produse.FindAsync(id);
            if (produs == null) return false;
           
            _context.Produse.Remove(produs);
             await _context.SaveChangesAsync();
             return true;
           
        }

        public async Task<List<Produs>> GetAllProduses()
        {
            return await _context.Produse.ToListAsync();
        }

        public async Task<Produs?> GetProdusById(int id)
        {
            return await _context.Produse.FindAsync(id);
        }

        public async Task<bool> UpdateProdus(int id, Produs produs_nou)
        {
            var produs = _context.Produse.Find(id);
            if (produs == null) return false;
            produs.Nume=produs_nou.Nume;
            produs.Pret = produs_nou.Pret;
            produs.Categorie=produs_nou.Categorie;
            produs.Stoc=produs_nou.Stoc;
            produs.StocMinim = produs_nou.StocMinim;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
