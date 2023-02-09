using EasySave_G8_CONS.View_Models;
using Newtonsoft.Json;

namespace EasySave_G8_CONS.Models
{
    public class Model_PRE
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public bool Type { get; set; }

        public Model_PRE(string Name, string Source, string Destination, bool Type)
        {
            this.Name = Name;
            this.Source = Source;
            this.Destination = Destination;
            this.Type = Type;
        }

        public void Exec() //Execute a backup
        {
            Model_AFT ModelAFT = new Model_AFT(this.Name, this.Source, this.Destination, this.Type);
            if(this.Type == true){ModelAFT.Run();} //Complete backup
            else if (this.Type == false){ModelAFT.RunDiff();} //Differential backup
            ModelAFT.Logs();
        }

        public void Save(bool ExeNow) //Save a work into work_conf 
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\works_config.json";
            if (File.Exists(fileName))  //Test if conf file exists, else it creates it and write JSON directly
            {
                string fileContent = File.ReadAllText(fileName); //Bring content of filename in filecontent
                List<Model_PRE> ?values = new List<Model_PRE>(); //Create the list named values
                values = JsonConvert.DeserializeObject<List<Model_PRE>>(fileContent); //Deserialialize the data in JSON form
                if (values.Count() == 5 || values.Count() > 5) //Check if there is already 5 works
                {
                    View_Model ViewMODEL = new View_Model();
                    ViewMODEL.MV_Work_5Works();
                    return;
                }
                values?.Add(this); //Add object ModelAFT in the list values
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(values, Newtonsoft.Json.Formatting.Indented); //Serialialize the data in JSON form
                File.WriteAllText(fileName, jsonString); //Write json file
            }
            else if (File.Exists(fileName) == false)
            {
                List<Model_PRE> values = new List<Model_PRE>(); //Create the list named values
                values.Add(this);//Add object ModelAFT in the list values
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(values, Newtonsoft.Json.Formatting.Indented); //Serialialize the data in JSON form
                File.WriteAllText(fileName, jsonString); //Write json file
            }
            if (ExeNow) { Exec();}
        }
    }
}