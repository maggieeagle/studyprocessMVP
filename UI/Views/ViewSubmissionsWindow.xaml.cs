using Application.Interfaces;
using System.Windows;
using UI.ViewModels;

namespace UI.Views
{
    public partial class ViewSubmissionsWindow : Window
    {
        public ViewSubmissionsWindow(int assignmentId, ICourseRepository repository)
        {
            InitializeComponent();
            DataContext = new ViewSubmissionsViewModel(assignmentId, repository);
        }
    }
}