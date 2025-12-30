# MVP for study organization app

## Entities
- Course
- User (stores basic information for authentication common for both Student and Teacher)
- Student
- Teacher
- Enrollment
- Assignment:
    + homework assignment (inherited)
    + exam assignment (inherited)
- Grade
- Submission
- Group (currently is not connected to UI and is unused)

## Use Cases

Application in its current state is supposed to used by two user types - Student and Teacher. Unregistered user can create account specifying type of user (Student/Teacher), personal information and setting password. After that user can login into created account.

### Preseed user data

For testing purposes is possible to use user credentials that are preceed in database. They are:

+ `john.doe@ut.ee`, `123` for Student
+ `jane.doe@ut.ee`, `123` for Teacher

### Student

+ On the main page after login student can observe all courses. Courses can be filtered by different parameters (e.g. Name, Code, Status...). Note, that the 'Search' button must be pressed. Student can enroll themselves on course by pressing an according button.

+ If clicked on a course name, Student appears on the course page where can observe list of assignments for the course, assignment's properties, status and grade for the assignment. Student can submit their solution for assignment by clicking according button.

+ ...

### Teacher

+ As a Student, on the main page after login Teacher can observe list of courses. All teachers can observe all courses.

+ If clicked on a course name, Teacher appears on the course page where can observe list of assignments for the course. Teacher can add assignment, specifying title, description, type of the assignment, due date and points that the assignment can give at maximum.

+ Teacher can view all student submissions for the specific assignment and grade them.

+ In addition, teacher can export list of assignments for the course in the CSV format by pressing according button.

+ ...

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

