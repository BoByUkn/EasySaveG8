using EasySave_G8_UI.Views;
using System.Windows;
using System.Windows.Navigation;

namespace EasySave_G8_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new Dashboard();
        }

        private void Classics_Click(object sender, RoutedEventArgs e)
        {
            Main.Content= new Classics();
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Dashboard();
        }

        private void Works_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Works();
        }

        private void Logs_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Logs();
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Main_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
