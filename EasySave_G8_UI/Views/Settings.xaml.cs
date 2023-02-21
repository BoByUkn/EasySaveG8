using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
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
using EasySave_G8_UI.Views.Works;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
            ListRefresh();   
        }

        private void Blacklist_add_btn_Click(object sender, RoutedEventArgs e)
        {
            View_Model ViewModel = new View_Model();
            string ProcessName = Blacklist_add.Text;
            ViewModel.VM_BlackListAdd(ProcessName);
            PageRefresh();
        }

        private void Blacklist_rm_btn_Click(object sender, RoutedEventArgs e)
        {
            View_Model ViewModel = new View_Model();
            string ProcessNameRm = Blacklist_rm_combobox.Text;
            ViewModel.VM_BlackListRemove(ProcessNameRm);
            ListRefresh();
            PageRefresh();
        }

        private void ListRefresh()
        {
            View_Model ViewModel = new View_Model();
            List<string> blacklist = ViewModel.MV_Blacklist();
            int i;
            for (i = 0; i < blacklist.Count; i++)
            {
                Blacklist_rm_combobox.Items.Add(blacklist[i]);
            }
            List<string> prioritylist = ViewModel.MV_PriorityListRe();
            for (i = 0; i < prioritylist.Count; i++)
            {
                PriorityNumer_combobox.Items.Add(i);
                Prioritylist_rm_combobox1.Items.Add(prioritylist[i]);

            }
        }

        private void PageRefresh()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Main.Content = new Settings();
        }


        private void Prioritylist_add_btn_Click(object sender, RoutedEventArgs e)
        {
            View_Model ViewModel = new View_Model();
            string ExtensionName = Prioritylist_add1.Text;
            int index = PriorityNumer_combobox.SelectedIndex;
            if (index == -1) { index = 0; }
            ViewModel.VM_PriorityListAdd(ExtensionName, index);
            PageRefresh();

        }

        private void Prioritylist_rm_btn_Click(object sender, RoutedEventArgs e)
        {
            View_Model ViewModel = new View_Model();
            string ExtensionName = Prioritylist_rm_combobox1.Text;
            ViewModel.VM_PriorityListRemove(ExtensionName);
            ListRefresh();
            PageRefresh();
        }
    }
}
