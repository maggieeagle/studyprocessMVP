using Application.DTO;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using UI.Views;

namespace UI.ViewModels
{
    public partial class ViewSubmissionsViewModel : ObservableObject
    {
        private readonly ICourseRepository _repository;
        private readonly int _assignmentId;

        public ObservableCollection<SubmissionDTO> Submissions { get; } = new();

        public ViewSubmissionsViewModel(
            int assignmentId,
            ICourseRepository repository)
        {
            _assignmentId = assignmentId;
            _repository = repository;

            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var list = await _repository.GetSubmissionsForAssignmentAsync(_assignmentId);

            Submissions.Clear();
            foreach (var s in list)
                Submissions.Add(s);
        }

        [RelayCommand]
        private async Task SaveGradesAsync(Window window)
        {
            try
            {
                await _repository.SaveGradesAsync(Submissions.ToList());

                CustomMessageBox.ShowSuccess("Grades saved successfully!");
                window?.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Save Error: {ex}");
                CustomMessageBox.ShowError("Failed to save grades. Check the debug console.");
            }
        }
    }
}
