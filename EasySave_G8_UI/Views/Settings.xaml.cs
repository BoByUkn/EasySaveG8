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
using EasySave_G8_UI.View_Models;

namespace EasySave_G8_UI.Views
{
    /// <summary>
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Blacklist_add_btn_Click(object sender, RoutedEventArgs e)
        {
            View_Model ViewModel = new View_Model();
            string ProcessName = Blacklist_add.Text;
            ViewModel.VM_BlackListAdd(ProcessName);
        }
    }
}
