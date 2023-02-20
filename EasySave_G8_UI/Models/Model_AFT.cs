using EasySave_G8_UI.View_Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Threading;

namespace EasySave_G8_UI.Models
{
    public class Model_AFT : Model_PRE
    {
        public double Size { get; set; }
        public string utcDateString { get; set; }
        private DateTime utcDateDateTime { get; set; }
        private DateTime utcDateStart { get; set; }
        private DateTime utcDateFinish { get; set; }
        private TimeSpan Duration { get; set; }
        public int total_files { get; set; }
        public int file_remain { get; set; }
        public double millisecondsDuration { get; set; }

        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        View_Model VM = new View_Model();
        private double ActualSize2 = 0;
        public Model_AFT(){}
        public Model_AFT(string Name, string Source, string Destination, bool Type) : base(Name, Source, Destination, Type)
        {
            this.Name = Name;
            this.Source = Source;
            this.Destination = Destination;
            this.Type = Type;
            this.Size = 0;
            this.utcDateDateTime = DateTime.Now;
            this.utcDateString = utcDateDateTime.ToString("dd/MM/yyyy HH:mm:ss");
            this.Duration = TimeSpan.Zero;
            this.millisecondsDuration=0;
            this.total_files = Directory.GetFiles(Source, "*.*", SearchOption.AllDirectories).Length;
        }
        public void Run() //Run a backup
        {
            {
                Model_StateLogs ModelStateLogs = new Model_StateLogs(this.Name, this.Source, this.Destination, this.Type, this.total_files);

                if (File.Exists(Source)) //If it's a file
                {
                    utcDateStart = DateTime.Now;

                    File.Copy(Source, Destination, true);
                    Size = new System.IO.FileInfo(Source).Length;

                    utcDateFinish = DateTime.Now;
                }
                else if (Directory.Exists(Source)) //If it's a folder
                {
                    utcDateStart = DateTime.Now;
                    var files = Directory.GetFiles(Source, "*.*", SearchOption.AllDirectories); //Get folders and files in the source directory
                    Directory.CreateDirectory(Destination); //Create the destination directory if it doesn't exist
                    
                    string Destination2;
                    Destination2 = Destination + @"\" + Path.GetFileName(Source); //Combine the destination directory with the file name of the source 
                    Directory.CreateDirectory(Destination2); //Create the destination directory
                    file_remain = total_files;

                    foreach (var file in files) //Loop throught every files and copy them
                    {
                        Size = Size + new System.IO.FileInfo(file).Length;//Increment size with each file
                    }
                    ModelStateLogs.Size = Size;
                    foreach (var file in files) //Loop throught every files and copy them
                    {
                        string targetFile = file.Replace(Source, Destination2);

                        ActualSize2 = ActualSize2 + new System.IO.FileInfo(file).Length;//Increment size with each file
                        
                        int percentage = (int)(((double)ActualSize2 / (double)Size) * 100);//progression's percentage of the save

                        Directory.CreateDirectory(Path.GetDirectoryName(targetFile)); // Create a directory
                        File.Copy(file, targetFile,true); // Do the copy
                        file_remain = file_remain - 1; // File remain decrease when a file copy have been done


                        ModelStateLogs.progression = percentage; // actualize the progression attribute on ModelStateLogs with actual percentage
                        ModelStateLogs.file_remain = file_remain; // actualize the file remain attribute on ModelStateLogs with actual percentage
                        StateLog(ModelStateLogs); // Write the json state logs with new infos (it changes at each iteration)
                        }
                    ModelStateLogs.file_remain = file_remain;
                    ModelStateLogs.State = "ENDED";
                   
                    utcDateFinish = DateTime.Now;
                }
                
                Duration = utcDateFinish.Subtract(utcDateStart);  // Calculation of the result of the arrival date - the departure date to obtain a duration, it's in TimeSpan, it is the result of the subtraction of two DataTime

                millisecondsDuration = Duration.TotalMilliseconds; // Convert Duration in milliseconds

                ModelStateLogs.millisecondsDuration = millisecondsDuration; //add millisecondsDuration to the object ModelStateLogs
                StateLog(ModelStateLogs);// Write the JSon State Logs with all info 
            }
        }

        public void RunDiff() //Execute a differential backup
        {
            {
                Model_StateLogs ModelStateLogs = new Model_StateLogs(this.Name, this.Source, this.Destination, this.Type, this.total_files);
                if (!Directory.Exists(Destination)) { Directory.CreateDirectory(Destination); } //Create the destination directory if it doesn't exist
                if (Directory.Exists(Source))
                {
                    utcDateStart = DateTime.Now;

                    string[] sourceFiles = Directory.GetFiles(Source, "*.*", SearchOption.AllDirectories); // Get the list of files in the source directory
                    string Destination2;

                    Destination2 = Destination + @"\" + Path.GetFileName(Source); //Combine the destination directory with the file name of the source 
                    Directory.CreateDirectory(Destination2); //Create the destination directory

                    file_remain = total_files;

                    foreach (var file in sourceFiles) //Loop throught every files and copy them
                    {
                        Size = Size + new System.IO.FileInfo(file).Length;//Increment size with each file
                    }
                    ModelStateLogs.Size = Size;
                    foreach (string sourceFile in sourceFiles) // Browse each file in the source directory
                    {
                        string destinationFile = sourceFile.Replace(Source, Destination2); // Create a destination path for the file
                        ActualSize2 = ActualSize2 + new System.IO.FileInfo(sourceFile).Length;//Increment size with each file
                        int percentage = (int)(((double)ActualSize2 / (double)Size) * 100);
                        ModelStateLogs.progression = percentage;

                        file_remain = file_remain - 1;

                        if (File.Exists(destinationFile)) // Check if the file already exists in the backup directory
                        {
                            FileInfo sourceFileInfo = new FileInfo(sourceFile); // Get information about source and destination files
                            FileInfo destinationFileInfo = new FileInfo(destinationFile);
                            
                            if (sourceFileInfo.LastWriteTime > destinationFileInfo.LastWriteTime) // Check if the file has been modified in the source directory
                            {
                                File.Copy(sourceFile, destinationFile, true); //Copy the modified file to the backup directory
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));// Copy the file that does not exist to the backup directory
                            File.Copy(sourceFile, destinationFile, false);
                        }
                        ModelStateLogs.file_remain = file_remain;
                        StateLog(ModelStateLogs);

                    }
                    utcDateFinish = DateTime.Now;
                }                
                Duration = utcDateFinish.Subtract(utcDateStart); // Calculation of the result of the arrival date - the departure date to obtain a duration, it's in TimeSpan, it is the result of the subtraction of two DataTime
                millisecondsDuration = Duration.TotalMilliseconds; // Convert Duration in milliseconds

                ModelStateLogs.millisecondsDuration = millisecondsDuration; //add millisecondsDuration to the object ModelStateLogs
                ModelStateLogs.State = "ENDED"; // Uptadte status of the save in order to write it in Json state logs
                StateLog(ModelStateLogs); // Write the JSon State Logs with all info 
            }
        }

        public void Logs() //Write backup's logs
        {
            _semaphore.Wait();
            string utcDateOnly = utcDateDateTime.ToString("dd/MM/yyyy");
            utcDateOnly = utcDateOnly.Replace("/", "-"); //Format the date to allow serializing
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\logs\JSON\" + utcDateOnly + ".json";
            string fileName2 = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\logs\XML\" + utcDateOnly + ".xml";

            if (File.Exists(fileName))  //Test if log file exists, else it creates it
            {
                string fileContent = File.ReadAllText(fileName); //Bring content of filename in filecontent
                
                List<Model_AFT> ?values = new List<Model_AFT>(); //Create the list named values
                values = JsonConvert.DeserializeObject<List<Model_AFT>>(fileContent); //Deserialialize the data in JSON form
                values?.Add(this); //Add object ModelAFT in the list values
                
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(values, Newtonsoft.Json.Formatting.Indented); //Serialialize the data in JSON form
                File.WriteAllText(fileName, jsonString); //Write json file

                XmlSerializer serializer = new XmlSerializer(typeof(List<Model_AFT>));
                StreamWriter writer = new StreamWriter(fileName2);
                serializer.Serialize(writer, values);
                writer.Close();
            }

            else if (!File.Exists(fileName))
            {
                List<Model_AFT> values = new List<Model_AFT>(); // create the list named values
                values.Add(this);// Add object ModelAFT in the list values
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(values, Newtonsoft.Json.Formatting.Indented); //Serialialize the data in JSON form
                File.WriteAllText(fileName, jsonString); // Write json file

                XmlSerializer serializer = new XmlSerializer(typeof(List<Model_AFT>));
                StreamWriter writer = new StreamWriter(fileName2);
                serializer.Serialize(writer, values);
                writer.Close();
            }
            _semaphore.Release();
        }

        private void StateLog(Model_StateLogs statelog) //Write backup's state logs
        {
            _semaphore.Wait();
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\StateLog.json";
            if (File.Exists(fileName))  //Test if log file exists, else it creates it
            {
                string fileContent = File.ReadAllText(fileName); // Bring content of filename in filecontent
                List<Model_StateLogs>? values = new List<Model_StateLogs>(); // Create the list named values
                List<Model_StateLogs>? new_values = new List<Model_StateLogs>();
                values = JsonConvert.DeserializeObject<List<Model_StateLogs>>(fileContent); //Deserialialize the data in JSON form

                foreach (Model_StateLogs obj in values) //Loop throught every objects in the deserialized data
                {
                    if (!(obj.Name == statelog.Name)) //If we find the save we are looking for in a single work execution
                    {
                        new_values.Add(obj);
                    }
                }
                new_values.Add(statelog);
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new_values, Newtonsoft.Json.Formatting.Indented); //Serialialize the data in JSON form
                File.WriteAllText(fileName, jsonString); // Write json file
            }
            else if (File.Exists(fileName) == false)
            {
                List<Model_StateLogs> values = new List<Model_StateLogs>(); // create the list named values
                values.Add(statelog);// Add object Model_StateLogs in the list values
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(values, Newtonsoft.Json.Formatting.Indented); //Serialialize the data in JSON form
                File.WriteAllText(fileName, jsonString); // Write json file
            }
            _semaphore.Release();
        }
    }
}