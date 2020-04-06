using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Entities
{
    public class Genre
    {
        //props
        public int GenreId { get; set; }
        public string GenreNaam { get; set; }

        //navigation
        public virtual ICollection<Voorstelling> Voorstellingen { get; set; } = new List<Voorstelling>();
    }
}
