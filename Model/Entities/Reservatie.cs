using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Entities
{
    public class Reservatie
    {
        //props
        public int ReservatieId { get; set; }
        public int Plaatsen { get; set; }
        //navigation
        public int KlantId { get; set; }
        public virtual Klant Klant { get; set; }
        public int VoorstellingsId { get; set; }
        public virtual Voorstelling Voorstelling { get; set; }
    }
}
