using System;
using System.Collections.Generic;
using System.Text;
using Model.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Model.Services
{ 
    public class CultuurServices
    {
        //methods

        public List<Genre> GetAllGenres()
        {
            using (var context = new EFCultuurhuisContext())
            {
                return context.Genres.OrderBy(m => m.GenreNaam).ToList();
            }
        }

        public Genre GetGenre(int? id)
        {
            using (var context = new EFCultuurhuisContext())
            {
                return context.Genres.Find(id);
            }
        }
        
        public List<Voorstelling> GetAlleVoorstellingenVanGenre(int? id)
        {
            using (var context = new EFCultuurhuisContext())
            {
                var query = from voorstelling in context.Voorstellingen.Include("Genres")
                            where voorstelling.GenreId == id
                            orderby voorstelling.Datum
                            select voorstelling;

                return query.ToList();
            }
        }

        public Voorstelling GetVoorstelling(int? id)
        {
            using (var context = new EFCultuurhuisContext())
            {
                return context.Voorstellingen.Find(id);
            }
        }

        public Klant GetKlant(string naam, string paswoord)
        {
            using (var context = new EFCultuurhuisContext())
            {
                return (from klant in context.Klanten
                        where klant.GebruikersNaam.ToUpper() == naam.ToUpper() && klant.Paswoord == paswoord
                        select klant).FirstOrDefault();
            }
        }

        public void VoegKlantToe(Klant nieuweKlant)
        {
            using (var context = new EFCultuurhuisContext())
            {
                context.Klanten.Add(nieuweKlant);
                context.SaveChanges();
            }
        }

        public void BewaarReservatie(Reservatie gelukteReservatie)
        {
            using (var context = new EFCultuurhuisContext())
            {
                var voorstelling = GetVoorstelling(gelukteReservatie.VoorstellingsId);
                voorstelling.VrijePlaatsen -= gelukteReservatie.Plaatsen;
                context.Reservaties.Add(gelukteReservatie);
                context.SaveChanges();
            }
        }
    }
}
