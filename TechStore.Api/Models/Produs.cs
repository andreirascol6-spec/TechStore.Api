using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.Api.Models
{
    public class Produs
    {
        public int Id { get; set; }
        [Column("Denumire")]
        public string Nume { get; set; } = string.Empty;
        public string Categorie { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Pret {  get; set; }
        public int Stoc { get; set; }

        public int StocMinim { get; set; }

    }
}
