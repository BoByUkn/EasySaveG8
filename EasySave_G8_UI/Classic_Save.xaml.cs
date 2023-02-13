using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasySave_G8_UI.View_Models;

namespace EasySave_G8_UI
{
    /// <summary>
    /// Logique d'interaction pour Classic_Save.xaml
    /// </summary>
    public partial class Classic_Save : Page
    {
        public Classic_Save()
        {
            InitializeComponent();



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Name = this.textBox1.Text;
            string Source = this.textBox2.Text ;
            string Destination = this.textBox3.Text;
            int indexType = this.comboBox1.SelectedIndex;
            bool Type;
            if (indexType == 0) {Type = true;}
            else { Type = false;}

            View_Model Classic_Save = new View_Model();
            Classic_Save.VM_Classic(Name,Source,Destination,Type);
        }
    }
}
