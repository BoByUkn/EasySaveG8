using EasySave_G8_UI.Models;
using EasySave_G8_UI.View_Models;
using EasySave_G8_UI.Views;
using EasySave_G8_UI.Views.Works;
using System;
using System.Windows;
using System.Windows.Controls;
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
            ViewMODEL.VM_Init();
            translate();
            Main.Content = new Dashboard();
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

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Main_Navigated(object sender, NavigationEventArgs e)
        {

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


        //private void combobox_selectionchanged(object sender, system.windows.controls.selectionchangedeventargs e)
        //{
        //    if (lang.selectedindex == 0)
        //    {
        //        view_model viewmodeleng = new view_model();
        //        viewmodeleng.vm_change_language("en");
        //    }
        //    else
        //    {
        //        view_model viewmodelfr = new view_model();
        //        viewmodelfr.vm_change_language("fr");
        //    }
        //        translate();
        //}

    }
}
