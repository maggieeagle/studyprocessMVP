using Application.DTO;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static Domain.Entities.Course;

namespace UI.ViewModels
{
    public partial class StudentCoursesViewModel : ObservableObject
    {
        private readonly IStudentCourseService _studentCourseService;
        private readonly int _studentId; // Current logged-in student

        public Array CourseStatusValues => Enum.GetValues(typeof(Course.CourseStatus));

        [ObservableProperty]
        private ObservableCollection<StudentCourseDTO> _courses = new();

        [ObservableProperty]
        private string? _searchText;

        [ObservableProperty]
        private string? _courseCode;

        [ObservableProperty]
        private DateTime? _startDate;

        [ObservableProperty]
        private DateTime? _endDate;

        [ObservableProperty]
        private Course.CourseStatus? _courseStatus = Course.CourseStatus.Available;


        public StudentCoursesViewModel(IStudentCourseService studentCourseService, int studentId)
        {
            _studentCourseService = studentCourseService;
            _studentId = studentId;

            LoadCoursesCommand = new AsyncRelayCommand(LoadCoursesAsync);
            EnrollCommand = new AsyncRelayCommand<StudentCourseDTO>(EnrollAsync);

            _ = LoadCoursesAsync();
        }

        public IAsyncRelayCommand LoadCoursesCommand { get; }
        public IAsyncRelayCommand<StudentCourseDTO> EnrollCommand { get; }

        private async Task LoadCoursesAsync()
        {
            var list = await _studentCourseService.GetAllCoursesWithEnrollmentStatusAsync(_studentId, _searchText, _courseCode, _courseStatus, _startDate, _endDate);
            Courses = new ObservableCollection<StudentCourseDTO>(list);
        }

        private async Task EnrollAsync(StudentCourseDTO courseDto)
        {
            if (courseDto.IsEnrolled) return; // Already enrolled

            await _studentCourseService.Enroll(_studentId, courseDto.CourseId);
            courseDto.IsEnrolled = true;

            await LoadCoursesAsync();
        }

    }
}
