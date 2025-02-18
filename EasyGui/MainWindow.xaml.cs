using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
    }
}