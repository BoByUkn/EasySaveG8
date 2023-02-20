using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace EasySave_G8_UI.Models
{
    public class Model_Logs
    {
        public List<Model_AFT> Get_Logs(string Date) //Retrieve log file content
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\logs\JSON\" + Date + ".json";

            List<Model_AFT>? obj_list = new List<Model_AFT>(); //Create the list named obj_list to hold the return list
            List<Model_AFT>? values = new List<Model_AFT>(); //Create the list named values to hold deserialized data

            if (File.Exists(fileName))
            {
                string fileContent = File.ReadAllText(fileName); //Bring content of filename in filecontent
                values = JsonConvert.DeserializeObject<List<Model_AFT>>(fileContent); //Deserialialize the data in JSON form

                foreach (Model_AFT obj in values) //Loop throught every objects in the deserialized data
                {
                    obj_list.Add(obj); //Store the object into a list for return
                }
                return (obj_list);
            }
            return (obj_list); //Return empty list if today's log is not found
        }

        public List<Model_StateLogs> Get_StateLogs() 
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\StateLog.json";

            List<Model_StateLogs>? obj_list = new List<Model_StateLogs>(); //Create the list named obj_list to hold the return list

            if (File.Exists(fileName))
            {
                string fileContent = File.ReadAllText(fileName); //Bring content of filename in filecontent
                obj_list = JsonConvert.DeserializeObject<List<Model_StateLogs>>(fileContent); //Deserialialize the data in JSON form
                return (obj_list);
            }
            return (obj_list); //Return empty list if statelogs are not found
        }

        public int Get_StateLogsPercentage(string Name, string Source, string Destination, bool Type)
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\StateLogs.json";
            string fileContent = File.ReadAllText(fileName); // Bring content of filename in filecontent
            List<Model_StateLogs>? values = new List<Model_StateLogs>(); // Create the list named values
            values = JsonConvert.DeserializeObject<List<Model_StateLogs>>(fileContent); //Deserialialize the data in JSON form
            int progression = 0;
            foreach (Model_StateLogs obj in values) //Loop throught every objects in the deserialized data
            {
                if (obj.Name == Name && obj.Source == Source && obj.Destination == Destination && obj.Type == Type) //If we find the save we are looking for in a single work execution
                {
                    progression = obj.progression;   
                }
            }
            return progression;
        }






        public string Get_StateLogsState(string Name)
        {

            return "a";
        }
    }




    public class Model_StateLogs : Model_AFT
    {
        public int progression { get; set; }
        public string State { get; set; }
        public Model_StateLogs(string Name, string Source, string Destination, bool Type, int file_remain) : base(Name, Source, Destination, Type)
        {
            this.Name = Name;
            this.Source = Source;
            this.Destination = Destination;
            this.Type = Type;
            this.Size = 0;
            this.file_remain = file_remain;
            this.total_files = Directory.GetFiles(Source, "*.*", SearchOption.AllDirectories).Length;
            this.progression = 0;
            this.State = "STARTED";
        }
    }




}
