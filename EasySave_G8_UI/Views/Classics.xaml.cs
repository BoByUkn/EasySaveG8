using EasySave_G8_UI.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Name = this.textBox1.Text;
            string Source = this.textBox2.Text;
            string Destination = this.textBox3.Text;
            int indexType = this.comboBox1.SelectedIndex;
            bool Type;
            if (indexType == 0) { Type = true; }
            else { Type = false; }

            View_Model Classic_Save = new View_Model();
            Classic_Save.VM_Classic(Name, Source, Destination, Type);


        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
