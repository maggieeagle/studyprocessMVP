using Application.DTO;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;

namespace UI.ViewModels
{
    public partial class StudentCoursesViewModel : ObservableObject
    {
        private readonly IStudentCourseService _studentCourseService;
        private readonly INavigationService _navigationService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private ObservableCollection<StudentCourseDTO> _courses = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsStudent))]
        private bool _isTeacher;

        public bool IsStudent => !IsTeacher;
        private string? _searchText;

        [ObservableProperty]
        private string? _courseCode;

        [ObservableProperty]
        private DateTime? _startDate;

        [ObservableProperty]
        private DateTime? _endDate;

        [ObservableProperty]
        private Course.CourseStatus? _courseStatus = null;

        public IReadOnlyList<object> CourseStatusValues =>
        new object[] { null, Course.CourseStatus.Available, Course.CourseStatus.Enrolled, Course.CourseStatus.Completed };

        public StudentCoursesViewModel(INavigationService navigationService, IStudentCourseService studentCourseService, IAuthService authService)
        {
            _navigationService = navigationService;
            _studentCourseService = studentCourseService;
            _authService = authService;

            _authService.StateChanged += OnAuthStateChanged;

            UpdateRole();

            LoadCoursesCommand = new AsyncRelayCommand(LoadCoursesAsync);
            EnrollCommand = new AsyncRelayCommand<StudentCourseDTO>(EnrollAsync);

            _ = LoadCoursesAsync();
        }

        private void UpdateRole()
        {
            var roles = _authService.GetCurrentUserRoles();
            IsTeacher = roles.Contains("Teacher");
        }

        private void OnAuthStateChanged()
        {
            UpdateRole();
        }

        public IAsyncRelayCommand LoadCoursesCommand { get; }
        public IAsyncRelayCommand<StudentCourseDTO> EnrollCommand { get; }

        private async Task LoadCoursesAsync()
        {
            int currentId = _authService.GetCurrentUserId();

            var list = await _studentCourseService.GetAllCoursesWithEnrollmentStatusAsync(currentId, _searchText, _courseCode, _courseStatus, _startDate, _endDate);
            Courses = new ObservableCollection<StudentCourseDTO>(list);
        }

        private async Task EnrollAsync(StudentCourseDTO courseDto)
        {
            if (courseDto == null || courseDto.IsEnrolled) return;

            await _studentCourseService.Enroll(
                _authService.GetCurrentUserId(),
                courseDto.CourseId);

            await LoadCoursesAsync();
        }

        [RelayCommand]
        private void NavigateToCourse(StudentCourseDTO course)
        {
            if (course != null)
            {
                _navigationService.NavigateToCourseDetails(course.CourseId);
            }
        }
    }
}
