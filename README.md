# 🎾 SportBookingSystem

System rezerwacji kortów tenisowych zbudowany w **ASP.NET Core 8 MVC**.

## 📋 Opis projektu

Platforma webowa umożliwiająca klientom samodzielną rezerwację kortów tenisowych online oraz administratorom zarządzanie obiektami i rezerwacjami – bez konieczności dzwonienia do recepcji.

## ✅ Funkcje aplikacji

### Dla klienta:
- Przeglądanie dostępnych kortów (nawierzchnia, cena, typ)
- Rejestracja i logowanie (ASP.NET Core Identity)
- Rezerwacja kortu z wyborem daty i godziny
- Dynamiczny kalkulator ceny (JavaScript)
- Podgląd zajętych terminów w czasie rzeczywistym (AJAX)
- Podgląd i anulowanie własnych rezerwacji

### Dla administratora:
- Panel admina zabezpieczony rolą `Admin`
- Pełny CRUD kortów (dodawanie, edycja, usuwanie)
- Podgląd i usuwanie wszystkich rezerwacji użytkowników
- Dashboard ze statystykami (liczba kortów, rezerwacji, przychód)

## 🛠️ Użyte technologie

| Warstwa        | Technologia                        |
|----------------|------------------------------------|
| Backend        | ASP.NET Core 8 MVC                 |
| ORM            | Entity Framework Core 8            |
| Baza danych    | MS SQL Server (LocalDB)            |
| Autentykacja   | ASP.NET Core Identity + Role       |
| Frontend       | Bootstrap 5.3 + Bootstrap Icons    |
| JavaScript     | Vanilla JS (bez frameworka)        |
| CSS            | Bootstrap + własne style (Flexbox) |
| IDE            | Visual Studio 2022                 |
| Repozytorium   | GitHub (Git)                       |

## 🚀 Uruchomienie lokalne – krok po kroku

### Wymagania wstępne
- **Visual Studio 2022** (wersja 17.x lub nowsza)
- **SDK .NET 8.0** – https://dotnet.microsoft.com/download/dotnet/8.0
- **SQL Server LocalDB** – instalowany razem z Visual Studio (workload: *ASP.NET and web development*)

### 1. Sklonuj repozytorium

```bash
git clone https://github.com/MsciejBednarczyk/SportBookingSystem.git
cd SportBookingSystem
```

### 2. Otwórz projekt w Visual Studio

Otwórz plik `SportBookingSystem.csproj` w Visual Studio 2022.

### 3. Sprawdź connection string

W pliku `appsettings.json` upewnij się, że connection string jest poprawny:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SportBookingDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 4. Zastosuj migracje bazy danych

W **Package Manager Console** (Narzędzia → Menedżer pakietów NuGet → Konsola menedżera pakietów):

```powershell
Add-Migration InitialCreate
Update-Database
```

Baza zostanie utworzona automatycznie wraz z przykładowymi kortami i kontem admina.

### 5. Uruchom aplikację

Naciśnij **F5** lub kliknij przycisk ▶️ w Visual Studio.

Aplikacja uruchomi się pod adresem `https://localhost:XXXX`.

### 6. Dane logowania admina

| Pole     | Wartość          |
|----------|------------------|
| Email    | `admin@sport.pl` |
| Hasło    | `Admin123!`      |

## 📁 Struktura projektu

```
SportBookingSystem/
├── Controllers/
│   ├── HomeController.cs          # Strona główna + SimplePage GET/POST (z zajęć)
│   ├── CourtsController.cs        # Publiczna lista kortów
│   ├── ReservationsController.cs  # Rezerwacje użytkownika
│   └── AdminController.cs         # Panel administracyjny (CRUD)
├── Models/
│   ├── Court.cs                   # Model kortu
│   ├── Reservation.cs             # Model rezerwacji
│   ├── ApplicationUser.cs         # Rozszerzony użytkownik Identity + ViewModels
│   └── ErrorViewModel.cs          # Model błędu
├── Data/
│   ├── ApplicationDbContext.cs    # EF Core DbContext + Seed danych
│   └── SeedData.cs                # Inicjalizacja ról i konta admina
├── Views/
│   ├── Home/                      # Strona główna, SimplePage, About
│   ├── Courts/                    # Lista i szczegóły kortów
│   ├── Reservations/              # Tworzenie i lista rezerwacji
│   ├── Admin/                     # Panel administracyjny
│   └── Shared/                    # Layout, partial views
└── wwwroot/
    ├── css/site.css               # Własne style + RWD (@media)
    └── js/site.js                 # Vanilla JavaScript
```

## 🔐 Role i autoryzacja

| Rola    | Uprawnienia                                      |
|---------|--------------------------------------------------|
| `User`  | Przeglądanie kortów, rezerwacja, własne konto    |
| `Admin` | Pełny dostęp do panelu `/Admin`, CRUD kortów     |

## 📝 Wzorce z zajęć uwzględnione w projekcie

- Metody **GET i POST** z walidacją modelu (`/Home/SimplePage`)
- **Parametryczna metoda GET** (`/Home/SimplePage?name=Jan&message=Test`)
- Atrybuty walidacyjne: `[Required]`, `[Display]`, `[Range]`, `[StringLength]`
- **Tag Helpers**: `asp-for`, `asp-action`, `asp-controller`, `asp-route-*`
- **ViewData** i **TempData** do przekazywania danych do widoku
- **ModelState.IsValid** do obsługi błędów formularza
- Wzorzec **MVC** (Model-View-Controller)
- **ASP.NET Core Identity** z rozszerzonym użytkownikiem (`ApplicationUser`)
- **Entity Framework Core** z relacjami i Seed Data
- **Autoryzacja oparta na rolach** (`[Authorize(Roles = "Admin")]`)