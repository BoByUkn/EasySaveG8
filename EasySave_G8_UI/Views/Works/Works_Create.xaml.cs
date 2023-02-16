using EasySave_G8_UI.View_Models;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;

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
            if (comboBox1.Text == "Complète" || comboBox1.Text == "Complete") { Type = true; }
            else { Type = false; }

            bool ExeNow;
            if (comboBox2.Text == "Oui" || comboBox2.Text == "Yes") { ExeNow = true; } 
            else { ExeNow = false; }

            ViewModel.VM_Work_New(Name, textBox2.Text, textBox3.Text, Type, ExeNow);

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
