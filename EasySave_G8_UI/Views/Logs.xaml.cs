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
            String folderPath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\logs\";// Get the files in the folder
            string[] files = Directory.GetFiles(folderPath);
            textBoxLogs.Clear();// Clear the contents of the text box
            foreach (string file in files)// Loop through the files and add them to the text box
            {
                textBoxLogs.AppendText(file + Environment.NewLine);
                textBoxLogs.AppendText(File.ReadAllText(file) + Environment.NewLine);
            }
        }

        private void ButtonStateLogs_Refresh(object sender, EventArgs e)
        {
            textBoxLogs.Text = "";
            String file = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\StateLog.json";
            textBoxLogs.AppendText(file + Environment.NewLine);
            textBoxLogs.AppendText(File.ReadAllText(file) + Environment.NewLine);
        }
    }
}
