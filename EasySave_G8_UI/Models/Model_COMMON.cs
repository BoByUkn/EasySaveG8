﻿using Newtonsoft.Json;
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
using System.Collections.Generic;
using System.Windows;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static EasySave_G8_UI.Models.Model_BLACKLIST;

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

            string fileName2 = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\blackList.json";
            Model_BLACKLIST bLACKLIST = new Model_BLACKLIST();
            var jsonString2 = JsonConvert.SerializeObject(bLACKLIST); //Serialialize the data in JSON form
            File.WriteAllText(fileName2, jsonString2); //Create and append JSON into file

            string fileNamePriorityFiles = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\priority.json";
            Model_PRIORITY PriorityFiles = new Model_PRIORITY();
            var jsonStringPriority = JsonConvert.SerializeObject(PriorityFiles); //Serialialize the data in JSON form
            File.WriteAllText(fileNamePriorityFiles, jsonStringPriority); //Create and append JSON into file


            var RandomInt64 = new Random();
            long cipherKey = RandomInt64.NextInt64(); //Generates a random 64bit key for CryptoSoft
            string filePath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\cipherkey.txt";
            //File.Create(filePath); //Creates a file to store the key
            //using (StreamWriter writer = new StreamWriter(filePath)) //Writes the key into that file
            //{
                //writer.Write(cipherKey);
            //}
            File.WriteAllText(filePath, cipherKey.ToString());
        }
    }

    public class Model_LANG
    {
        private static ResourceManager _rm;
        public string lang { get; set; }
        //public List<string> blacklist { get; set; }
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

            string fileContent = File.ReadAllText(fileName); // Bring content of filename in filecontent
            Model_LANG base_conf = JsonConvert.DeserializeObject<Model_LANG>(fileContent); // Create the list named values
            base_conf.lang = this.lang;

            var jsonString = JsonConvert.SerializeObject(base_conf); //Serialialize the data in JSON form
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
        public List<string> blacklist { get; set; }
        public Model_BLACKLIST()
        {
            blacklist = new List<string>();
        }
        public bool BlacklistTest()
        {
            blacklist = BlacklistReturn();
            bool blacklistState = false;

            var processes = Process.GetProcesses();

            foreach (Process process in Process.GetProcesses())
            {
                if (blacklist.Contains(process.ProcessName))
                {
                    blacklistState = true;
                }
            }
            return blacklistState;

        }

        public List<string> BlacklistReturn()
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\blackList.json";
            string fileContent = File.ReadAllText(fileName); // Bring content of filename in filecontent
            Model_BLACKLIST black_list = JsonConvert.DeserializeObject<Model_BLACKLIST>(fileContent); // Create the list named values
            return black_list.blacklist;
        }

        public void BlacklistAdd(string ProcessName)
        {
            List<string> blacklist = this.BlacklistReturn();
            bool ProcExistTest = false;
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\blackList.json";
            if (blacklist != null)
            {
                foreach (string Processname in blacklist)
                {
                    if (ProcessName == Processname)
                    {
                        ProcExistTest = true;
                    }
                }
                if (ProcExistTest == false)
                {
                    blacklist.Add(ProcessName);
                    this.blacklist = blacklist;
                    Trace.Write(blacklist);

                    var jsonString = JsonConvert.SerializeObject(this); //Serialialize the data in JSON form
                    File.WriteAllText(fileName, jsonString); //Create and append JSON into file
                }
            }
            else
            {
                blacklist.Add(ProcessName);
                var jsonString = JsonConvert.SerializeObject(this); //Serialialize the data in JSON form
                File.WriteAllText(fileName, jsonString); //Create and append JSON into file
            }
        }

        public void BlacklistRemove(string ProcessNameRm)
        {
            List<string> blacklist = this.BlacklistReturn();
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\blackList.json";
            blacklist.Remove(ProcessNameRm);
            this.blacklist = blacklist;
            var jsonString = JsonConvert.SerializeObject(this); //Serialialize the data in JSON form
            File.WriteAllText(fileName, jsonString); //Create and append JSON into file
        }

    }


    public class Model_PRIORITY
    {
        public List<string> priority { get; set; }
        public Model_PRIORITY()
        {
            this.priority = new List<string>();
        }

        public List<string> priorityReturn()
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\priority.json";
            string fileContent = File.ReadAllText(fileName); // Bring content of filename in filecontent
            Model_PRIORITY base_conf_priority = JsonConvert.DeserializeObject<Model_PRIORITY>(fileContent); // Create the list named values
            return base_conf_priority.priority;
        }

        public void priorityAdd(string ExtensionName, int indexToInsert)
        {
            List<string> priority = this.priorityReturn();
            bool ProcExistTest = false;
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\priority.json";
            if (priority != null)
            {
                foreach (string Extensionname in priority)
                {
                    if (Extensionname == ExtensionName)
                    {
                        ProcExistTest = true;
                    }
                }
                if (ProcExistTest == false)
                {
                    //priority.Insert(indexToInsert, ExtensionName);
                    priority.Add(ExtensionName);
                    this.priority = priority;
                    var jsonString = JsonConvert.SerializeObject(this); //Serialialize the data in JSON form
                    File.WriteAllText(fileName, jsonString); //Create and append JSON into file
                }
            }
            else
            {
                priority.Insert(indexToInsert, ExtensionName);
                var jsonString = JsonConvert.SerializeObject(this); //Serialialize the data in JSON form
                File.WriteAllText(fileName, jsonString); //Create and append JSON into file}
            }
        }


        public void priorityRemove(string PrirorityRm)
        {
            List<string> priority = this.priorityReturn();
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\priority.json";
            priority.Remove(PrirorityRm);
            this.priority = priority;
            var jsonString = JsonConvert.SerializeObject(this); //Serialialize the data in JSON form
            File.WriteAllText(fileName, jsonString); //Create and append JSON into file
        }
    }
}



