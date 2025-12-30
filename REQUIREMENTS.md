1) Eesmärk
Luua MVP WPF‑töölauarakendus, mis rakendab DDD, SOLID, MVVM, EF Core (migratsioonid + seed), pärilust ja kvaliteetse koodi tavasid (logimine, veahaldus, dokumentatsioon).

2) Vabadus + piirangud
Te valite ise domeeni ja äriprotsessi.

Peab sisaldama: vähemalt 
- [x] 1 pärilushierarhia (nt Base → Derived1/Derived2), **: Assignment → Homework/Exam Assignment** 
- [ ] vähemalt 2 Aggregate Root, 
- [x] 1 töövoog/staatuseloogika (nt Draft → Done) **: Course statuses - Available → Enrolled → Completed**, 
- [x] eksport (CSV). **: export of Assignments for Course for Teacher**

Keelatud: ainult "CRUD tabelitele" ilma domeenireegliteta.

3) Tehniline stack
- [x] .NET, WPF/AVALONIA  UI  (MVVM) 

- [x] EF Core 8 – SQLite või **SQL Server LocalDB**

4) Arhitektuur (DDD kihid)
- [x] Presentation (WPF/AVALONIA UI)
- [x] Application:
  - [x] (UseCase’id, **: methods of IStudentService?**
  - [x] DTOd, **: StudentDTO, CourseDTO etc.**
  - [x] orkestreerimine)

- [ ] Domain (
  - [x] Entities, **: Student, Teacher, Course, Enrollment etc.**
  - [x] ValueObjects, 
  - [x] Aggregates, **: Student, Course** 
  - [ ] Domain Services,
  - [x] Events) **: StudentEnrolledEvent**

- [ ] Infrastructure (
  - [x] EF Core, 
  - [x] Repositories, **: UserRepository, StudentRepository, CourseRepository**
  - [x] Migrations, **: on the moment of writing it - initial, one that add startDate and endDate columns for the course**
  - [ ] Failid/PDF)


5) Andmemudel (nõuded)
- [x] Pärilus: TPH (discriminator) või TPT – valik põhjendada. **: Assignment → Homework/Exam Assignment - TPH; BaseEntity → Assignment/Course/Grade/Group/Student/Teacher/User - TPT**

- [x] Unikaalsus: vähemalt 1 unikaalne äriväärtus (indeks), nt Code, VIN, Email. **: Email is unique for User, Code is unique for Course**

- [x] Seosed: vähemalt üks 1‑N ja üks N‑N (võib olla läbi vahetabeli). **: Course can have many assignments (1-N), Student can be enrolled to many Courses and many Students can bi enrolled to one Course (N-N), Enrollment is the table that connects them**

- [x] Seed data: minimaalselt 2–3 tüüpnäidet kõigi põhientiteetide jaoks. **: User, Course, Assignment**



6) Funktsionaalsed must‑have
- [ ] CRUD vähemalt kolmel põhientiteedil (sh derived-tüübid vormis). **: User - CRUD; Course - R; Assignment - CRUD**

- [x] Otsing/filtrid (tekst + select + kuupäevavahemik). **: search/filters for courses under student account - text (course name/code), select (course status), course start-end date range**

- [x] Töövoog/staatused ühe Aggregate’i sees (reeglid Domain/Application kihis). **: Course statuses - Available → Enrolled → Completed**

- [x] Eksport:  CSV ühe põhivaatest (nt detailraport/tellimus).  **: export of Assignments for Course for Teacher**

- [x] Valideerimine: UI + domaini tasemel (FluentValidation/DataAnnotations).

- [ ] Veahaldus: 
  - [x] kasutajale viisakad sõnumid; 
  - [ ] logisse detailid.




7) Mittefunktsionaalsed nõuded
- [x] MVVM: ViewModelites pole andmebaasi loogikat; DI kasutusel.

- [ ] SOLID: SRP, väiksed interface’id, Dependency Inversion igal tasemel.

- [x] Jõudlus: loetelud AsNoTracking(); vajadusel paging .

- [ ] Dokumentatsioon: README + arhitektuuri ja andmemudeli skeem

8) UI vaated (miinimum)
- [x] Dashboard –  Peaaken

- [ ] Master‑Detail vaade #1 (Aggregate A) – grid + detail + Add/Edit/Delete.

- [x] Master‑Detail vaade #2 (Aggregate B) – grid + detail + filtrid. **: Courses view under Student account**

- [x] Workflow vaade (staatuse muutmine reeglitega). **: Course Status for Student - Available → Enrolled → Completed**

- [x] Export nupp (CSV) detail‑ või aruandevaatest.  **: export of Assignments for Course for Teacher**



9) Andmebaas ja migratsioonid
- [ ] Code First, migratsioonid repository’s; dotnet ef database update töötab puhtal masinal.

- [x] Seed OnModelCreating või käivitamisel (fikseeritud Id-d). **: Student, Courses creation etc.*

10) Projekti struktuur (soovitus)
Solution/
- [x] App.UI (
  - [x] WPF, 

  - [x] Views, 

  - [x] ViewModels, 

  - [x] DI)

- [x] App.Application (Services)

- [x] App.Domain (
  - [x] Entities, 

  - [x] ValueObjects, 

  - [x] Aggregates)

- [ ] App.Infrastructure (
  - [x] EF, 
  - [x] Migrations, 
  - [x] Repositories, 
  - [ ] Logging)