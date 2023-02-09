using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

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
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\app_config.json";
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(ModelCOMMON); //Serialialize the data in JSON form
            File.WriteAllText(fileName, jsonString); //Create and append JSON into file
        }
    }

    public class Model_LANG
    {
        private static ResourceManager _rm;
        public string lang { get; set; }
        public Model_LANG(string? lang) { this.lang = lang; }

        static Model_LANG() //Initiate Lang RessourceManager
        {
            _rm = new ResourceManager("EasySave_G8_CONS.Assets.Language.strings", Assembly.GetExecutingAssembly());
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
            return (base_conf.lang);
        }

        public void UpdateLanguage() //Update CultureInfo with Language stored in app_config
        {
            string language = GetLanguage();
            var cultureInfo = new CultureInfo(language);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
    }
}



