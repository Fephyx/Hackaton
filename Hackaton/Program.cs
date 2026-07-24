using System;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string soubor = "ucty.json";

        List<BankovniUcet> ucty = new List<BankovniUcet>();


        // Načtení všech účtů
        if (File.Exists(soubor))
        {
            string data = File.ReadAllText(soubor);

            ucty = JsonSerializer.Deserialize<List<BankovniUcet>>(data)
                ?? new List<BankovniUcet>();
            if (!ucty.Any(x => x.Admin))
            {
                BankovniUcet admin = new BankovniUcet();

                admin.Jmeno = "Admin";
                admin.Prijmeni = "Acc";
                admin.Pin = HashPin("9999");
                admin.Karta = "ADMIN";
                admin.Zustatek = 0;
                admin.Historie = new List<string>();
                admin.Admin = true;


                ucty.Add(admin);


                File.WriteAllText(
                    soubor,
                    JsonSerializer.Serialize(
                        ucty,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true
                        })
                );
            }
        }


        Console.WriteLine("===== Bank Bank =====");
        Console.WriteLine();


        Console.Write("Zadejte své jméno: ");
        string name = Console.ReadLine();


        Console.Write("Zadejte své příjmení: ");
        string secondname = Console.ReadLine();

        // Hledání účtu
        BankovniUcet ucet = ucty.FirstOrDefault(
            x => x.Jmeno == name && x.Prijmeni == secondname);

        // Pokud účet existuje
        if (ucet != null)
        {
            Console.Clear();

            Console.WriteLine("Účet nalezen!");
            Console.WriteLine($"Vítej zpět {ucet.Jmeno} {ucet.Prijmeni}");
            Console.WriteLine();


            Console.Write("Zadejte PIN: ");
            int zadanyPin = int.Parse(Console.ReadLine());


            int pokusy = 0;


            while (HashPin(zadanyPin.ToString()) != ucet.Pin)
            {
                pokusy++;


                if (pokusy == 3)
                {
                    Console.WriteLine("Účet byl zablokován.");
                    return;
                }


                Console.WriteLine($"Špatný PIN. Zbývá {3 - pokusy} pokusů.");

                Console.Write("Zadejte PIN: ");
                zadanyPin = int.Parse(Console.ReadLine());
            }


            Console.WriteLine("Přihlášení úspěšné!");
            if (ucet.Admin)
            {
                AdminMenu(ucty, soubor);
                return;
            }
            Thread.Sleep(100);
        }



        // Pokud účet neexistuje
        else
        {
            Console.Clear();

            Console.WriteLine("Účet nenalezen.");
            Console.WriteLine("Registrace nového účtu.");
            Console.WriteLine();


            ucet = new BankovniUcet();


            ucet.Jmeno = name;
            ucet.Prijmeni = secondname;



            Console.Write("Vytvořte číselný PIN: ");
            int pin = int.Parse(Console.ReadLine());


            Console.Write("Zadejte PIN znovu: ");
            int pin2 = int.Parse(Console.ReadLine());


            while (pin != pin2)
            {
                Console.WriteLine("Piny se neshodují.");

                Console.Write("Vytvořte číselný PIN: ");
                pin = int.Parse(Console.ReadLine());


                Console.Write("Zadejte PIN znovu: ");
                pin2 = int.Parse(Console.ReadLine());
            }



            ucet.Pin = HashPin(pin.ToString());

            ucet.Zustatek = 10000;

            ucet.Historie = new List<string>();


            // Vygenerování karty

            Random generatorKarty = new Random();

            ucet.Karta = "";


            for (int i = 0; i < 16; i++)
            {
                ucet.Karta += generatorKarty.Next(0, 10);


                if ((i + 1) % 4 == 0 && i != 15)
                {
                    ucet.Karta += " ";
                }
            }

            Random generatorUctu = new Random();

            ucet.CisloUctu = "";

            for (int i = 0; i < 10; i++)
            {
                ucet.CisloUctu += generatorUctu.Next(0, 10);
            }



            ucty.Add(ucet);



            // Uložení účtů

            File.WriteAllText(
                soubor,
                JsonSerializer.Serialize(
                    ucty,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    })
            );


            Console.WriteLine();
            Console.WriteLine("Registrace dokončena!");

            Thread.Sleep(1500);
        }



        Console.Clear();


        int accountBalance = ucet.Zustatek;

        List<string> historie = ucet.Historie;


        bool konec = false;



        while (!konec)
        {
            Console.Clear();


            Console.WriteLine("===== Bank Bank =====");
            Console.WriteLine("1. Zobrazit stav účtu");
            Console.WriteLine("2. Vložit peníze");
            Console.WriteLine("3. Vybrat peníze");
            Console.WriteLine("4. Historie transakcí");
            Console.WriteLine("5. Debitní Karta");
            Console.WriteLine("6. Použít kartu v kasínu");
            Console.WriteLine("7. Číslo účtu");
            Console.WriteLine("8. Změna pinu");
            Console.WriteLine("9. Pujcka");
            Console.WriteLine("10. Splatit dluh");
            Console.WriteLine("11. Konec");

            Console.Write("Vyber možnost: ");


            int volba = int.Parse(Console.ReadLine());


            if (volba == 1)
            {
                Console.WriteLine($"Stav účtu: {accountBalance} Kč");
            }


            else if (volba == 2)
            {
                Console.Write("Kolik chcete vložit? ");

                int amount = int.Parse(Console.ReadLine());


                accountBalance += amount;

                ucet.Zustatek = accountBalance;

                historie.Add($"Vklad: +{amount} Kč");

                ucet.Historie = historie;

                Ulozit(ucty, soubor, ucet);


                Console.WriteLine("Peníze vloženy.");
            }


            else if (volba == 3)
            {
                Console.Write("Kolik chcete vybrat? ");

                int vyber = int.Parse(Console.ReadLine());


                if (vyber > accountBalance)
                {
                    Console.WriteLine("Nedostatek peněz.");
                }

                else
                {
                    accountBalance -= vyber;

                    ucet.Zustatek = accountBalance;

                    historie.Add($"Výběr: -{vyber} Kč");

                    ucet.Historie = historie;

                    Ulozit(ucty, soubor, ucet);

                    Console.WriteLine("Výběr úspěšný.");
                }
            }
            else if (volba == 4)
            {
                Console.WriteLine("=== Historie transakcí ===");


                if (historie.Count == 0)
                {
                    Console.WriteLine("Žádné transakce.");
                }

                else
                {
                    foreach (string transakce in historie)
                    {
                        Console.WriteLine(transakce);
                    }
                }
            }



            else if (volba == 5)
            {
                Console.WriteLine("Vaše karta:");

                Console.WriteLine(ucet.Karta);
            }

            else if (volba == 6)

            {
                Console.Clear();
                Console.WriteLine("==== Vítejte v Kasínu ====");
                Console.WriteLine("1. Gamblemat");
                Console.WriteLine("2. BlackJack");
                Console.WriteLine("3. Vratit se do menu");

                int volba1 = Convert.ToInt32(Console.ReadLine());

                if (volba1 == 1)
                {
                    Console.WriteLine("Spouštím Gamblemat...");
                    Gamblemat(ucet, ucty, soubor);
                    accountBalance = ucet.Zustatek;
                }
                else if (volba1 == 2)
                {
                    Console.WriteLine("Spouštím BlackJack...");
                    Blackjack(ucet, ucty, soubor);
                    accountBalance = ucet.Zustatek;
                }
                else if (volba1 == 3)
                {
                    continue;

                }
                else
                {
                    Console.WriteLine("Neplatná volba.");
                }
            }


            else if (volba == 7)
            {
                Console.WriteLine($"Vase cislo uctu je: {ucet.CisloUctu}" + "/" + "1234");
            }

            else if (volba == 8)
            {
                Console.Write("Zadej starý PIN: ");
                int staryPin = int.Parse(Console.ReadLine());


                if (HashPin(staryPin.ToString()) == ucet.Pin)
                {
                    Console.Write("Zadej nový PIN: ");
                    int novyPin = int.Parse(Console.ReadLine());


                    Console.Write("Zadej nový PIN znovu: ");
                    int novyPin2 = int.Parse(Console.ReadLine());


                    if (novyPin == novyPin2 && novyPin >= 1000)
                    {
                        ucet.Pin = HashPin(novyPin.ToString());

                        Ulozit(ucty, soubor, ucet);

                        Console.WriteLine("PIN byl úspěšně změněn.");
                    }
                    else
                    {
                        Console.WriteLine("PINy se neshodují nebo je PIN moc krátký.");
                    }
                }
                else
                {
                    Console.WriteLine("Špatný starý PIN.");
                }
            }
            else if (volba == 9)
            {
                Console.Write("Kolik si chceš půjčit? ");
                int pujcka = int.Parse(Console.ReadLine());

                if (pujcka > 0)
                {
                    ucet.Zustatek += pujcka;
                    accountBalance = ucet.Zustatek;

                    // 5% úrok
                    int urok = pujcka * 5 / 100;

                    // Přičtení dluhu i s úrokem
                    ucet.Dluh += pujcka + urok;

                    ucet.Historie.Add($"Půjčka: +{pujcka} Kč (úrok {urok} Kč)");

                    Ulozit(ucty, soubor, ucet);

                    Console.WriteLine($"Půjčil sis {pujcka} Kč.");
                    Console.WriteLine($"Úrok: {urok} Kč");
                    Console.WriteLine($"Celkem musíš splatit: {ucet.Dluh} Kč.");
                }
                else
                {
                    Console.WriteLine("Neplatná částka.");
                }
            }

            else if (volba == 10)
            {
                if (ucet.Dluh == 0)
                {
                    Console.WriteLine("Nemáš žádnou půjčku.");
                }
                else
                {
                    Console.WriteLine($"Tvůj dluh je: {ucet.Dluh} Kč");
                    Console.Write("Kolik chceš splatit? ");

                    int splatka = int.Parse(Console.ReadLine());

                    if (splatka <= 0)
                    {
                        Console.WriteLine("Neplatná částka.");
                    }
                    else if (splatka > ucet.Zustatek)
                    {
                        Console.WriteLine("Nemáš dostatek peněz na účtu.");
                    }
                    else if (splatka > ucet.Dluh)
                    {
                        Console.WriteLine("Nemůžeš splatit více, než dlužíš.");
                    }
                    else if (splatka == ucet.Dluh)
                    {
                        ucet.Zustatek -= splatka;
                        accountBalance = ucet.Zustatek;
                        ucet.Dluh = 0;
                        ucet.Historie.Add($"Splacení půjčky: -{splatka} Kč");
                        Ulozit(ucty, soubor, ucet);
                        Console.WriteLine("Půjčka byla splacena.");
                        Console.WriteLine($"Zůstatek: {ucet.Zustatek} Kč");
                    }
                    else
                    {
                        ucet.Zustatek -= splatka;
                        accountBalance = ucet.Zustatek;

                        ucet.Dluh -= splatka;

                        ucet.Historie.Add($"Splacení půjčky: -{splatka} Kč");

                        Ulozit(ucty, soubor, ucet);

                        Console.WriteLine("Půjčka byla částečně splacena.");
                        Console.WriteLine($"Zůstatek: {ucet.Zustatek} Kč");
                        Console.WriteLine($"Zbývající dluh: {ucet.Dluh} Kč");
                    }
                }
            }

            else if (volba == 11)
            {
                konec = true;
            }

            else
            {
                Console.WriteLine("Neplatná volba.");
            }



            if (!konec)
            {
                Console.WriteLine();

                Console.WriteLine("Stiskni Enter pro návrat do menu...");

                Console.ReadLine();
            }

        }

    }
    static void Gamblemat(BankovniUcet ucet, List<BankovniUcet> ucty, string soubor)
    {
        Console.Clear();


        Console.WriteLine(@"
██████╗  █████╗ ███████╗██╗███╗   ██╗ ██████╗
██╔════╝ ██╔══██╗██╔════╝██║████╗  ██║██╔═══██╗
██║  ███╗███████║███████╗██║██╔██╗ ██║██║   ██║
██║   ██║██╔══██║╚════██║██║██║╚██╗██║██║   ██║
╚██████╔╝██║  ██║███████║██║██║ ╚████║╚██████╔╝
 ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝╚═╝  ╚═══╝ ╚═════╝
");


        Random rnd = new Random();
        bool konecKasina = false;

        while (!konecKasina)
        {

            if (ucet.Zustatek == 0)
            {
                Console.Clear();
                Console.WriteLine("Nemáš peníze!");
                Thread.Sleep(1000);
                break;
            }

            Console.Clear();
            Console.WriteLine($"Máš {ucet.Zustatek} Kč.");

            Console.Write("Kolik chcete vsadit: ");

            int bet = int.Parse(Console.ReadLine());



            if (bet > ucet.Zustatek)
            {
                Console.WriteLine("Nemůžeš vsadit víc peněz než máš!");
                return;
            }



            if (bet <= 0)
            {
                Console.WriteLine("Sázka musí být větší než 0!");
                return;
            }



            int cislo1 = rnd.Next(1, 11);
            int cislo2 = rnd.Next(1, 11);
            int cislo3 = rnd.Next(1, 11);



            Console.WriteLine("      _______________________________");
            Console.WriteLine("     /===============================\\");
            Console.WriteLine("    /        $ GAMBLEMAT $            \\");
            Console.WriteLine("   |----------------------------------|");
            Console.WriteLine("   |                                  |");
            Console.WriteLine($"   |        ┌────┬────┬────┐          |");
            Console.WriteLine($"   |        │ {cislo1,2} │ {cislo2,2} │ {cislo3,2} │          |");
            Console.WriteLine("   |        └────┴────┴────┘          |");
            Console.WriteLine("   |                                  |");
            Console.WriteLine("   |__________________________________|");



            if (cislo1 == cislo2 && cislo2 == cislo3)
            {
                ucet.Zustatek += bet * 10;

                ucet.Historie.Add(
                    $"Casino výhra: +{bet * 10} Kč");


                Console.WriteLine("🎰 JACKPOT! Vyhrál jsi 10x sázku!");
            }


            else if (cislo1 == cislo2 || cislo1 == cislo3 || cislo2 == cislo3)
            {
                ucet.Zustatek += bet * 2;

                ucet.Historie.Add(
                    $"Casino výhra: +{bet * 2} Kč");


                Console.WriteLine("Dvě stejná čísla! Vyhrál jsi 2x sázku.");
            }


            else
            {
                ucet.Zustatek -= bet;

                ucet.Historie.Add($"Casino prohra: -{bet} Kč");


                Console.WriteLine("Prohrál jsi svou sázku.");
            }






            Ulozit(ucty, soubor, ucet);
            Console.WriteLine($"Aktuální zůstatek: {ucet.Zustatek} Kč");

            Console.WriteLine("ENTER = další hra | napiš konec = vypnout");
            string answer = Console.ReadLine();
            Console.Clear();

            if (answer.ToLower() == "konec")
            {
                konecKasina = true;
            }



        }
    }

    static void Blackjack(BankovniUcet ucet, List<BankovniUcet> ucty, string soubor)
    {
        Console.Clear();
        Console.WriteLine("===== BLACKJACK =====");

        Random rnd = new Random();
        bool konec = false;

        while (!konec)
        {

            if (ucet.Zustatek == 0)
            {
                Console.Clear();
                Console.WriteLine("Nemáš peníze!");
                Thread.Sleep(1000);
                break;
            }

            Console.WriteLine($"Máš {ucet.Zustatek} Kč.");
            Console.Write("Kolik chceš vsadit: ");

            int sazka = int.Parse(Console.ReadLine());

            if (sazka <= 0)
            {
                Console.WriteLine("Sázka musí být větší než 0.");
                continue;
            }

            if (sazka > ucet.Zustatek)
            {
                Console.WriteLine("Nemáš dostatek peněz.");
                continue;
            }

            string[] balicek =
            {
            "A","2","3","4","5","6","7","8","9","10","J","Q","K"
        };

            int score = 0;
            int scoreDealera = 0;

            bool hraSkoncila = false;

            while (!hraSkoncila)
            {
                string karta = balicek[rnd.Next(balicek.Length)];

                int hodnota;

                if (karta == "A")
                    hodnota = 11;
                else if (karta == "J" || karta == "Q" || karta == "K")
                    hodnota = 10;
                else
                    hodnota = Convert.ToInt32(karta);

                score += hodnota;

                Console.Clear();
                Console.WriteLine("===== BLACKJACK =====");
                Console.WriteLine($"Padla ti karta: {karta}");
                Console.WriteLine($"Tvé skóre: {score}");

                if (score > 21)
                {
                    Console.WriteLine("Přesáhl jsi 21.");
                    ucet.Zustatek -= sazka;
                    ucet.Historie.Add($"BlackJack prohra: -{sazka} Kč");
                    Ulozit(ucty, soubor, ucet);

                    hraSkoncila = true;
                    break;
                }

                if (score == 21)
                {
                    Console.WriteLine("BLACKJACK!");
                    break;
                }

                Console.WriteLine();
                Console.WriteLine("1 - Další karta");
                Console.WriteLine("2 - Zůstat");

                string volba = Console.ReadLine();

                if (volba == "1")
                {
                    continue;
                }
                else if (volba == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Neplatná volba.");
                    Thread.Sleep(1000);
                }
            }

            if (score > 21)
            {
                Console.WriteLine("ENTER...");
                Console.ReadLine();
                continue;
            }
            // Dealer bere karty do 17
            while (scoreDealera < 17)
            {
                string kartaDealera = balicek[rnd.Next(balicek.Length)];

                int hodnotaDealera;

                if (kartaDealera == "A")
                    hodnotaDealera = 11;
                else if (kartaDealera == "J" || kartaDealera == "Q" || kartaDealera == "K")
                    hodnotaDealera = 10;
                else
                    hodnotaDealera = Convert.ToInt32(kartaDealera);

                scoreDealera += hodnotaDealera;

                Console.WriteLine($"Dealer líznul: {kartaDealera}");
                Thread.Sleep(700);
            }

            Console.WriteLine();
            Console.WriteLine($"Tvé skóre: {score}");
            Console.WriteLine($"Skóre dealera: {scoreDealera}");
            Console.WriteLine();

            if (scoreDealera > 21)
            {
                Console.WriteLine("Dealer přesáhl 21.");
                Console.WriteLine($"Vyhrál jsi {sazka} Kč!");

                ucet.Zustatek += sazka;
                ucet.Historie.Add($"BlackJack výhra: +{sazka} Kč");
            }
            else if (score > scoreDealera)
            {
                Console.WriteLine($"Vyhrál jsi {sazka} Kč!");

                ucet.Zustatek += sazka;
                ucet.Historie.Add($"BlackJack výhra: +{sazka} Kč");
            }
            else if (score == scoreDealera)
            {
                Console.WriteLine("Remíza.");
                ucet.Historie.Add("BlackJack remíza");
            }
            else
            {
                Console.WriteLine($"Prohrál jsi {sazka} Kč.");

                ucet.Zustatek -= sazka;
                ucet.Historie.Add($"BlackJack prohra: -{sazka} Kč");
            }

            Ulozit(ucty, soubor, ucet);

            Console.WriteLine();
            Console.WriteLine($"Aktuální zůstatek: {ucet.Zustatek} Kč");
            Console.WriteLine();
            Console.WriteLine("ENTER = další hra | napiš konec = zpět");

            string odpoved = Console.ReadLine();

            if (odpoved.ToLower() == "konec")
            {
                konec = true;
            }

            Console.Clear();
        }

    }

    static void AdminMenu(List<BankovniUcet> ucty, string soubor)
    {
        bool konec = false;


        while (!konec)
        {
            Console.Clear();

            Console.WriteLine("===== ADMIN PANEL =====");
            Console.WriteLine("1. Zobrazit všechny účty");
            Console.WriteLine("2. Změnit zůstatek");
            Console.WriteLine("3. Smazat účet");
            Console.WriteLine("4. Konec");


            Console.Write("Volba: ");

            int volba = int.Parse(Console.ReadLine());


            if (volba == 1)
            {
                foreach (var ucet in ucty)
                {
                    if (!ucet.Admin)
                    {
                        Console.WriteLine(
                        $"{ucet.Jmeno} {ucet.Prijmeni} | {ucet.Zustatek} Kč");
                    }
                }
            }



            else if (volba == 2)
            {
                Console.Write("Jméno účtu: ");
                string jmeno = Console.ReadLine();


                BankovniUcet nalezen = ucty.FirstOrDefault(
                    x => x.Jmeno == jmeno
                );


                if (nalezen != null)
                {
                    Console.Write("Nový zůstatek: ");

                    int penize = int.Parse(Console.ReadLine());


                    nalezen.Zustatek = penize;


                    File.WriteAllText(
                        soubor,
                        JsonSerializer.Serialize(

                            ucty,
                            new JsonSerializerOptions
                            {
                                WriteIndented = true
                            })
                    );


                    Console.WriteLine("Změněno.");
                }
                else
                {
                    Console.WriteLine("Účet nenalezen.");
                }
            }



            else if (volba == 3)
            {
                Console.Write("Jméno účtu ke smazání: ");

                string jmeno = Console.ReadLine();


                BankovniUcet nalezen = ucty.FirstOrDefault(
                    x => x.Jmeno == jmeno
                );


                if (nalezen != null && !nalezen.Admin)
                {
                    ucty.Remove(nalezen);


                    File.WriteAllText(
                        soubor,
                        JsonSerializer.Serialize(
                            ucty,
                            new JsonSerializerOptions
                            {
                                WriteIndented = true
                            })
                    );


                    Console.WriteLine("Účet odstraněn.");
                }
                else
                {
                    Console.WriteLine("Účet nenalezen");
                }
            }



            else if (volba == 4)
            {
                konec = true;
            }


            Console.WriteLine("Enter...");
            Console.ReadLine();
        }
    }



    // Funkce pro ukládání účtů

    static void Ulozit(List<BankovniUcet> ucty, string soubor, BankovniUcet ucet)
    {

        static void Ulozit(List<BankovniUcet> ucty, string soubor, BankovniUcet ucet)
        {
            File.WriteAllText(
                soubor,
                JsonSerializer.Serialize(
                    ucty,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    })
            );
        }

        File.WriteAllText(
            soubor,
            JsonSerializer.Serialize(
                ucty,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                })
        );

    }


    static string HashPin(string pin)
    {

        using (SHA256 sha = SHA256.Create())

        {

            byte[] bytes = Encoding.UTF8.GetBytes(pin);

            byte[] hash = sha.ComputeHash(bytes);

            StringBuilder vysledek = new StringBuilder();

            foreach (byte b in hash)
            {
                vysledek.Append(b.ToString("x2"));
            }

            return vysledek.ToString();
        }

    }

}


// Třída účtu

class BankovniUcet
{
    public int Dluh { get; set; }

    public string CisloUctu { get; set; }

    public bool Admin { get; set; }

    public string Jmeno { get; set; }

    public string Prijmeni { get; set; }

    public string Pin { get; set; }

    public string Karta { get; set; }

    public int Zustatek { get; set; }

    public List<string> Historie { get; set; }

}
