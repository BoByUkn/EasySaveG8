using System.IO;
using System;
using System.Windows.Controls;
using System.Windows.Forms;

namespace EasySave_G8_UI.Views
{
    /// <summary>
    /// Logique d'interaction pour Logs.xaml
    /// </summary>
    public partial class Logs : Page
    {
        public Logs()
        {
            InitializeComponent();
        }  


        private void ButtonLogs_Refresh(object sender, EventArgs e)
        {
            FolderBrowserDialog folderbrowserdialog1 = new FolderBrowserDialog();

            if (folderbrowserdialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxLogs.Text = "";

                string[] files = Directory.GetFiles(folderbrowserdialog1.SelectedPath);

                foreach (string file in files)
                {
                    textBoxLogs.Text += file + Environment.NewLine;
                }
            }
        }

        private void ButtonStateLogs_Refresh(object sender, EventArgs e)
        {
            textBoxLogs.Text = "";
            textBoxLogs.Text = File.ReadAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\StateLog.json");
        }
    }
}
