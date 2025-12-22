using Application.DTO;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace UI.ViewModels
{
    public partial class StudentCoursesViewModel : ObservableObject
    {
        private readonly IStudentCourseService _studentCourseService;
        private readonly INavigationService _navigationService;
        private readonly int _studentId;

        [ObservableProperty]
        private ObservableCollection<StudentCourseDTO> _courses = new();

        public StudentCoursesViewModel(INavigationService navigationService, IStudentCourseService studentCourseService, int studentId)
        {
            _navigationService = navigationService;
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
            var list = await _studentCourseService.GetAllCoursesWithEnrollmentStatusAsync(_studentId);
            Courses = new ObservableCollection<StudentCourseDTO>(list);
        }

        private async Task EnrollAsync(StudentCourseDTO courseDto)
        {
            if (courseDto == null || courseDto.IsEnrolled) return;

            await _studentCourseService.Enroll(_studentId, courseDto.CourseId);

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
