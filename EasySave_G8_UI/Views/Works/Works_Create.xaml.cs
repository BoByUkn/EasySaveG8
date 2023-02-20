using EasySave_G8_UI.View_Models;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using System;

namespace EasySave_G8_UI.Views.Works
{
    /// <summary>
    /// Logique d'interaction pour Works_Create.xaml
    /// </summary>
    public partial class Works_Create : Page
    {
        public Works_Create()
        {
            InitializeComponent();
            translate();
        }

        private void translate()
        {
            CreateWork_Title.Text = $"{View_Model.VM_GetString_Language("create_work_title")}";
            Name.Text = $"{View_Model.VM_GetString_Language("name")}";
            Source_Path.Text = $"{View_Model.VM_GetString_Language("source_path")}";
            Dest_Path.Text = $"{View_Model.VM_GetString_Language("dest_path")}";
            Browse.Content = $"{View_Model.VM_GetString_Language("browse")}";
            Browse2.Content = $"{View_Model.VM_GetString_Language("browse")}";
            Save_btn.Content = $"{View_Model.VM_GetString_Language("save_work")}";
            Execute_Now.Text = $"{View_Model.VM_GetString_Language("execute_now")}";
            Complete.Content = $"{View_Model.VM_GetString_Language("complete")}";
            Differential.Content = $"{View_Model.VM_GetString_Language("differential")}";
            Yes.Content = $"{View_Model.VM_GetString_Language("yes")}";
            No.Content = $"{View_Model.VM_GetString_Language("no")}";
        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            View_Model ViewModel = new View_Model();
            string Name = textBox1.Text;
            if (ViewModel.VM_Work_Exist(Name))
            {
                LabelError.Content = "This name is already taken.";
                return;
            }

            bool Type;
            if (comboBox1.SelectedIndex == 0) { Type = true; }
            else { Type = false; }

            bool ExeNow;
            if (comboBox2.SelectedIndex == 0) { ExeNow = true; } 
            else { ExeNow = false; }

            bool blacklist_state = ViewModel.VM_BlackListTest();
            if (((blacklist_state == false) && (ExeNow == true)) || (ExeNow == false))
            {
                ViewModel.VM_Work_New(Name, textBox2.Text, textBox3.Text, Type, ExeNow);
            }
            else
            {
                MessageBox.Show("Erreur : Le travail ne peut pas être exécuté car un processus de la blacklist est en exécution.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Main.Content = new Works();


        }

        private void Button_Click_Browse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.ValidateNames = false;
            openFileDialog.FileName = "Selectionner un dossier";
            openFileDialog.Filter = "Dossiers|*.directory";
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
            openFileDialog.FileName = "Selectionner un dossier";
            openFileDialog.Filter = "Dossiers|*.directory";
            openFileDialog.InitialDirectory = @"C:\";

            if (openFileDialog.ShowDialog() == true)
            {
                textBox3.Text = Path.GetDirectoryName(openFileDialog.FileName);
            }
        }
    }
}
