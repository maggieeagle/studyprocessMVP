using Application.DTO;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using UI.Views;

namespace UI.ViewModels
{
    public partial class CourseViewModel : ObservableObject
    {
        private readonly ICourseRepository _repository;
        private readonly IAuthService _authService;
        private readonly int _courseId;

        [ObservableProperty]
        private string courseName;

        [ObservableProperty]
        private string code;

        [ObservableProperty]
        private string teacherName;

        [ObservableProperty]
        private bool isTeacher;

        public ObservableCollection<AssignmentDTO> Assignments { get; } = new();

        public CourseViewModel(
            int courseId,
            ICourseRepository repository,
            IAuthService authService)
        {
            _courseId = courseId;
            _repository = repository;
            _authService = authService;

            LoadCourseDetailsAsync();
        }

        private async void LoadCourseDetailsAsync()
        {
            int currentUserId = _authService.GetCurrentUserId();

            IsTeacher = await _repository.IsTeacherAsync(currentUserId);

            var details = await _repository.GetCourseDetailsAsync(_courseId, currentUserId);

            CourseName = details.CourseName;
            Code = details.Code;
            TeacherName = details.TeacherName;

            Assignments.Clear();
            foreach (var asm in details.Assignments)
            {
                Assignments.Add(asm);
            }
        }

        [RelayCommand]
        private async Task OpenSubmitWindow(AssignmentDTO assignment)
        {
            int currentUserId = _authService.GetCurrentUserId();
            var dialog = new SubmitAssignmentWindow();

            if (dialog.ShowDialog() == true)
            {
                string studentAnswer = dialog.Answer;

                await _repository.SubmitAssignmentAsync(currentUserId, assignment.Id, studentAnswer);

                assignment.Status = "Submitted";
                MessageBox.Show("Assignment submitted successfully!");
            }
        }

        [RelayCommand]
        private async Task OpenAddAssignmentWindow()
        {
            var dialog = new AddAssignmentWindow();

            if (dialog.ShowDialog() == true)
            {
                var newAssignment = dialog.Result;

                try
                {
                    await _repository.AddAssignmentAsync(_courseId, newAssignment);

                    LoadCourseDetailsAsync();
                    MessageBox.Show("Assignment added successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding assignment: {ex.Message}");
                }
            }
        }
    }
}