using EasySave_G8_UI.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.IO;
using System.Windows.Threading;
using Microsoft.Win32;

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
