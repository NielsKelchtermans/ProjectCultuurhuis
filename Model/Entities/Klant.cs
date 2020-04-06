using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Entities
{
    public class Klant
    {
        //Prop
        public int KlantId { get; set; }
        public string VoorNaam { get; set; }
        public string FamilieNaam { get; set; }
        public string Straat { get; set; }
        public string HuisNr { get; set; }
        public string PostCode { get; set; }
        public string Gemeente { get; set; }
        public string GebruikersNaam { get; set; }
        public string Paswoord { get; set; }

        //navigation
        public virtual ICollection<Reservatie> Reservaties { get; set; } = new List<Reservatie>();
    }
}
