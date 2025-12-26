using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for CoursePage.xaml
    /// </summary>
    public partial class CoursePage : UserControl
    {
        public CoursePage()
        {
            InitializeComponent();
            Loaded += CoursePage_Loaded;
        }

        private void CoursePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CourseViewModel vm)
            {
                vm.PropertyChanged += Vm_PropertyChanged;
                UpdateColumnVisibility(vm);
            }
        }

        private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is CourseViewModel vm &&
                (e.PropertyName == nameof(vm.IsTeacher)))
            {
                Dispatcher.Invoke(() => UpdateColumnVisibility(vm));
            }
        }

        private void UpdateColumnVisibility(CourseViewModel vm)
        {
            var teacherVisibility = vm.IsTeacher
                ? Visibility.Visible
                : Visibility.Collapsed;

            var studentVisibility = vm.IsTeacher
                ? Visibility.Collapsed
                : Visibility.Visible;

            UpdateColumn.Visibility = teacherVisibility;
            DeleteColumn.Visibility = teacherVisibility;
            GradeColumn.Visibility = studentVisibility;
        }
    }
}
