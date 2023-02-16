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
            int i = 0;
            string WorkName = "";
            foreach (string Work in List_Works.SelectedItems)
            {
                i++;
                if (i>1) { MessageBox.Show("Only one Work can be edited at a time.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
                WorkName = Work;
            }
            if (i == 1) { mainWindow.Main.Content = new Works_Edit(WorkName); }
            else { MessageBox.Show("Please choose a Work in the list to edit it.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }
        private void List_Works_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List_Work_Detail.Text = "";
            if (List_Works.SelectedItems.Count == 0) { return; }

            View_Model ViewModel = new View_Model();
            List<Model_PRE> obj_list = ViewModel.VM_Work_Show((List_Works.Items[List_Works.SelectedIndex].ToString()), false);
            
            foreach(Model_PRE obj in obj_list)
            {
                List_Work_Detail.Text = "Name: " + obj.Name + "\n";
                List_Work_Detail.Text += "Source: " + obj.Source + "\n";
                List_Work_Detail.Text += "Destination: " + obj.Destination + "\n";
                if (obj.Type) { List_Work_Detail.Text += "Type: Complete \n"; }
                else { List_Work_Detail.Text += "Type: Differential \n"; }
            }
        }

        private void Next_btn_Click(object sender, RoutedEventArgs e)
        {
            if (List_Works.SelectedItems.Count == 0 || List_Works.SelectedItems.Count == 1) { return; }

            string CurrentWork = "";
            string WorkName = "";
            bool AfterCurrent = false;
            bool WorkNameFound = false;

            foreach (string Item in List_Works.SelectedItems)
            {
                if (List_Work_Detail.Text.Contains(Item)) { CurrentWork = Item; }
            }

            while (!WorkNameFound)
            {
                foreach (string Item in List_Works.SelectedItems)
                {
                    if (Item == CurrentWork) { AfterCurrent = true; }
                    if (Item != CurrentWork && AfterCurrent)
                    {
                        WorkName = Item;
                        WorkNameFound = true;
                    }
                    else if (AfterCurrent)
                    {
                        WorkName = Item;
                        WorkNameFound = true;
                    }
                }
            }

            List_Work_Detail.Text = "";

            View_Model ViewModel = new View_Model();
            List<Model_PRE> obj_list = ViewModel.VM_Work_Show(WorkName, false);

            foreach (Model_PRE obj in obj_list)
            {
                List_Work_Detail.Text = "Name: " + obj.Name + "\n";
                List_Work_Detail.Text += "Source: " + obj.Source + "\n";
                List_Work_Detail.Text += "Destination: " + obj.Destination + "\n";
                if (obj.Type) { List_Work_Detail.Text += "Type: Complete \n"; }
                else { List_Work_Detail.Text += "Type: Differential \n"; }
            }
        }
    }
}
