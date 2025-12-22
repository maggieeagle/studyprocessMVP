using Application.DTO;
using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace UI.ViewModels
{
    public partial class CourseViewModel : ObservableObject
    {
        private readonly ICourseRepository _repository;
        private readonly int _courseId;
        private readonly int _studentId = 1;

        [ObservableProperty]
        private string courseName;

        [ObservableProperty]
        private string code;

        [ObservableProperty]
        private string teacherName;

        public ObservableCollection<AssignmentDTO> Assignments { get; } = new();

        public CourseViewModel(int courseId, ICourseRepository repository)
        {
            _courseId = courseId;
            _repository = repository;

            LoadCourseDetailsAsync();
        }

        private async void LoadCourseDetailsAsync()
        {
            var details = await _repository.GetCourseDetailsAsync(_courseId, _studentId);

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
        private void OpenSubmitModal(AssignmentDTO assignment)
        {
            // Logic to open modal window goes here
        }
    }
}