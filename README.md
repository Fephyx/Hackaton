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

- Přihlášení pomocí hesla **kdikoliv**
- **Změna** pinu 
<!--=========================== dodělat ==============================-->

## Bankovní účet
- Zobrazení zůstatku
- Číslo bankovního účtu
- Číslo karty 
- CVC
- datum expirace

## Finanční operace
- Vklad peněz
- Výběr peněz
- Půjčky
- ====== doplit ostatni ========

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
