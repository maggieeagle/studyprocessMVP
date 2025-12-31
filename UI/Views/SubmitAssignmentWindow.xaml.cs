using System.Windows;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for SubmitAssignmentWindow.xaml
    /// </summary>
    public partial class SubmitAssignmentWindow : Window
    {
        public string Answer { get; private set; } = string.Empty;
        public SubmitAssignmentWindow()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Answer = AnswerBox.Text;
            DialogResult = true; // Closes window and returns success
        }
    }
}
