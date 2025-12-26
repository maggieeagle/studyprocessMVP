using Application.DTO;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using UI.Views;
using Microsoft.Win32;
using System.IO;

namespace UI.ViewModels
{
    public partial class CourseViewModel : ObservableObject
    {
        private readonly ICourseRepository _repository;
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly ICourseAssignmentsExportService _courseAssignmentsExportService;
        private readonly int _courseId;

        [ObservableProperty]
        private string courseName;

        [ObservableProperty]
        private string code;

        [ObservableProperty]
        private string teacherName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsStudent))]
        private bool isTeacher;

        public bool IsStudent => !IsTeacher;

        public ObservableCollection<AssignmentDTO> Assignments { get; } = new();

        public IAsyncRelayCommand ExportAssignmentsCsvCommand { get; }

        public CourseViewModel(
            int courseId,
            ICourseRepository repository,
            IAuthService authService,
            INavigationService navigationService,
            ICourseAssignmentsExportService courseAssignmentsExportService)
        {
            _courseId = courseId;
            _repository = repository;
            _authService = authService;
            _navigationService = navigationService;
            _courseAssignmentsExportService = courseAssignmentsExportService;

            ExportAssignmentsCsvCommand = new AsyncRelayCommand(ExportAssignmentsCsvAsync);

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

        [RelayCommand]
        private async Task OpenViewSubmissionsWindow(AssignmentDTO assignment)
        {
            if (assignment == null) return;

            var window = new ViewSubmissionsWindow(assignment.Id, _repository);
            window.ShowDialog();
        }

        [RelayCommand]
        private void GoBack()
        {
            _navigationService.NavigateToStudentCourses();
        }

        private async Task ExportAssignmentsCsvAsync()
        {
            try
            {
                // Get DTOs from Application service
                var exportData = await _courseAssignmentsExportService.GetAssignmentsForCourseAsync(_courseId);

                // Convert to CSV string
                var csv = _courseAssignmentsExportService.ToCsv(exportData);

                // Ask teacher where to save
                var dlg = new SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv",
                    FileName = $"{CourseName}_Assignments.csv"
                };

                if (dlg.ShowDialog() == true)
                {
                    await File.WriteAllTextAsync(dlg.FileName, csv);
                    MessageBox.Show("CSV exported successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export CSV: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task DeleteAssignment(AssignmentDTO assignment)
        {
            if (assignment == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete '{assignment.Name}'? This cannot be undone.",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _repository.DeleteAssignmentAsync(assignment.Id);
                    Assignments.Remove(assignment);
                    MessageBox.Show("Assignment deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting assignment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        public async Task UpdateAssignment(AssignmentDTO assignment)
        {
            if (assignment == null || IsStudent) return;

            try
            {
                await _repository.UpdateAssignmentAsync(assignment.Id, assignment);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save changes: {ex.Message}", "Sync Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);

                LoadCourseDetailsAsync();
            }
        }
    }
}