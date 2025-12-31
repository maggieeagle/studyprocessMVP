using Application.DTO;
using System.Windows;
using System.Windows.Controls;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddAssignmentWindow : Window
    {
        // Public property to retrieve data after window closes
        public AddAssignmentDTO? Result { get; private set; }

        public AddAssignmentWindow()
        {
            InitializeComponent();
            DueDatePicker.SelectedDate = DateTime.Now.AddDays(7);
        }

        private void TypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HomeworkPanel == null) return; // Prevention for init

            var isHomework = TypeCombo.SelectedIndex == 0;
            HomeworkPanel.Visibility = isHomework ? Visibility.Visible : Visibility.Collapsed;
            ExamPanel.Visibility = !isHomework ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Basic Validation
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                CustomMessageBox.ShowWarning(Resource1.TitleIsRequired);
                return;
            }

            Result = new AddAssignmentDTO
            {
                Title = TitleBox.Text,
                Description = DescBox.Text,
                Type = TypeCombo.SelectedIndex == 0 ? Resource1.Homework : Resource1.Exam,
                DueDate = DueDatePicker.SelectedDate ?? DateTime.Now,
                MaxPoints = int.TryParse(PointsBox.Text, out int p) ? p : 0,
                ExamDate = ExamDatePicker.SelectedDate
            };

            DialogResult = true;
        }
    }
}
