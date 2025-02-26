using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyGui.Views
{
	/// <summary>
	/// Logique d'interaction pour BackupsView.xaml
	/// </summary>
	public partial class BackupJobListView : Page
	{
		public BackupJobListView()
		{
			InitializeComponent();
			DataContext = new ViewModels.BackupJobListViewModel();
		}

		private void BackupJobList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				ViewModels.BackupJobListViewModel vm = (ViewModels.BackupJobListViewModel)DataContext;
				vm.SelectionChanged(sender, e);
			}
		}
	}
}
