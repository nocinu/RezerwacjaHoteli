# Dokumentacja Projektu - RezerwacjaHoteli

# 1. Opis Projektu
Aplikacja internetowa typu MVC (Model-View-Controller) służąca do rezerwacji pokoi w hotelach. Pozwala użytkownikom przeglądać dostępne hotele, dokonywać rezerwacji oraz zarządzać swoimi rezerwacjami. Administratorzy mają możliwość zarządzania bazą hoteli i pokoi.

# 2. Wymagania Systemowe
- Środowisko: .NET 10.0 SDK 
- Baza danych: Microsoft SQL Server 
- IDE: Visual Studio 2022 

# 3. Instalacja i Konfiguracja
1. Pobranie: Skopiuj kod źródłowy na swój komputer.
2. Otwarcie: Otwórz plik "RezerwacjaHoteli.csproj" w Visual Studio.
3. Baza Danych:
   - Aplikacja korzysta z Entity Framework Core i automatycznie tworzy bazę danych przy pierwszym uruchomieniu.
   - Łańcuch połączenia znajduje się w pliku "appsettings.json" i domyślnie wskazuje na lokalną instancję
4. Uruchomienie:
   - Wybierz "Uruchom" w Visual Studio.
   - Aplikacja automatycznie załaduje przykładowe dane przy pierwszym starcie.

# 4. Dane Testowe
Aplikacja przy pierwszym uruchomieniu tworzy dwóch użytkowników z różnymi uprawnieniami.

# Administrator
- Email: admin@hotel.com
- Hasło: Admin123!
- Uprawnienia: Pełny dostęp do panelu zarządzania (dodawanie/edycja/usuwanie hoteli i pokoi).

# Gość 
- Email: guest@example.com
- Hasło: Guest123!
- Uprawnienia: Przeglądanie hoteli, rezerwacja pokoi, podgląd własnych rezerwacji.

# 5. Instrukcja Obsługi

# Gość
1. Rejestracja: Kliknij "Zarejestruj się" w prawym górnym rogu. Podaj wymagane dane.
2. Logowanie: Zaloguj się utworzonym kontem lub kontem testowym.
3. Przeglądanie: Na stronie głównej wybierz hotel klikając "Zobacz szczegóły".
4. Rezerwacja: Wybierz pokój ze statusem "Dostępny". Kliknij "Rezerwuj", wybierz daty i liczbę gości. Po zatwierdzeniu rezerwacja pojawi się w zakładce "Moje rezerwacje".

# Administrator
1. Logowanie: Zaloguj się danymi Administratora.
2. Zarządzanie: W menu górnym pojawią się dodatkowe przyciski: "Dodaj nowy hotel/pokój". Po uzupełnieniu danych pojawi się nowa pozycja.
3. Edycja: W widoku hoteli, dostępna jest edycja i usuwanie hoteli.


