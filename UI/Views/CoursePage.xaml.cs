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
            DataContextChanged += CoursePage_DataContextChanged;
        }

        private void CoursePage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is CourseViewModel oldVm)
            {
                oldVm.PropertyChanged -= Vm_PropertyChanged;
            }

            if (e.NewValue is CourseViewModel vm)
            {
                vm.PropertyChanged += Vm_PropertyChanged;
                UpdateColumnVisibility(vm);
            }
        }

        private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is CourseViewModel vm && e.PropertyName == nameof(vm.IsTeacher))
            {
                Dispatcher.Invoke(() => UpdateColumnVisibility(vm));
            }
        }

        private void UpdateColumnVisibility(CourseViewModel vm)
        {
            var studentVisibility = vm.IsTeacher ? Visibility.Collapsed : Visibility.Visible;
            GradeColumn.Visibility = studentVisibility;
        }
    }
}
