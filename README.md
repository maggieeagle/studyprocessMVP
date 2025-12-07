# MVP for study organization app

## Entities
- Course
- Student
- Enrollment
- Assingment:
    + homework assignment
    + exam assignment
- Grade
- Group

## Architecture

### UI
  Defines UI and NVVM
  
  Input validation on UI level

### Application
  Defines operations that needs to be performed.
  
  Application services (IStudentService, ICourseService, etc.)
  
  DTOs, interfaces for repositories, CRUD
  
### Domain
  Defines the core business logic and rules of the system.
  Here are defined entities.
  
### Infrastructure
  Implements all technical details.

  EF Core DbContext, entity configurations and migrations
  
  Repository implementations (e.g., EfStudentRepository)

  Dependency Injection setup

### Interaction of layers
<img width="500" alt="Screenshot_20251207_151554" src="https://github.com/user-attachments/assets/35666baf-321e-41d9-82c0-b6f971a1a481" />


