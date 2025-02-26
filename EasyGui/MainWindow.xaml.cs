using System.Windows;
using System.Windows.Navigation;
using EasyGui.ViewModels;

namespace EasyGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainWindowViewModel.Instance;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            
        }

        private void Frame_Navigated_1(object sender, NavigationEventArgs e)
        {
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
