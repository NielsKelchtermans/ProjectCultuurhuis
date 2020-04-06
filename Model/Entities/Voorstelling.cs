using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Entities
{
    public class Voorstelling
    {
        //props
        public int VoorstellingsId { get; set; }
        public string Titel { get; set; }
        public string Uitvoerders { get; set; }
        public DateTime Datum { get; set; }
        public decimal Prijs { get; set; }
        public int VrijePlaatsen { get; set; }
        public int GenreId { get; set; }
        //navigation
        public virtual Genre Genre { get; set; }
        public virtual ICollection<Reservatie> Reservaties { get; set; } = new List<Reservatie>();
    }
}
