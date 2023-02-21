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
        private double ActualSize2 = 0;
        private Model_Logs ModelLogs = new Model_Logs();
        
        private SemaphoreSlim _semaphorefiles = new SemaphoreSlim(1);
        private SemaphoreSlim _semaphorelogs = new SemaphoreSlim(1);

        public Model_AFT () { }
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
                    string Destination2;
                    var files = Directory.GetFiles(Source, "*.*", SearchOption.AllDirectories); //Get folders and files in the source directory
                    Destination2 = Destination + @"\" + Path.GetFileName(Source); //Combine the destination directory with the file name of the source 

                    Directory.CreateDirectory(Destination); //Create the destination directory if it doesn't exist
                    Directory.CreateDirectory(Destination2); //Create the destination directory                    
                    
                    file_remain = total_files;
                    foreach (var file in files) //Loop throught every files and copy them
                    {
                        Size = Size + new FileInfo(file).Length;//Increment size with each file
                    }

                    ModelStateLogs.Size = Size;

                    foreach (var file in files) //Loop throught every files and copy them
                    {
                        string targetFile = file.Replace(Source, Destination2);
                        ActualSize2 = ActualSize2 + new FileInfo(file).Length;//Increment size with each file
                        int percentage = (int)(((double)ActualSize2 / (double)Size) * 100);//progression's percentage of the save
                   
                        Directory.CreateDirectory(Path.GetDirectoryName(targetFile)); // Create a directory
                        
                        _semaphorefiles.Wait();
                        try { File.Copy(file, targetFile, true); } // Do the copy
                        finally { _semaphorefiles.Release(); }

                        file_remain = file_remain - 1; // File remain decrease when a file copy have been done
                        ModelStateLogs.progression = percentage; // actualize the progression attribute on ModelStateLogs with actual percentage
                        ModelStateLogs.file_remain = file_remain; // actualize the file remain attribute on ModelStateLogs with actual percentage

                        ModelLogs.StateLog(ModelStateLogs); // Write the json state logs with new infos (it changes at each iteration)
                    }
                    ModelStateLogs.file_remain = file_remain;
                    ModelStateLogs.State = "ENDED";
                    utcDateFinish = DateTime.Now;
                }
                Duration = utcDateFinish.Subtract(utcDateStart);  // Calculation of the result of the arrival date - the departure date to obtain a duration, it's in TimeSpan, it is the result of the subtraction of two DataTime
                millisecondsDuration = Duration.TotalMilliseconds; // Convert Duration in milliseconds
                ModelStateLogs.millisecondsDuration = millisecondsDuration; //add millisecondsDuration to the object ModelStateLogs
                ModelLogs.StateLog(ModelStateLogs);// Write the JSon State Logs with all info 
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
                                _semaphorefiles.Wait();
                                try { File.Copy(sourceFile, destinationFile, true); }//Copy the modified file to the backup directory
                                finally { _semaphorefiles.Release(); }
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));// Copy the file that does not exist to the backup directory

                            _semaphorefiles.Wait();
                            try { File.Copy(sourceFile, destinationFile, false); }
                            finally { _semaphorefiles.Release(); }
                        }
                        ModelStateLogs.file_remain = file_remain;
                        ModelLogs.StateLog(ModelStateLogs);
                    }
                    utcDateFinish = DateTime.Now;
                }                
                Duration = utcDateFinish.Subtract(utcDateStart); // Calculation of the result of the arrival date - the departure date to obtain a duration, it's in TimeSpan, it is the result of the subtraction of two DataTime
                millisecondsDuration = Duration.TotalMilliseconds; // Convert Duration in milliseconds
                ModelStateLogs.millisecondsDuration = millisecondsDuration; //add millisecondsDuration to the object ModelStateLogs
                ModelStateLogs.State = "ENDED"; // Uptadte status of the save in order to write it in Json state logs
                ModelLogs.StateLog(ModelStateLogs); // Write the JSon State Logs with all info 
            }
        }

        public void Logs() //Write backup's logs
        {
            string utcDateOnly = utcDateDateTime.ToString("dd/MM/yyyy");
            utcDateOnly = utcDateOnly.Replace("/", "-"); //Format the date to allow serializing
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\logs\JSON\" + utcDateOnly + ".json";
            string fileName2 = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\logs\XML\" + utcDateOnly + ".xml";

            if (File.Exists(fileName))  //Test if log file exists, else it creates it
            {
                string fileContent = null;
                _semaphorelogs.Wait();
                try { fileContent = File.ReadAllText(fileName); } //Bring content of filename in filecontent
                finally { _semaphorelogs.Release(); }

                List<Model_AFT> ?values = new List<Model_AFT>(); //Create the list named values
                values = JsonConvert.DeserializeObject<List<Model_AFT>>(fileContent); //Deserialialize the data in JSON form
                values?.Add(this); //Add object ModelAFT in the list values
                
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(values, Newtonsoft.Json.Formatting.Indented); //Serialialize the data in JSON form
                File.WriteAllText(fileName, jsonString); //Write json file

                XmlSerializer serializer = new XmlSerializer(typeof(List<Model_AFT>));
                
                _semaphorelogs.Wait();
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
        }
    }
}