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
                        Registeren();
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
    }
}
