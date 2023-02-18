using Newtonsoft.Json;
using EasySave_G8_UI.View_Models;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.Runtime.ConstrainedExecution;
using System.Windows.Controls;

namespace EasySave_G8_UI.Models
{
    public class Model_COMMON
    {
        public string lang { get; set; }
        public Model_COMMON(string lang) { this.lang = lang; }

        public void Init(Model_COMMON ModelCOMMON) //Create the mandatory files in appdata
        {
            Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave"); //Creates "EasySave" folder in Roaming
            Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\logs"); //Creates "logs" subfolder
            Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\logs\JSON\");
            Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\logs\XML\");
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\app_config.json";
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(ModelCOMMON); //Serialialize the data in JSON form
            File.WriteAllText(fileName, jsonString); //Create and append JSON into file
            var RandomInt64 = new Random();
            long cipherKey = RandomInt64.NextInt64(); //Generates a random 64bit key for CryptoSoft
            string filePath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\cipherkey.txt"; //Creates a file to store the key
            File.Create(filePath).Dispose();
            using (StreamWriter writer = new StreamWriter(filePath)) //Writes the key into that file
            {
                writer.Write(cipherKey);
            }
        }
    }

    public class Model_LANG
    {
        private static ResourceManager _rm;
        public string lang { get; set; }
        public Model_LANG(string? lang) { this.lang = lang; }

        static Model_LANG() //Initiate Lang RessourceManager
        {
            _rm = new ResourceManager("EasySave_G8_UI.Assets.Language.strings", Assembly.GetExecutingAssembly());
        }

        public static string? GetString(string name) //Get the string from the resx files
        {
            return _rm.GetString(name);
        }

        public void ChangeLanguage() //Change the Language stored in app_config
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\app_config.json";
            var jsonString = JsonConvert.SerializeObject(this); //Serialialize the data in JSON form
            File.WriteAllText(fileName, jsonString); //Create and append JSON into file
            UpdateLanguage();

        }

        public string GetLanguage() //Get the Language stored in app_config
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\app_config.json";
            string fileContent = File.ReadAllText(fileName); // Bring content of filename in filecontent
            Model_LANG base_conf = JsonConvert.DeserializeObject<Model_LANG>(fileContent); // Create the list named values
            return base_conf.lang;
        }

        public void UpdateLanguage() //Update CultureInfo with Language stored in app_config
        {
            string language = GetLanguage();
            var cultureInfo = new CultureInfo(language);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
    }


    public class Model_BLACKLIST
    {
        public string[] blacklist { get; set; }
        public bool BlacklistTest()
        {
            string[] blacklistProcessus = { "CalculatorApp" };
            bool blacklistState = false;

            var processes = Process.GetProcesses();

            foreach (Process process in Process.GetProcesses())
            {
                if(Array.IndexOf(blacklistProcessus, process.ProcessName) != -1)
                {
                    blacklistState = true;
                }
            }
            return blacklistState;

        }

        public string[] BlacklistReturn()
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\app_config.json";
            string fileContent = File.ReadAllText(fileName); // Bring content of filename in filecontent
            Model_BLACKLIST base_conf = JsonConvert.DeserializeObject<Model_BLACKLIST>(fileContent); // Create the list named values
            return base_conf.blacklist;

        }
    }
}



