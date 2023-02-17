using System.IO;
using System;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using EasySave_G8_UI.View_Models;
using System.Windows.Forms;
using System.Windows;

namespace EasySave_G8_UI.Views
{
    /// <summary>
    /// Logique d'interaction pour Logs.xaml
    /// </summary>
    public partial class Logs : System.Windows.Controls.Page
    {
        public Logs()
        {
            InitializeComponent();
            translate();
        }

        private void translate()
        {
            Logs_Title.Text = $"{View_Model.VM_GetString_Language("logs_title")}";
            Daily_Logs.Content = $"{View_Model.VM_GetString_Language("daily_logs")}";
            State_Logs.Content = $"{View_Model.VM_GetString_Language("state_logs")}";
        }

        private void ButtonLogs_Refresh(object sender, EventArgs e)
        {
            try
            {
                if (LogsButton.Content == "XML")
            {
                ChangeLogs("XML");
            }
            else
            {
                ChangeLogs("JSON");
            }

            }
            catch (Exception) { System.Windows.MessageBox.Show("You don't have StateLogs file yet, please launch classic save or work first", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }

        private void ButtonStateLogs_Refresh(object sender, EventArgs e)
        {
            try
            {
                textBoxLogs.Text = "";
            String file = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\StateLog.json";
            textBoxLogs.AppendText(file + Environment.NewLine);
            textBoxLogs.AppendText(File.ReadAllText(file) + Environment.NewLine);
            }
            catch (Exception) { System.Windows.MessageBox.Show("You don't have StateLogs file yet, please launch classic save or work first", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }


        private void LogsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try 
            { 

                if (LogsButton.Content == "XML")
                {
                    LogsButton.Content = "JSON";
                    ChangeLogs("JSON");
                }
                else
                {
                    LogsButton.Content = "XML";
                    ChangeLogs("XML");
                }
            }
            catch (Exception) { System.Windows.MessageBox.Show("You don't have logs files yet, please launch classic save or work first", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }


        private void ChangeLogs(string type)
        {
            try
            {
                String folderPath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\logs\" + type + @"\";// Get the files in the folder
            string[] files = Directory.GetFiles(folderPath);
            textBoxLogs.Clear();// Clear the contents of the text box
            foreach (string file in files)// Loop through the files and add them to the text box
            {
                textBoxLogs.AppendText(file + Environment.NewLine);
                textBoxLogs.AppendText(File.ReadAllText(file) + Environment.NewLine);
            }
            }
            catch (Exception) { System.Windows.MessageBox.Show("You don't have logs files yet, please launch classic save or work first", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }
    }
}
