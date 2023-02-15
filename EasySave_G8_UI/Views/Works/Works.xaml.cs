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
    }
}
