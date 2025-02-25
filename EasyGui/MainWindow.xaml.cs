using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using EasyCmd.Model;
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

        private void SaveMaxFileSize_Click(object sender, RoutedEventArgs e)
        {
            if (long.TryParse(MaxFileSizeTextBox.Text, out long maxFileSizeInKB))
            {
                // Create a BackupJob instance with the new value
                BackupJob backupJob = new BackupJob("Nom", "Source", "Destination", 1, maxFileSizeInKB);
                // Use the BackupJob instance as needed
            }
            else
            {
                MessageBox.Show("Veuillez entrer une valeur valide pour la taille maximale des fichiers.");
            }
        }
    }
}
