using System;
using System.Collections.Generic;
using System.Linq;
using Model.Entities;
using Model.Services;

namespace UI
{
    class Program
    {
        static ICollection<WinkelmandjeItem> WinkelMandje = new List<WinkelmandjeItem>();   //net aangemaakt, nog leeg in Entities
        static CultuurServices Service = new CultuurServices();
        static Klant Klant;
        static void Main(string[] args)
        {
            Console.WriteLine("=============================");
            Console.WriteLine("H E T C U L T U U R H U I S");
            Console.WriteLine("=============================");

            KiesHoofdmenu();

            Console.WriteLine("\nWij danken u om te winkelen. Tot de volgend keer....");
            Console.ReadKey();


        }

        public static void KiesHoofdmenu()
        {
            string[] input = { "A", "B", "W", "X" };
            var keuze = "";

            while (keuze != "X")
            {
                Console.WriteLine();
                Console.WriteLine($"=================");
                Console.WriteLine($"H O O F D M E N U - {(Klant == null ? "Niet ingelogd" : Klant.GebruikersNaam)}");
                Console.WriteLine($"=================");
                Console.WriteLine("<A>ccount");
                Console.WriteLine("<B>estellen");
                Console.WriteLine("<W>inkelmandje");
                Console.WriteLine("e<X>it");
                Console.WriteLine();
                Console.Write("Geef uw keuze: ");
                keuze = Console.ReadLine().ToUpper();

                while (!input.Contains(keuze))   //
                {
                    Console.WriteLine("Geef uw keuze:");
                    keuze = Console.ReadLine().ToUpper();

                }
                switch (keuze)
                {
                    case "A":
                        KiesAccountMenu();
                        break;
                    case "B":
                        KiesGenre();
                        break;
                    case "W":
                        KiesWinkelmandjeMenu();
                        break;
                }
            }

        }

        public static void KiesAccountMenu()
        {
            string[] input = { "I", "U", "R", "X" };
            var keuze = "";
            while (keuze != "X")
            {
                Console.WriteLine();
                Console.WriteLine($"===================");
                Console.WriteLine($"A C C O U T M E N U - {(Klant == null ? "Niet ingelogd" : Klant.GebruikersNaam)}");
                Console.WriteLine($"===================");
                Console.WriteLine("<I>nloggen");
                Console.WriteLine("<U>itloggen");
                Console.WriteLine("<R>egistreren");
                Console.WriteLine("e<X>it");
                Console.WriteLine();
                Console.Write("Geef uw keuze: ");
                keuze = Console.ReadLine().ToUpper();
                while (!input.Contains(keuze))
                {
                    Console.Write("Geef uw keuze: ");
                    keuze = Console.ReadLine().ToUpper();
                }
                switch (keuze)
                {
                    case "I":
                        Inloggen();
                        break;
                    case "U":
                        Uitloggen();
                        break;
                    case "R":
                        Registreren();
                        break;

                }
            }

        }

        public static void KiesWinkelmandjeMenu()
        {
            string[] input = { "T", "V", "A", "X" };
            var keuze = "";
            while (keuze != "X")
            {
                Console.WriteLine();
                Console.WriteLine($"===============================");
                Console.WriteLine($"W I N K E L M A N D J E M E N U - {(Klant == null ? "Niet ingelogd" : Klant.GebruikersNaam)}");
                Console.WriteLine($"===============================");
                Console.WriteLine("<T>onen");
                Console.WriteLine("<V>erwijderen");
                Console.WriteLine("<A>frekenen");
                Console.WriteLine("e<X>it");
                Console.WriteLine();
                Console.Write("Geef uw keuze: ");
                keuze = Console.ReadLine().ToUpper();
                while (!input.Contains(keuze))
                {
                    Console.Write("Geef uw keuze: ");
                    keuze = Console.ReadLine().ToUpper();
                }
                switch (keuze)
                {
                    case "T":
                        ToonMandje();
                        break;
                    case "V":
                        VerwijderBestelling();
                        break;
                    case "A":
                        Afrekenen();
                        break;
                }
            }

        }
        static public void KiesGenre()
        {
            var stop = false;
            Genre genre;

            while (!stop)
            {
                ToonGenreMenu();
                var result = LeesGenre();
                genre = result.Item1;
                stop = result.Item2;

                while ((genre is null) & !stop) //id is ongeldig
                {
                    result = LeesGenre();
                    genre = result.Item1;
                    stop = result.Item2;
                }
                if (genre!=null)
                {
                    Console.WriteLine($"Keuze = {genre.GenreId} / {genre.GenreNaam}");
                    KiesVoorstelling(genre);
                }
            }
        }
        public static void ToonGenreMenu()
        {
            Console.WriteLine();
            Console.WriteLine("===========");
            Console.WriteLine("G E N R E S");
            Console.WriteLine("===========");
            Console.WriteLine("Id\tGenreNaam");
            Console.WriteLine("--\t---------");

            //elke genreId en genreNaam weergeven
            foreach (var genreke in Service.GetAllGenres())
            {
                Console.WriteLine($"{genreke.GenreId}\t{genreke.GenreNaam}");

            }
            Console.WriteLine("<Enter> = Terug");
            Console.WriteLine();
        }
        public static Tuple<Genre, bool> LeesGenre()
        {
            Genre genre = null;
            int genreId;

            Console.Write("Geef een correct GenreId: ");
            var keuze = Console.ReadLine();
            if (keuze!=string.Empty)
            {
                int.TryParse(keuze, out genreId);
                genre = Service.GetGenre(genreId);
            }
            return new Tuple<Genre, bool>(genre, keuze == string.Empty ? true : false);    //true als lege string voor de rest een genreobject meegeven

        }

        public static void KiesVoorstelling(Genre genre)
        {
            var stop = false;
            Voorstelling voorstelling;

            while (!stop)
            {
                ToonVoorstellingMenu(genre);
                var result = LeesVoorstelling();
                voorstelling = result.Item1;
                stop = result.Item2;
                while ((voorstelling is null)&(!stop))
                {
                    result = LeesVoorstelling();
                    voorstelling = result.Item1;
                    stop = result.Item2;
                }
                if (voorstelling!=null)
                {
                    var winkelMandjeItem = (from item in WinkelMandje
                                            where item.Voorstelling.VoorstellingsId == voorstelling.VoorstellingsId
                                            select item).FirstOrDefault();
                    if (winkelMandjeItem !=null)
                    {
                        Console.WriteLine("Deze voorstelling werd  reeds opgenomen in het winkelmandje>>> vervangen?");
                        if (Bevestig("Bevestiging vervangen (Y/N): "))
                        {
                            WinkelMandje.Remove(winkelMandjeItem);
                        }
                        else
                        {
                            return;
                        }
                    }
                    Console.WriteLine($"Keuze = {voorstelling.VoorstellingsId} / {voorstelling.Titel}");
                    ReserveerTickets(voorstelling);
                }
                
            }
        }

        public static bool Bevestig(string text)
        {
            Console.WriteLine(text);
            var keuze = Console.ReadLine().ToUpper();
            while ((keuze !="Y")&& (keuze!="N"))
            {
                Console.WriteLine(text);
                keuze = Console.ReadLine().ToUpper();
            }
            return (keuze == "Y") ? true : false;
        }

        public static void ToonVoorstellingMenu(Genre genre)
        {
            var voorstellingenLijst = Service.GetAlleVoorstellingenVanGenre(genre.GenreId);
            if (voorstellingenLijst.Count()==0)
            {
                Console.WriteLine("Er zijn geen voorstellingen van dit genre beschikbaar");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine($"===========================");
                Console.WriteLine($"V O O R S T E L L I N G E N voor {genre.GenreNaam}");
                Console.WriteLine($"===========================");
                Console.WriteLine("Id\tKeuze\tDatum\tTitel\tUitvoerders\tPrijs\tVrije Plaatsen\tReserveren");
                Console.WriteLine("--\t-----\t-----\t-----\t-----------\t-----\t--------------\t----------");

                foreach (var item in voorstellingenLijst)
                {
                    Console.WriteLine($"{item.VoorstellingsId} -{ item.Datum}\t{ item.Titel}\t{ item.Uitvoerders}\t{ item.Prijs}" +
                        $"\t{ item.VrijePlaatsen}\t{(item.VrijePlaatsen > 0 ?"Reserveer" : "Volzet")}");

                }
                Console.WriteLine("<Enter> = Terug");
                Console.WriteLine();

            }
        }
        public static Tuple<Voorstelling, bool> LeesVoorstelling()
        {
            Voorstelling voorstelling = null;
            int voorstellingId;

            Console.WriteLine("Geef een correcte VoorstellingsId:");
            var keuze = Console.ReadLine();

            if (keuze!=string.Empty)
            {
                int.TryParse(keuze, out voorstellingId);
                voorstelling = Service.GetVoorstelling(voorstellingId);
                int vrijePlaatsen = voorstelling is null ? int.MinValue : voorstelling.VrijePlaatsen;

                if ((voorstelling!=null)&(vrijePlaatsen<=0))
                {
                    Console.WriteLine("Voor deze voorstelling zijn geen vrije kaarten meer");
                    voorstelling = null;
                }

            }
            return new Tuple<Voorstelling, bool>(voorstelling, keuze == string.Empty ? true : false);

        }
        public static void ReserveerTickets(Voorstelling voorstelling)
        {
            var stop = false;
            while (!stop)
            {
                Console.WriteLine();
                Console.WriteLine("===================================");
                Console.WriteLine("R E S E R V E R E N T I C K E T S");
                Console.WriteLine("===================================");

                var result = LeesAantalTickets(voorstelling);
                var aantalTickets = result.Item1;
                stop = result.Item2;
                while ((aantalTickets < 1) & !stop)
                {
                    result = LeesAantalTickets(voorstelling);
                    aantalTickets = result.Item1;
                    stop = result.Item2;
                }
                if (!stop)
                {
                    if (Bevestig("Bevestiging Reserveren (Y/N) : "))
                    {
                        VoegReserveringToeInWinkelmandje(voorstelling, aantalTickets);
                        ToonMandje();
                    }
                    stop = true;
                }

            }
            return;
        }
        public static Tuple<int, bool> LeesAantalTickets(Voorstelling voorstelling)
        {
            int aantalTickets = 0;
            Console.Write($"Geef aantal ticketten 1-{voorstelling.VrijePlaatsen} (<Enter>=Terug): ");
            var keuze = Console.ReadLine();
            if (keuze != string.Empty)
            {
                if (!int.TryParse(keuze, out aantalTickets))
                {
                    Console.WriteLine($"Het aantal tickets moet numerisch zijn...");
                }
                else
                {
                    if ((aantalTickets < 1) | (aantalTickets > voorstelling.VrijePlaatsen))
                    {
                        Console.WriteLine($"Het aantal tickets moet liggen tussen 1 en {voorstelling.VrijePlaatsen}");
                        aantalTickets = 0;
                    }
                }

            }
            return new Tuple<int, bool>(aantalTickets, keuze == string.Empty ? true : false);
        }
        public static void VoegReserveringToeInWinkelmandje(Voorstelling voorstelling,int aantalTickets)
        {
            //In winkelmandje
            var winkelmandjeItem = new WinkelmandjeItem();
            winkelmandjeItem.Voorstelling = voorstelling;
            winkelmandjeItem.AantalPlaatsen = aantalTickets;
            WinkelMandje.Add(winkelmandjeItem);
        }
        public static void ToonMandje()
        {
            Console.WriteLine();
            Console.WriteLine("=======================");
            Console.WriteLine("W I N K E L M A N D J E");
            Console.WriteLine("=======================");
            Console.WriteLine();
            if (WinkelMandje.Count() == 0)
            {
                Console.WriteLine("Uw winkelmandje is leeg...");
            }
            else
            {
                foreach (var item in WinkelMandje)
                {
                    Console.WriteLine($"{item.Voorstelling.VoorstellingsId} " +
                        $"-{ item.Voorstelling.Datum}- { item.Voorstelling.Titel}- " +
                        $"{ item.Voorstelling.Uitvoerders}-{ item.Voorstelling.Prijs}" +
                        $"- { item.AantalPlaatsen}reservatie(s)");
                }
            }
        }
        public static void VerwijderBestelling()
        {
            ToonMandje();
            Console.WriteLine();

            var stop = false;
            WinkelmandjeItem winkelmandjeItem;

            while (!stop)
            {
                var result = LeesItemUitWinkelmandje();
                winkelmandjeItem = result.Item1;
                stop = result.Item2;
                while ((winkelmandjeItem is null) & (!stop))
                {
                    result = LeesItemUitWinkelmandje();
                    winkelmandjeItem = result.Item1;
                    stop = result.Item2;
                }
                if (winkelmandjeItem != null)
                {
                    WinkelMandje.Remove(winkelmandjeItem);
                    Console.WriteLine($"{ winkelmandjeItem.Voorstelling.VoorstellingsId} werd verwijderd.");
                    WinkelMandje.Remove(winkelmandjeItem);
                    ToonMandje();

                }
            }
        }
        public static Tuple<WinkelmandjeItem, bool> LeesItemUitWinkelmandje()
        {
            int voorstellingsid;
            WinkelmandjeItem winkelmandjeItem = null;
            Console.Write("Geef de voorstelling uit het winkelmandje <Enter>=Terug: ");
            var keuze = Console.ReadLine();
            while ((!int.TryParse(keuze, out voorstellingsid)) & (keuze != string.Empty))
            {
                Console.Write("Geef de voorstelling uit het winkelmandje <Enter>=Terug: ");
                keuze = Console.ReadLine();
            }
            if (keuze != string.Empty)
            {
                winkelmandjeItem = (from item in WinkelMandje
                                    where item.Voorstelling.VoorstellingsId == int.Parse(keuze)
                                    select item).FirstOrDefault();
            }
            return new Tuple<WinkelmandjeItem, bool>(winkelmandjeItem, keuze == string.Empty ? true :
            false);
        }
        static public void Afrekenen()
        {
            if (Klant is null)
            {
                Console.WriteLine("Klant is niet ingelogd");
                return;
            }
            Console.WriteLine();
            Console.WriteLine("=================");
            Console.WriteLine("A F R E K E N E N");
            Console.WriteLine("=================");
            Console.WriteLine();

            var gelukteReservaties = new List<WinkelmandjeItem>();
            var mislukteReservaties = new List<WinkelmandjeItem>();
            decimal totaleKostprijs = 0m;

            //update reserveringen
            foreach (var item in WinkelMandje)
            {
                if (item.Voorstelling.VrijePlaatsen >= item.AantalPlaatsen)
                {
                    var reservatie = new Reservatie()
                    {
                        KlantId = Klant.KlantId,
                        VoorstellingsId = item.Voorstelling.VoorstellingsId,
                        Plaatsen = item.AantalPlaatsen
                    };
                    Service.BewaarReservatie(reservatie);     //hier gaat het naar de database!!
                    totaleKostprijs += (item.Voorstelling.Prijs * item.AantalPlaatsen);
                    gelukteReservaties.Add(item);
                    WinkelMandje.Remove(item);

                }
                {
                    mislukteReservaties.Add(item);
                }
            }
            //toon gelukte reservaties
            Console.WriteLine();
            Console.WriteLine("Volgende reserveringen werden met succes uitgevoerd");
            Console.WriteLine("---------------------------------------------------");
            foreach (var item in gelukteReservaties)
            {
                Console.WriteLine($"{item.Voorstelling.VoorstellingsId} " +
                    $"-{ item.Voorstelling.Datum}- { item.Voorstelling.Titel}- " +
                    $"{ item.Voorstelling.Uitvoerders}-{ item.Voorstelling.Prijs}- { item.AantalPlaatsen}");
            }
            Console.WriteLine($"-------------------");
            Console.WriteLine($"Totaal te betalen : {totaleKostprijs}");
            Console.WriteLine($"-------------------");
            // Toon Mislukte reservaties
            Console.WriteLine();
            Console.WriteLine("Volgende reserveringen werden niet uitgevoerd");
            Console.WriteLine("---------------------------------------------");
            foreach (var item in mislukteReservaties)
            {
                Console.WriteLine($"{item.Voorstelling.VoorstellingsId} -{ item.Voorstelling.Datum}" +
                    $"- { item.Voorstelling.Titel}- { item.Voorstelling.Uitvoerders}-{ item.Voorstelling.Prijs}- { item.AantalPlaatsen}");
            }
        }
        public static void Inloggen()
        {
            if (!(Klant is null))
            {
                Console.WriteLine("Klant is reeds ingelogd. Log eerst uit aub");
                return;
            }
            Klant klant = null;
            var gebruikersnaam = "-";

            while (gebruikersnaam!=string.Empty)
            {
                Console.WriteLine();
                Console.WriteLine("===============");
                Console.WriteLine("I N L O G G E N");
                Console.WriteLine("===============");
                Console.Write("Gebruikersnaam <Enter>=Terug: ");
                gebruikersnaam = Console.ReadLine();

                while ((gebruikersnaam!=string.Empty)&(klant is null))
                {
                    Console.Write(" Wachtwoord: ");
                    var wachtwoord = Console.ReadLine();
                    klant = Service.GetKlant(gebruikersnaam, wachtwoord);
                    if (klant is null)
                    {
                        Console.WriteLine("Klant werd niet gevonden. Probeer opnieuw.");
                        Console.Write("Gebruikersnaam <Enter>=Terug: ");
                        gebruikersnaam = Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Inloggen met succes voltooid.");
                        Klant = klant;
                        gebruikersnaam = string.Empty;
                    }

                }
            }
            return;

        }
        public static void Uitloggen()
        {
            if (Klant is null)
            {
                Console.WriteLine("Klant is niet ingelogd");
            }
            Klant = null;
        }

        public static Klant Registreren()
        {
            Klant klant = null;
            var voornaam = "-";

            while (voornaam!=string.Empty)
            {
                Console.WriteLine();
                Console.WriteLine("=====================");
                Console.WriteLine("R E G I S T R E R E N");
                Console.WriteLine("=====================");
                Console.WriteLine();
                Console.Write(" Voornaam <Enter>=Terug: ");
                voornaam = Console.ReadLine();

                while (voornaam !=string.Empty)
                {
                    Console.Write(" Familienaam: ");
                    var familienaam = Console.ReadLine();
                    Console.Write(" Straat: ");
                    var straat = Console.ReadLine();
                    Console.Write(" Huisnr: ");
                    var huisnr = Console.ReadLine();
                    Console.Write(" Postcode: ");
                    var postcode = Console.ReadLine();
                    Console.Write(" Gemeente: ");
                    var gemeente = Console.ReadLine();
                    Console.Write(" Gebruikersnaam: ");
                    var gebruikersnaam = Console.ReadLine();
                    Console.Write(" Wachtwoord: ");
                    var wachtwoord = Console.ReadLine();
                    Console.Write("Wachtwoord bevestigen: ");
                    var wachtwoordbevestiging = Console.ReadLine();
                    Console.WriteLine();

                    //Validatie
                    bool ok = true;

                    if (string.IsNullOrEmpty(familienaam))
                    {
                        Console.WriteLine("familienaam is verplicht.");
                        ok = false;
                    }
                    if (string.IsNullOrEmpty(gebruikersnaam))
                    {
                        Console.WriteLine("Gebruikersnaam is verplicht.");
                        ok = false;
                    }
                    if (string.IsNullOrEmpty(wachtwoord))
                    {
                        Console.WriteLine("wachtwoord is verplicht.");
                        ok = false;
                    }
                    if (string.IsNullOrEmpty(wachtwoordbevestiging))
                    {
                        Console.WriteLine("wachtwoordbevestiging is verplicht.");
                        ok = false;
                    }
                    if (wachtwoord != wachtwoordbevestiging)
                    {
                        Console.WriteLine("Wachtwoord en Wachtwoordbevestiging moeten gelijk zijn.");
                        ok = false;
                    }
                    Console.WriteLine();

                    if (ok)
                    {
                        //Toevoegen klant
                        klant = new Klant()
                        {
                            VoorNaam = voornaam,
                            FamilieNaam = familienaam,
                            Straat = straat,
                            HuisNr = huisnr,
                            PostCode = postcode,
                            Gemeente = gemeente,
                            GebruikersNaam = gebruikersnaam,
                            Paswoord = wachtwoord
                        };
                        Service.VoegKlantToe(klant);
                        Console.WriteLine("De klant werd toegevoegd.");
                        Klant = klant;
                        voornaam = string.Empty;
                    }
                    else
                    {
                        // Opnieuw
                        Console.Write(" Voornaam <Enter>=Terug: ");
                        voornaam = Console.ReadLine();
                    }
                }

            }
            return klant;
        }

    }
}
