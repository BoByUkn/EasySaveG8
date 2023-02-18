using EasySave_G8_UI.Models;
using EasySave_G8_UI.View_Models;
using EasySave_G8_UI.Views;
using EasySave_G8_UI.Views.Works;
using System;
using System.IO;
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
            View_Model ViewMODEL = new View_Model();
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave";
            if (!Directory.Exists(fileName)) //Check if mandatory files are present or not. If not, creates them
            {
                ViewMODEL.VM_Init();
            }

            translate();
            Main.Content = new Dashboard();
            if ($"{View_Model.VM_GetString_Language("lang")}" == "en") { FRradio.IsChecked = true; }
            else { ENradio.IsChecked = true; }
        }


        private void translate()
        {
            View_Model ViewModelLang = new View_Model();
            ViewModelLang.VM_Update_Language(); //Get the language from app_conf JSON file
            Main.Content = new Dashboard();
            Dashboard_btn.Content = $"{View_Model.VM_GetString_Language("dashboard")}";
            Classics_btn.Content = $"{View_Model.VM_GetString_Language("classics")}";
            Works_btn.Content = $"{View_Model.VM_GetString_Language("works")}";
            Logs_btn.Content = $"{View_Model.VM_GetString_Language("logs")}";
            Shutdown_btn.Content = $"{View_Model.VM_GetString_Language("shutdown")}";
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

        private void Loading_btn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Loading();
        }

        private void Settings_btn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Settings();
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void EngButton_Checked(object sender, RoutedEventArgs e)
        {
            View_Model ViewModelENG = new View_Model();
            ViewModelENG.VM_Change_Language("en");
            translate();
        }

        private void FraButton_Checked(object sender, RoutedEventArgs e)
        {
            View_Model ViewModelFR = new View_Model();
            ViewModelFR.VM_Change_Language("fr");
            translate();
        }
    }
}
