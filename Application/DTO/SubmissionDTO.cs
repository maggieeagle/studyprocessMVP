using CommunityToolkit.Mvvm.ComponentModel;

namespace Application.DTO
{
    public partial class SubmissionDTO : ObservableObject
    {
        public int SubmissionId { get; set; }
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; } = string.Empty;
        public string StudentLastName { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }

        [ObservableProperty]
        private int? grade;
    }
}
