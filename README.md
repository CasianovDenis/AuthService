# LiceuAPI
A fost implementat JWT Token pentru autentificarea utilizatorului și limitarea accesului la API-uri pentru utilizatori ne autorizați.

Lucrul cu baza de date s-e realizează prin intermediul lui entity framework ,iar stocarea datelor are loc în baza de date PostgreSQL

Schema la baza de date:
<img src="https://i.imgur.com/sPR8FDi.png"/>

Pentru a rula aplicația:

- Trebuie de conectat baza de date ,pentru aceasta configurați fișierul appsettings.json , indicănd calea la baza de date 
- Executați migrațiile prin comanda Update-Database "only Visual studio" sau  dotnet ef database update "VS Code"
