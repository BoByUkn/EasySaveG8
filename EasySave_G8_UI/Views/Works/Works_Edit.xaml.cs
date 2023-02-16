using EasySave_G8_UI.View_Models;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using EasySave_G8_UI.Models;
using System.Collections.Generic;

namespace EasySave_G8_UI.Views.Works
{
    /// <summary>
    /// Logique d'interaction pour Works_Edit.xaml
    /// </summary>
    public partial class Works_Edit : Page
    {
        private string Original_WorkName { get; set; }

        public Works_Edit(string? WorkName)
        {
            InitializeComponent();
            Original_WorkName = WorkName;
            Get_Work_Data();
            translate();
        }

        private void translate()
        {
            Work_Edit.Text = $"{View_Model.VM_GetString_Language("work_edit")}";
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

        private void Get_Work_Data()
        {
            textBox1.Text = Original_WorkName;
            View_Model ViewModel = new View_Model();
            List<Model_PRE> Work_List = ViewModel.VM_Work_Show(Original_WorkName, false);
            foreach(Model_PRE obj in Work_List) 
            {
                textBox2.Text = obj.Source;
                textBox3.Text = obj.Destination;
                if (obj.Type) { comboBox1.SelectedIndex = 0; }
                else { comboBox1.SelectedIndex = 1;}
            }
        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            View_Model ViewModel = new View_Model();
            ViewModel.VM_Work_Delete(Original_WorkName);

            bool Type;
            if (comboBox1.Text == "Complète" || comboBox1.Text == "Complete") { Type = true; }
            else { Type = false; }

            bool ExeNow;
            if (comboBox2.Text == "Oui" || comboBox2.Text == "Yes") { ExeNow = true; }
            else { ExeNow = false; }

            ViewModel.VM_Work_New(textBox1.Text, textBox2.Text, textBox3.Text, Type, ExeNow);

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
