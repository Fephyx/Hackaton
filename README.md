# MyBank - Konzolová bankovní aplikace v jazyce C#

## Popis projektu

**MyBank** je konzolová bankovní aplikace vytvořená v jazyce **C#**
Uživatel může **spravovat svůj účet, provádět finanční operace, sledovat historii transakcí** a využívat **spořicí služby**.


## Hlavní funkce

### Správa uživatelů 
- registrace nového klienta

```csharp
Console.WriteLine("Registrujte se zde:");
Console.WriteLine("Zmačkněte Enter pro pokračování...");
Console.ReadKey();
Console.Clear();
Console.WriteLine("Zadejte své Jméno: ");
string name = Console.ReadLine();
Console.WriteLine("Zadejte své příjmení:");
string secondname = Console.ReadLine();

Console.WriteLine("Vytvorte si pin (Min 4 cislice):");
int pin = int.Parse(Console.ReadLine());
Console.Clear();
Console.WriteLine("Znovu zadejte pin:");
int pin2 = int.Parse(Console.ReadLine());
while (pin != pin2)
{
    Console.Clear();
    Console.WriteLine("Piny se neshodují. Zkuste to znovu.");
    Console.ReadKey();
    Console.Clear();
    Console.WriteLine("Vytvorte si pin:");
    pin = int.Parse(Console.ReadLine());
    Console.Clear();
    Console.WriteLine("Znovu zadejte pin:");
    pin2 = int.Parse(Console.ReadLine());
        }

Console.Clear();
Console.WriteLine("Registrace dokončena!");
ucet = new BankovniUcet();
```
- Ukládání uživatelů do .json

```csharp
======================== doplnit kod =============================
```

- Přihlášení pomocí PINU **kdikoliv**
- Po 3 špatných pokusech se účet zablokuje
- **Změna** pinu 
```csharp
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

```

## Bankovní účet
- Zobrazení zůstatku
- Číslo bankovního účtu
- Číslo karty 

## Finanční operace
- Vklad peněz
- Výběr peněz
- Půjčky
- Splatit dluh

```csharp
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

```

## Historie transakcí
- Každá operace je uložena a zaznamenána

## Kasíno
- svoji kartu můžete použít i v našem kasínu
- Hry:
     - BlackJack
     - Slot machine

### Slot machine
- **3** stejná čísla = x10 sázka
- **2** stejná čísla = x2 sázka
- ostatní = **prohra**


## Instalace

## 1. Klonování projektu

```bash
git clone https://github.com/Fephyx/Hackaton
```

## 2. Otevření projektu

Otevřete: 

```
MyBank.sln
```

a aplikaci:

- Visual studio
- Rider
- Visual Studio code

<!--img aplikaci-->

## 3. Spuštění

```bash
dotnet run
```

---

### Autoři: 

- Corven (Matyáš Vindiš)
- Zyxxyc {Patrik Polák}
- Fephyx [Antonín Sejpka]

Rok: 
2026
