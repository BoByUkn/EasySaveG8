using EasySave_G8_UI.View_Models;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using System;
using static EasySave_G8_UI.Views.Loading;

namespace EasySave_G8_UI.Views
{
    /// <summary>
    /// Logique d'interaction pour Classics.xaml
    /// </summary>
    public partial class Classics : Page
    {
        public Classics()
        {
            InitializeComponent();
            translate();
        }

        private void translate()
        {
            Classics_Title.Text = Name.Text = $"{View_Model.VM_GetString_Language("classics_title")}";
            Name.Text = $"{View_Model.VM_GetString_Language("name")}";
            Source_Path.Text = $"{View_Model.VM_GetString_Language("source_path")}";
            Dest_Path.Text = $"{View_Model.VM_GetString_Language("dest_path")}";
            LaunchBtn.Content = $"{View_Model.VM_GetString_Language("launch_save")}";
            Complete.Content = $"{View_Model.VM_GetString_Language("complete")}";
            Differential.Content = $"{View_Model.VM_GetString_Language("differential")}";
            Browse.Content = $"{View_Model.VM_GetString_Language("browse")}";
            Browse2.Content = $"{View_Model.VM_GetString_Language("browse")}";

        }

        private void Button_Click_LaunchSave(object sender, RoutedEventArgs e)
        {
            View_Model ViewModel = new View_Model();
            
            bool blacklist = ViewModel.VM_BlackList();
            if(blacklist == false)
            {
                string Name = this.textBox1.Text;
                if (ViewModel.VM_StateLogsExists(Name)) { System.Windows.MessageBox.Show("A work with that Name already exists. Please use another Name.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

                string Source = this.textBox2.Text;
                string Destination = this.textBox3.Text;
                int indexType = this.comboBox1.SelectedIndex;
                bool Type;
                if (indexType == 0) { Type = true; }
                else { Type = false; }

                try 
                { 
                    ViewModel.VM_Classic(Name, Source, Destination, Type);
                }
                catch (Exception) { System.Windows.MessageBox.Show("You can't launch a save without all parameters", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }
            }
        }

        private void Button_Click_Browse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.ValidateNames = false;
            openFileDialog.FileName = $"{View_Model.VM_GetString_Language("select_directory")}";
            openFileDialog.Filter = $"{View_Model.VM_GetString_Language("directories")}| *.directory";
            openFileDialog.InitialDirectory = @"C:\";

            if (openFileDialog.ShowDialog() == true)
            {
                textBox2.Text = Path.GetDirectoryName(openFileDialog.FileName);
            }
        }

        private void Button_Click_Browse2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.ValidateNames = false;
            openFileDialog.FileName = $"{View_Model.VM_GetString_Language("select_directory")}";
            openFileDialog.Filter = $"{View_Model.VM_GetString_Language("directories")}| *.directory";
            openFileDialog.InitialDirectory = @"C:\";

            if (openFileDialog.ShowDialog() == true)
            {
                textBox3.Text = Path.GetDirectoryName(openFileDialog.FileName);
            }
        }
    }
}
