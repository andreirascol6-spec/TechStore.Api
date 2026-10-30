using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TechStore.Wpf.Models
{
    public  class Comanda
    {
        public int Id { get; set; }
        public DateTime DataComanda { get; set; }
      
        public int ProdusId { get; set; }
        public Produs? Produs { get; set; }
        public int Cantitate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total_Plata;

    }
}
