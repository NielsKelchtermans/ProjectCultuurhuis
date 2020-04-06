using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Model.Entities
{
    public class EFCultuurhuisContext : DbContext
    {
        // --------
        // DbSet
        // --------
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Reservatie> Reservaties { get; set; }
        public DbSet<Voorstelling> Voorstellingen { get; set; }

        //const
        const string Server = "localhost";
        const string DatabaseNaam = "EFCultuurhuis";
        const string ConfigNaam = "Cultuurhuis";
        bool testMode = false;

        //constructors
        public EFCultuurhuisContext()
        {

        }

        public EFCultuurhuisContext(DbContextOptions s) : base(s)
        {

        }

        //methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Running Tests
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            //Default voor Migrations (als app.config niet gevonden wordt)
            optionsBuilder.UseSqlServer($@"Server={Server};Database={DatabaseNaam};Trusted_Connection=true"
                , options => options.MaxBatchSize(150));

            //Zoek de naam in de connectionStrings section app.config
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConfigNaam];

            if (settings != null) //indien de naam gevonden is
            {
                switch (settings.ProviderName)
                {
                    case "System.Data.SQLClient":
                        { optionsBuilder.UseSqlServer(settings.ConnectionString, options => options.MaxBatchSize(150)); break; };
                        
                    default:
                        break;
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Klant
            modelBuilder.Entity<Klant>().ToTable("Klanten");
            modelBuilder.Entity<Klant>().HasKey(b => b.KlantId);
            modelBuilder.Entity<Klant>().Property(b => b.VoorNaam).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Klant>().Property(b => b.FamilieNaam).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Klant>().Property(b => b.Straat)
            .IsRequired()
            .HasMaxLength(50);
            modelBuilder.Entity<Klant>().Property(b => b.HuisNr)
            .IsRequired()
            .HasMaxLength(50);
            modelBuilder.Entity<Klant>().Property(b => b.PostCode)
            .IsRequired()
            .HasMaxLength(50);
            modelBuilder.Entity<Klant>().Property(b => b.Gemeente)
            .IsRequired()
            .HasMaxLength(50);
            modelBuilder.Entity<Klant>().Property(b => b.GebruikersNaam)
            .IsRequired()
            .HasMaxLength(50);
            modelBuilder.Entity<Klant>().Property(b => b.Paswoord)
            .IsRequired()
            .HasMaxLength(50);

            //genre
            modelBuilder.Entity<Genre>().ToTable("Genres");
            modelBuilder.Entity<Genre>().HasKey(b => b.GenreId);
            modelBuilder.Entity<Genre>().Property(b => b.GenreNaam)
            .IsRequired()
            .HasMaxLength(50);
            //voorstelling
            modelBuilder.Entity<Voorstelling>().ToTable("Voorstellingen");
            modelBuilder.Entity<Voorstelling>().HasKey(b => b.VoorstellingsId);
            modelBuilder.Entity<Voorstelling>().Property(b => b.Titel)
            .IsRequired()
            .HasMaxLength(50);
            modelBuilder.Entity<Voorstelling>().Property(b => b.Uitvoerders)
            .IsRequired()
            .HasMaxLength(50);
            modelBuilder.Entity<Voorstelling>().Property(b => b.Datum)
            .IsRequired()
            .HasColumnType("datetime");                 //datetime
            modelBuilder.Entity<Voorstelling>().Property(b => b.GenreId)
            .IsRequired();
            modelBuilder.Entity<Voorstelling>().Property(b => b.Prijs)
            .IsRequired()
            .HasColumnType("decimal(18, 4)");           //decimal
            modelBuilder.Entity<Voorstelling>().Property(b => b.VrijePlaatsen)
            .IsRequired()
            .HasColumnType("smallint");     //smallint

            //associatie met Genre (een op veel)
            modelBuilder.Entity<Voorstelling>
                (t =>
                {
                    t.HasOne(b => b.Genre)
                    .WithMany(b => b.Voorstellingen)
                    .HasForeignKey(b => b.GenreId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Voorstelling_Genre");
                }
                );

            //reservatie
            modelBuilder.Entity<Reservatie>
                    (
                    t =>
                    {
                        t.ToTable("Reservaties");
                        t.HasKey(b => b.ReservatieId);
                        t.Property(b => b.KlantId).IsRequired();
                        t.Property(b => b.VoorstellingsId).IsRequired();
                        t.Property(b => b.Plaatsen)
                        .IsRequired()
                        .HasColumnType("smallint"); //
                        t.HasOne(b => b.Klant)
                        .WithMany(b => b.Reservaties)
                        .HasForeignKey(b => b.KlantId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Reservatie_Klant");
                        t.HasOne(b => b.Voorstelling)
                        .WithMany(b => b.Reservaties)
                        .HasForeignKey(b => b.VoorstellingsId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Reservatie_Voorstelling");
                    }
                    );

            if (!testMode)
            {
                // ---------------
                // Seeding Klanten
                // ---------------
                modelBuilder.Entity<Klant>().HasData
                (
                new Klant
                {
                    KlantId = 1,
                    VoorNaam = "Hans",
                    FamilieNaam = "Desmet",
                    Straat = "Keizerslaan",
                    HuisNr
                = "7",
                    PostCode = "1000",
                    Gemeente = "Brussel",
                    GebruikersNaam = "hans",
                    Paswoord = "bolle"
                },
                new Klant
                {
                    KlantId = 2,
                    VoorNaam = "Alexandra",
                    FamilieNaam = "Blondeel",
                    Straat = "Anspachlaan",
                    HuisNr = "65",
                    PostCode = "1000",
                    Gemeente = "Brussel",
                    GebruikersNaam = "alexandra",
                    Paswoord = "belle"
                }
                );
                // --------------
                // Seeding Genres
                // --------------
                modelBuilder.Entity<Genre>().HasData
                (
                new Genre { GenreId = 1, GenreNaam = "Humor" },
                new Genre { GenreId = 2, GenreNaam = "Theater" },
                new Genre { GenreId = 3, GenreNaam = "Muziek" },
                new Genre { GenreId = 4, GenreNaam = "Wereldmuziek" },
                new Genre { GenreId = 5, GenreNaam = "Familie" },
                new Genre { GenreId = 6, GenreNaam = "Dans" },
                new Genre { GenreId = 7, GenreNaam = "Multimedia" },
                new Genre { GenreId = 8, GenreNaam = "Modern klassiek" },
                new Genre { GenreId = 9, GenreNaam = "Muziektheater" },
                new Genre { GenreId = 10, GenreNaam = "Circustheater" }
                );
                // ----------------------
                // Seeding Voorstellingen
                // ----------------------
                modelBuilder.Entity<Voorstelling>().HasData
                (
                new Voorstelling
                {
                    VoorstellingsId = 01,
                    Titel = "Rechtstreeks & integraal/Ka-Boom!",
                    Uitvoerders = "Neveneffecten / Alex Agnew",
                    Datum = new DateTime(2020, 10, 6),
                    GenreId = 1,
                    Prijs = 5.0000m,
                    VrijePlaatsen = 0
                },
                new Voorstelling
                {
                    VoorstellingsId = 02,
                    Titel = "De leeuw van Vlaanderen",
                    Uitvoerders = "Union Suspecte / Publiekstheater",
                    Datum = new DateTime(2020, 10, 7),
                    GenreId = 2,
                    Prijs = 7.0000m,
                    VrijePlaatsen = 141
                },
                new Voorstelling
                {
                    VoorstellingsId = 03,
                    Titel = "Ano Neko",
                    Uitvoerders = "Dobet Gnahoré",
                    Datum
                = new DateTime(2020, 10, 8),
                    GenreId = 4,
                    Prijs = 6.0000m,
                    VrijePlaatsen = 200
                },
                    new Voorstelling
                    {
                        VoorstellingsId = 04,
                        Titel = "Professor Bernhardi",
                        Uitvoerders = "de Roovers",
                        Datum = new DateTime(2020, 10, 9),
                        GenreId = 2,
                        Prijs = 7.500m,
                        VrijePlaatsen = 180
                    },
                new Voorstelling
                {
                    VoorstellingsId = 05,
                    Titel = "Ich bin wie du",
                    Uitvoerders = "het Toneelhuis",
                    Datum = new DateTime(2020, 10, 10),
                    GenreId = 2,
                    Prijs = 7.0000m,
                    VrijePlaatsen = 150
                },
                        new Voorstelling
                        {
                            VoorstellingsId = 06,
                            Titel = "Randschade",
                            Uitvoerders = "fABULEUS",
                            Datum = new DateTime(2020, 10, 11),
                            GenreId = 5,
                            Prijs = 6.0000m,
                            VrijePlaatsen = 199
                        },
                        new Voorstelling
                        {
                            VoorstellingsId = 07,
                            Titel = "Koning Lear",
                            Uitvoerders = "Tonic",
                            Datum = new
                DateTime(2020, 10, 12),
                            GenreId = 2,
                            Prijs = 7.0000m,
                            VrijePlaatsen = 170
                        },
                        new Voorstelling
                        {
                            VoorstellingsId = 08,
                            Titel = "Van alle landen thuis",
                            Uitvoerders = "Els Helewaut,D.Van Esbroeck & co",
                            Datum = new DateTime(2020, 10, 13),
                            GenreId = 3,
                            Prijs = 5.0000m,
                            VrijePlaatsen = 200
                        },
                new Voorstelling
                {
                    VoorstellingsId = 09,
                    Titel = "Ma - Earth",
                    Uitvoerders = "Akram Khan",
                    Datum =
                new DateTime(2020, 10, 14),
                    GenreId = 6,
                    Prijs = 8.0000m,
                    VrijePlaatsen = 180
                },
                            new Voorstelling
                            {
                                VoorstellingsId = 10,
                                Titel = "Jerusalem",
                                Uitvoerders = "Berlin",
                                Datum = new
                DateTime(2020, 10, 15),
                                GenreId = 7,
                                Prijs = 7.5000m,
                                VrijePlaatsen = 198
                            },
                            new Voorstelling
                            {
                                VoorstellingsId = 11,
                                Titel = "De fijnste dagen van het jaar",
                                Uitvoerders =
                "Het Paleis/Jenny",
                                Datum = new DateTime(2020, 10, 16),
                                GenreId = 5,
                                Prijs = 4.0000m,
                                VrijePlaatsen = 168
                            },
                            new Voorstelling
                            {
                                VoorstellingsId = 12,
                                Titel = "Cancion de un amorio",
                                Uitvoerders =
                "Bodicxhel",
                                Datum = new DateTime(2020, 10, 17),
                                GenreId = 4,
                                Prijs = 6.0000m,
                                VrijePlaatsen = 200
                            },
                            new Voorstelling
                            {
                                VoorstellingsId = 13,
                                Titel = "Moest ik van u zijn",
                                Uitvoerders = "Wouter Deprez",
                                Datum = new DateTime(2020, 10, 18),
                                GenreId = 1,
                                Prijs = 6.0000m,
                                VrijePlaatsen = 198
                            },
                    new Voorstelling
                    {
                        VoorstellingsId = 14,
                        Titel = "Poézique cabaret",
                        Uitvoerders = "La du chien qui tousse",
                        Datum = new DateTime(2020, 10, 19),
                        GenreId = 1,
                        Prijs = 6.5000m,
                        VrijePlaatsen = 200
                    },
                            new Voorstelling
                            {
                                VoorstellingsId = 15,
                                Titel = "Eekhoornbrood",
                                Uitvoerders = "Lampe",
                                Datum =
                            new DateTime(2020, 10, 20),
                                GenreId = 2,
                                Prijs = 5.500m,
                                VrijePlaatsen = 180
                            },
                                                    new Voorstelling
                                                    {
                                                        VoorstellingsId = 16,
                                                        Titel = "Liederen v. Baert, Debussy, Chausson, Weill",
                                                        Uitvoerders = "Bernard Baert & Anna Pardon",
                                                        Datum = new DateTime(2020, 10, 21),
                                                        GenreId = 8,
                                                        Prijs = 8.0000m,
                                                        VrijePlaatsen = 198
                                                    },
                                                    new Voorstelling
                                                    {
                                                        VoorstellingsId = 17,
                                                        Titel = "L'Hafa",
                                                        Uitvoerders = "Union Suspecte",
                                                        Datum =
                            new DateTime(2020, 10, 22),
                                                        GenreId = 2,
                                                        Prijs = 7.0000m,
                                                        VrijePlaatsen = 200
                                                    },
                                                    new Voorstelling
                                                    {
                                                        VoorstellingsId = 18,
                                                        Titel = "Achter 't eten",
                                                        Uitvoerders = "Ceremonia/Het muziek Lod / Theater Zuidpool",
                                                        Datum = new DateTime(2020, 10, 23),
                                                        GenreId = 3,
                                                        Prijs = 6.0000m,
                                                        VrijePlaatsen = 180
                                                    },
                            new Voorstelling
                            {
                                VoorstellingsId = 19,
                                Titel = "Poulenc / Stravinsky",
                                Uitvoerders = "Prometheus Ensemble",
                                Datum = new DateTime(2020, 10, 24),
                                GenreId = 8,
                                Prijs = 8.0000m,
                                VrijePlaatsen = 200
                            },
                            new Voorstelling
                            {
                                VoorstellingsId = 20,
                                Titel = "Lied der rusteloosheid",
                                Uitvoerders = "Eva De Roovere,Pedro Moutinho & G.de Mol",
                                Datum = new DateTime(2020, 10, 25),
                                GenreId = 4,
                                Prijs = 7.0000m,
                                VrijePlaatsen = 190
                            },
                            new Voorstelling
                            {
                                VoorstellingsId = 21,
                                Titel = "Wilde dingen",
                                Uitvoerders = "Kopergietery",
                                Datum = new DateTime(2020, 10, 26),
                                GenreId = 5,
                                Prijs = 5.0000m,
                                VrijePlaatsen = 196
                            },
                                new Voorstelling
                                {
                                    VoorstellingsId = 22,
                                    Titel = "Licht",
                                    Uitvoerders = "Bos",
                                    Datum = new
                            DateTime(2020, 10, 27),
                                    GenreId = 7,
                                    Prijs = 6.0000m,
                                    VrijePlaatsen = 94
                                },
                                new Voorstelling
                                {
                                    VoorstellingsId = 23,
                                    Titel = "Een hond in de nacht",
                                    Uitvoerders =
                            "Speeltheater Holland",
                                    Datum = new DateTime(2020, 10, 28),
                                    GenreId = 5,
                                    Prijs = 6.0000m,
                                    VrijePlaatsen = 0
                                },
                                new Voorstelling
                                {
                                    VoorstellingsId = 24,
                                    Titel = "Gloed",
                                    Uitvoerders = "theater Malpertuis",
                                    Datum = new DateTime(2020, 10, 29),
                                    GenreId = 2,
                                    Prijs = 7.0000m,
                                    VrijePlaatsen = 196
                                },
                                new Voorstelling
                                {
                                    VoorstellingsId = 25,
                                    Titel = "To speak or not to speak",
                                    Uitvoerders =
                            "Spectra Ensemble",
                                    Datum = new DateTime(2020, 10, 30),
                                    GenreId = 8,
                                    Prijs = 8.0000m,
                                    VrijePlaatsen = 200
                                },
                                new Voorstelling
                                {
                                    VoorstellingsId = 26,
                                    Titel = "Tres cultures por la paz",
                                    Uitvoerders = "Rosa Zaragoza",
                                    Datum = new DateTime(2020, 10, 31),
                                    GenreId = 4,
                                    Prijs = 7.2500m,
                                    VrijePlaatsen = 190
                                },
                            new Voorstelling
                            {
                                VoorstellingsId = 27,
                                Titel = "Zouff!",
                                Uitvoerders = "Les Argonautes",
                                Datum =
                            new DateTime(2020, 11, 1),
                                GenreId = 10,
                                Prijs = 5.0000m,
                                VrijePlaatsen = 200
                            },
                                    new Voorstelling
                                    {
                                        VoorstellingsId = 28,
                                        Titel = "La cucina dell'arte",
                                        Uitvoerders = "David & Danny Ronaldo",
                                        Datum = new DateTime(2020, 11, 2),
                                        GenreId = 10,
                                        Prijs = 6.0000m,
                                        VrijePlaatsen = 190
                                    },
                            new Voorstelling
                            {
                                VoorstellingsId = 29,
                                Titel = "Speelt Rzewski",
                                Uitvoerders = "Frederic Rzewski",
                                Datum = new DateTime(2020, 11, 3),
                                GenreId = 8,
                                Prijs = 8.0000m,
                                VrijePlaatsen = 160
                            },
                            new Voorstelling
                            {
                                VoorstellingsId = 30,
                                Titel = "Tv-tunes K.N.T.",
                                Uitvoerders = "Wim Opbrouck & Maandacht",
                                Datum = new DateTime(2020, 11, 4),
                                GenreId = 3,
                                Prijs = 7.0000m,
                                VrijePlaatsen = 200
                            },
                            new Voorstelling
                            {
                                VoorstellingsId = 31,
                                Titel = "Schone woorden klinken zo...",
                                Uitvoerders =
                            "Warre Borgmans & Jokke Schreurs",
                                Datum = new DateTime(2020, 11, 5),
                                GenreId = 3,
                                Prijs = 6.0000m,
                                VrijePlaatsen = 180
                            },
                                new Voorstelling
                                {
                                    VoorstellingsId = 32,
                                    Titel = "White Light White Heat - The Velvet Undergr.",
                                    Uitvoerders = "Bea Van der Maat & Dr Kloot Per W",
                                    Datum = new DateTime(2020, 11, 6),
                                    GenreId = 3,
                                    Prijs = 5.5000m,
                                    VrijePlaatsen = 200
                                },
                                new Voorstelling
                                {
                                    VoorstellingsId = 33,
                                    Titel = "Het gaat toch rap",
                                    Uitvoerders = "Raf Coppens",
                                    Datum = new DateTime(2020, 11, 7),
                                    GenreId = 1,
                                    Prijs = 6.0000m,
                                    VrijePlaatsen = 170
                                },
                                new Voorstelling
                                {
                                    VoorstellingsId = 34,
                                    Titel = "Emilia Galotti",
                                    Uitvoerders = "Tijd",
                                    Datum =
                            new DateTime(2020, 11, 8),
                                    GenreId = 2,
                                    Prijs = 7.0000m,
                                    VrijePlaatsen = 198
                                },
                                new Voorstelling
                                {
                                    VoorstellingsId = 35,
                                    Titel = "Iets over de liefde",
                                    Uitvoerders = "theater Malpertuis",
                                    Datum = new DateTime(2020, 11, 9),
                                    GenreId = 2,
                                    Prijs = 6.0000m,
                                    VrijePlaatsen = 160
                                },
                            new Voorstelling
                            {
                                VoorstellingsId = 36,
                                Titel = "Hendrickx, Xenakis & Tan Dun",
                                Uitvoerders =
                            "Spiegel Strijkkwartet",
                                Datum = new DateTime(2020, 11, 10),
                                GenreId = 8,
                                Prijs = 7.0000m,
                                VrijePlaatsen = 180
                            },
                                    new Voorstelling
                                    {
                                        VoorstellingsId = 37,
                                        Titel = "Cry like a man, part 2",
                                        Uitvoerders = "J.Blaute,Paul Michiels & Roland",
                                        Datum = new DateTime(2020, 11, 11),
                                        GenreId = 3,
                                        Prijs = 6.0000m,
                                        VrijePlaatsen = 8
                                    },
                            new Voorstelling
                            {
                                VoorstellingsId = 38,
                                Titel = "De Kreutzersonates",
                                Uitvoerders = "Het Net",
                                Datum = new DateTime(2020, 11, 12),
                                GenreId = 2,
                                Prijs = 7.0000m,
                                VrijePlaatsen = 100
                            }
);
            }


        }
    }
}

