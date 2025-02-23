using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using EasyCmd.Model;
using EasyGui.ViewModels;
using EasyGui.Views;

namespace EasyGui
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : System.Windows.Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			ProcessWatcher processWatcher = new ProcessWatcher();
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
		}
	}

}
