using EasySave_G8_UI.Models;
using EasySave_G8_UI.View_Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace EasySave_G8_UI.Views
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
    }
}
