1) Eesmärk
Luua MVP WPF‑töölauarakendus, mis rakendab DDD, SOLID, MVVM, EF Core (migratsioonid + seed), pärilust ja kvaliteetse koodi tavasid (logimine, veahaldus, dokumentatsioon).

2) Vabadus + piirangud
Te valite ise domeeni ja äriprotsessi.

Peab sisaldama: vähemalt 
- [x] 1 pärilushierarhia (nt Base → Derived1/Derived2), 
- [ ] vähemalt 2 Aggregate Root, 
- [ ] 1 töövoog/staatuseloogika (nt Draft → Done), 
eksport (CSV).

Keelatud: ainult "CRUD tabelitele" ilma domeenireegliteta.

3) Tehniline stack
.NET, WPF/AVALONIA  UI  (MVVM) 

EF Core 8 – SQLite või SQL Server LocalDB

4) Arhitektuur (DDD kihid)
- [ ] Presentation (WPF/AVALONIA UI)
- [ ] Application (UseCase’id, DTOd, orkestreerimine)
- [ ] Domain (Entities, ValueObjects, Aggregates, Domain Services, Events)
- [ ] Infrastructure (EF Core, Repositories, Migrations, Failid/PDF)
 
5) Andmemudel (nõuded)
- [ ] Pärilus: TPH (discriminator) või TPT – valik põhjendada.

- [x] Unikaalsus: vähemalt 1 unikaalne äriväärtus (indeks), nt Code, VIN, Email.

- [x] Seosed: vähemalt üks 1‑N ja üks N‑N (võib olla läbi vahetabeli).

- [ ] Seed data: minimaalselt 2–3 tüüpnäidet kõigi põhientiteetide jaoks.



6) Funktsionaalsed must‑have
- [ ] CRUD vähemalt kolmel põhientiteedil (sh derived-tüübid vormis).

- [ ] Otsing/filtrid (tekst + select + kuupäevavahemik).

- [ ] Töövoog/staatused ühe Aggregate’i sees (reeglid Domain/Application kihis).

- [ ] Eksport:  CSV ühe põhivaatest (nt detailraport/tellimus).

- [ ] Valideerimine: UI + domaini tasemel (FluentValidation/DataAnnotations).

- [ ] Veahaldus: kasutajale viisakad sõnumid; logisse detailid.



7) Mittefunktsionaalsed nõuded
- [x] MVVM: ViewModelites pole andmebaasi loogikat; DI kasutusel.

- [ ] SOLID: SRP, väiksed interface’id, Dependency Inversion igal tasemel.

- [ ] Jõudlus: loetelud AsNoTracking(); vajadusel paging .

- [ ] Dokumentatsioon: README + arhitektuuri ja andmemudeli skeem

8) UI vaated (miinimum)
- [ ] Dashboard –  Peaaken

- [ ] Master‑Detail vaade #1 (Aggregate A) – grid + detail + Add/Edit/Delete.

- [ ] Master‑Detail vaade #2 (Aggregate B) – grid + detail + filtrid.

- [ ] Workflow vaade (staatuse muutmine reeglitega).

- [ ] Export nupp (CSV) detail‑ või aruandevaatest.



9) Andmebaas ja migratsioonid
- [ ] Code First, migratsioonid repository’s; dotnet ef database update töötab puhtal masinal.

- [x] Seed OnModelCreating või käivitamisel (fikseeritud Id-d).

10) Projekti struktuur (soovitus)
Solution/
- [x] App.UI (WPF, Views, ViewModels, DI)

- [x] App.Application (Services)

- [ ] App.Domain (Entities, ValueObjects, Aggregates)

- [ ] App.Infrastructure (EF, Migrations, Repositories, Logging)