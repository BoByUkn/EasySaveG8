using EasySave_G8_UI.Models;
using EasySave_G8_UI.View_Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EasySave_G8_UI.Views.Works
{
    /// <summary>
    /// Logique d'interaction pour Works.xaml
    /// </summary>
    public partial class Works : Page
    {
        public Works()
        {
            InitializeComponent();
            Works_List();
        }

        private void Works_List()
        {
            List_Works.Items.Clear();
            View_Model ViewMODEL = new View_Model();
            List<Model_PRE>? WorkList = ViewMODEL.VM_Work_Show(null, true);
            foreach(Model_PRE obj in WorkList)
            {
                List_Works.Items.Add(obj.Name);
            }
        }

        private void Create_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Main.Content = new Works_Create();
        }

        private void ExecuteAll_btn_Click(object sender, RoutedEventArgs e)
        {
            View_Model ViewModel = new View_Model();
            ViewModel.VM_Work_Run("", true);
        }

        private void ExecuteSelected_btn_Click(object sender, RoutedEventArgs e)
        {
            View_Model ViewModel = new View_Model();
            foreach(string WorkName in List_Works.SelectedItems)
            {
                ViewModel.VM_Work_Run(WorkName, false);
            }
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            View_Model ViewModel = new View_Model();
            foreach (string WorkName in List_Works.SelectedItems)
            {
                ViewModel.VM_Work_Delete(WorkName);
            }
            Works_List();
        }

        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Main.Content = new Works_Create();
        }
    }
}
