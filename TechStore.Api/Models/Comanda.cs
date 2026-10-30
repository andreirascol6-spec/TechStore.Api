using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.Api.Models
{
    public class Comanda
    {
        public int Id { get; set; }
        public DateTime DataComanda { get; set; }
        public string Nume { get; set; } = string.Empty;
        public int ProdusId { get; set; }
        public Produs? Produs { get; set; }
        public int Cantitate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total_Plata;

       



    }
}
