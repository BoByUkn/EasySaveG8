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
            textBoxLogs.Text = "";

            string logsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave", "logs");
            string[] logFiles = Directory.GetFiles(logsFolder);

            foreach (string logFile in logFiles)
            {
                textBoxLogs.Text += File.ReadAllText(logFile) + Environment.NewLine;
            }
        }
        private void ButtonStateLogs_Refresh(object sender, EventArgs e)
        {
            textBoxLogs.Text = "";
            textBoxLogs.Text = File.ReadAllText(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\StateLog.json");
        }
    }
}

