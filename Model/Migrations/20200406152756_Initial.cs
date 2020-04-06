using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreNaam = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "Klanten",
                columns: table => new
                {
                    KlantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoorNaam = table.Column<string>(maxLength: 50, nullable: false),
                    FamilieNaam = table.Column<string>(maxLength: 50, nullable: false),
                    Straat = table.Column<string>(maxLength: 50, nullable: false),
                    HuisNr = table.Column<string>(maxLength: 50, nullable: false),
                    PostCode = table.Column<string>(maxLength: 50, nullable: false),
                    Gemeente = table.Column<string>(maxLength: 50, nullable: false),
                    GebruikersNaam = table.Column<string>(maxLength: 50, nullable: false),
                    Paswoord = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klanten", x => x.KlantId);
                });

            migrationBuilder.CreateTable(
                name: "Voorstellingen",
                columns: table => new
                {
                    VoorstellingsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(maxLength: 50, nullable: false),
                    Uitvoerders = table.Column<string>(maxLength: 50, nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime", nullable: false),
                    Prijs = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    VrijePlaatsen = table.Column<short>(type: "smallint", nullable: false),
                    GenreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voorstellingen", x => x.VoorstellingsId);
                    table.ForeignKey(
                        name: "FK_Voorstelling_Genre",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservaties",
                columns: table => new
                {
                    ReservatieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plaatsen = table.Column<short>(type: "smallint", nullable: false),
                    KlantId = table.Column<int>(nullable: false),
                    VoorstellingsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservaties", x => x.ReservatieId);
                    table.ForeignKey(
                        name: "FK_Reservatie_Klant",
                        column: x => x.KlantId,
                        principalTable: "Klanten",
                        principalColumn: "KlantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservatie_Voorstelling",
                        column: x => x.VoorstellingsId,
                        principalTable: "Voorstellingen",
                        principalColumn: "VoorstellingsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "GenreNaam" },
                values: new object[,]
                {
                    { 1, "Humor" },
                    { 2, "Theater" },
                    { 3, "Muziek" },
                    { 4, "Wereldmuziek" },
                    { 5, "Familie" },
                    { 6, "Dans" },
                    { 7, "Multimedia" },
                    { 8, "Modern klassiek" },
                    { 9, "Muziektheater" },
                    { 10, "Circustheater" }
                });

            migrationBuilder.InsertData(
                table: "Klanten",
                columns: new[] { "KlantId", "FamilieNaam", "GebruikersNaam", "Gemeente", "HuisNr", "Paswoord", "PostCode", "Straat", "VoorNaam" },
                values: new object[,]
                {
                    { 1, "Desmet", "hans", "Brussel", "7", "bolle", "1000", "Keizerslaan", "Hans" },
                    { 2, "Blondeel", "alexandra", "Brussel", "65", "belle", "1000", "Anspachlaan", "Alexandra" }
                });

            migrationBuilder.InsertData(
                table: "Voorstellingen",
                columns: new[] { "VoorstellingsId", "Datum", "GenreId", "Prijs", "Titel", "Uitvoerders", "VrijePlaatsen" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5.0000m, "Rechtstreeks & integraal/Ka-Boom!", "Neveneffecten / Alex Agnew", (short)0 },
                    { 12, new DateTime(2020, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 6.0000m, "Cancion de un amorio", "Bodicxhel", (short)200 },
                    { 20, new DateTime(2020, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 7.0000m, "Lied der rusteloosheid", "Eva De Roovere,Pedro Moutinho & G.de Mol", (short)190 },
                    { 26, new DateTime(2020, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 7.2500m, "Tres cultures por la paz", "Rosa Zaragoza", (short)190 },
                    { 6, new DateTime(2020, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 6.0000m, "Randschade", "fABULEUS", (short)199 },
                    { 11, new DateTime(2020, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 4.0000m, "De fijnste dagen van het jaar", "Het Paleis/Jenny", (short)168 },
                    { 21, new DateTime(2020, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 5.0000m, "Wilde dingen", "Kopergietery", (short)196 },
                    { 23, new DateTime(2020, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 6.0000m, "Een hond in de nacht", "Speeltheater Holland", (short)0 },
                    { 9, new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 8.0000m, "Ma - Earth", "Akram Khan", (short)180 },
                    { 10, new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 7.5000m, "Jerusalem", "Berlin", (short)198 },
                    { 22, new DateTime(2020, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 6.0000m, "Licht", "Bos", (short)94 },
                    { 16, new DateTime(2020, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 8.0000m, "Liederen v. Baert, Debussy, Chausson, Weill", "Bernard Baert & Anna Pardon", (short)198 },
                    { 19, new DateTime(2020, 10, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 8.0000m, "Poulenc / Stravinsky", "Prometheus Ensemble", (short)200 },
                    { 25, new DateTime(2020, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 8.0000m, "To speak or not to speak", "Spectra Ensemble", (short)200 },
                    { 29, new DateTime(2020, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 8.0000m, "Speelt Rzewski", "Frederic Rzewski", (short)160 },
                    { 36, new DateTime(2020, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 7.0000m, "Hendrickx, Xenakis & Tan Dun", "Spiegel Strijkkwartet", (short)180 },
                    { 3, new DateTime(2020, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 6.0000m, "Ano Neko", "Dobet Gnahoré", (short)200 },
                    { 37, new DateTime(2020, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 6.0000m, "Cry like a man, part 2", "J.Blaute,Paul Michiels & Roland", (short)8 },
                    { 32, new DateTime(2020, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 5.5000m, "White Light White Heat - The Velvet Undergr.", "Bea Van der Maat & Dr Kloot Per W", (short)200 },
                    { 31, new DateTime(2020, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 6.0000m, "Schone woorden klinken zo...", "Warre Borgmans & Jokke Schreurs", (short)180 },
                    { 13, new DateTime(2020, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 6.0000m, "Moest ik van u zijn", "Wouter Deprez", (short)198 },
                    { 14, new DateTime(2020, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 6.5000m, "Poézique cabaret", "La du chien qui tousse", (short)200 },
                    { 33, new DateTime(2020, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 6.0000m, "Het gaat toch rap", "Raf Coppens", (short)170 },
                    { 2, new DateTime(2020, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7.0000m, "De leeuw van Vlaanderen", "Union Suspecte / Publiekstheater", (short)141 },
                    { 4, new DateTime(2020, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7.500m, "Professor Bernhardi", "de Roovers", (short)180 },
                    { 5, new DateTime(2020, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7.0000m, "Ich bin wie du", "het Toneelhuis", (short)150 },
                    { 7, new DateTime(2020, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7.0000m, "Koning Lear", "Tonic", (short)170 },
                    { 27, new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 5.0000m, "Zouff!", "Les Argonautes", (short)200 },
                    { 15, new DateTime(2020, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 5.500m, "Eekhoornbrood", "Lampe", (short)180 },
                    { 24, new DateTime(2020, 10, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7.0000m, "Gloed", "theater Malpertuis", (short)196 },
                    { 34, new DateTime(2020, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7.0000m, "Emilia Galotti", "Tijd", (short)198 },
                    { 35, new DateTime(2020, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 6.0000m, "Iets over de liefde", "theater Malpertuis", (short)160 },
                    { 38, new DateTime(2020, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7.0000m, "De Kreutzersonates", "Het Net", (short)100 },
                    { 8, new DateTime(2020, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 5.0000m, "Van alle landen thuis", "Els Helewaut,D.Van Esbroeck & co", (short)200 },
                    { 18, new DateTime(2020, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 6.0000m, "Achter 't eten", "Ceremonia/Het muziek Lod / Theater Zuidpool", (short)180 },
                    { 30, new DateTime(2020, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 7.0000m, "Tv-tunes K.N.T.", "Wim Opbrouck & Maandacht", (short)200 },
                    { 17, new DateTime(2020, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7.0000m, "L'Hafa", "Union Suspecte", (short)200 },
                    { 28, new DateTime(2020, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 6.0000m, "La cucina dell'arte", "David & Danny Ronaldo", (short)190 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservaties_KlantId",
                table: "Reservaties",
                column: "KlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservaties_VoorstellingsId",
                table: "Reservaties",
                column: "VoorstellingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Voorstellingen_GenreId",
                table: "Voorstellingen",
                column: "GenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservaties");

            migrationBuilder.DropTable(
                name: "Klanten");

            migrationBuilder.DropTable(
                name: "Voorstellingen");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
