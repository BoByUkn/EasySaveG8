using EasySave_G8_UI.View_Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Threading;
using System.Windows.Forms.Design;

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
        

        private static SemaphoreSlim _semaphorejson = new SemaphoreSlim(1);
        private static SemaphoreSlim _semaphorexml = new SemaphoreSlim(1);

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
            ModelLogs = new Model_Logs(); 
        }

        public void Run() //Run a backup
        {
            {
                Model_StateLogs ModelStateLogs = new Model_StateLogs(this.Name, this.Source, this.Destination, this.Type, this.total_files);

                Model_PRIORITY model_PRIORITY = new Model_PRIORITY(); // create a new Model_Priority in order to have a priority list
                List<String> priorityList =model_PRIORITY.priorityReturn();

                if (File.Exists(Source)) //If it's a file
                {
                    utcDateStart = DateTime.Now; //Initialize the date

                    File.Copy(Source, Destination, true); //Run the save

                    //string appPath = @"C:\Users\" + Environment.UserName + @"\source\repos\Easy Save G8 UI\EasySave_G8_UI\Models\CryptoSoft\cryptosoft.exe";
                    //string destination = Destination + @"\" + Path.GetFileName(Source) + @"\" + Path.GetFileName(Source); //Combine paths to correctlty create the directory
                    //DateTime TimeStartCS = DateTime.Now; //Get starting time
                    //Process appProcess = new Process(); //Create the process
                    //appProcess.StartInfo.FileName = appPath; //Starting CryptoSoft
                    //appProcess.StartInfo.Arguments = destination; //Pass the argument
                    //appProcess.Start(); //Start the process
                    //appProcess.WaitForExit();  //Wait for the app to complete
                    //appProcess.Close();  //Close the process

                    DateTime TimeEndCS = DateTime.Now; //Get finish time
                    TimeSpan DurationCS = TimeStartCS.Subtract(TimeEndCS); //Get the duration of the encrypting
                    double msDurationCS = DurationCS.TotalMilliseconds; //and transform it in milliseconds

                    Size = new System.IO.FileInfo(Source).Length;
                    utcDateFinish = DateTime.Now;
                }
                else if (Directory.Exists(Source)) //If it's a folder
                {
                    utcDateStart = DateTime.Now;
                    var files = Directory.GetFiles(Source, "*.*", SearchOption.AllDirectories); //Get folders and files in the source directory
                   
                    List<string> files_NoPriority = new List<string>(files); // Convert Tab in List (in order to use Remove Method)
                    List<string> files_Priority = new List<string>(); //Create list of priorityfiles

                    string Destination2;
                    Destination2 = Destination + @"\" + Path.GetFileName(Source); //Combine the destination directory with the file name of the source 

                    Directory.CreateDirectory(Destination); //Create the destination directory if it doesn't exist
                    Directory.CreateDirectory(Destination2); //Create the destination directory                    
                    
                    file_remain = total_files;

                    foreach (var file in files) //Loop throught every files and add them to Pirority List
                    {
                        Size = Size + new FileInfo(file).Length; //Increment size with each file
                        string targetFile = file.Replace(Source, Destination2);

                        foreach (string ext in priorityList)
                        {
                            if (Path.GetExtension(file) == ext)
                            {
                                files_Priority.Add(file); // Add file in priority file
                                files_NoPriority.Remove(file); // Remove file from the all files in the list files_NoPriority (in order to have only no priority files)
                            }
                        }
                    }

                    foreach(var file in files_Priority)
                    {
                        string targetFile = file.Replace(Source, Destination2);
                        File.Copy(file, targetFile, true);  // Do the copy of priority Files

                        ActualSize2 = ActualSize2 + new FileInfo(file).Length;//Increment size with each file
                        int percentage = (int)(((double)ActualSize2 / (double)Size) * 100);//progression's percentage of the save
                        file_remain = file_remain - 1; // File remain decrease when a file copy have been done

                        // string appPath = @"C:\Users\" + Environment.UserName + @"\source\repos\Easy Save G8 UI\EasySave_G8_UI\Models\CryptoSoft\cryptosoft.exe";
                        // string destination = Destination + @"\" + Path.GetFileName(Source) + @"\" + Path.GetFileName(file); //Combine paths to correctlty create the directory
                        // DateTime TimeStartCS = DateTime.Now; //Get starting time
                        // Process appProcess = new Process(); //Create the process
                        // appProcess.StartInfo.FileName = appPath; //Starting CryptoSoft
                        // appProcess.StartInfo.Arguments = destination; //Pass the argument
                        // appProcess.Start(); //Start the process
                        //appProcess.WaitForExit();  //Wait for the app to complete
                        //appProcess.Close();  //Close the process

                        ModelStateLogs.progression = percentage; // actualize the progression attribute on ModelStateLogs with actual percentage
                        ModelStateLogs.file_remain = file_remain; // actualize the file remain attribute on ModelStateLogs with actual percentage
                        ModelLogs.StateLog(ModelStateLogs); // Write the json state logs with new infos (it changes at each iteration)
                    }

                    ModelStateLogs.Size = Size;
                    foreach (var file in files_NoPriority) //Loop throught every files and copy them
                    {
                        string targetFile = file.Replace(Source, Destination2);
                        File.Copy(file, targetFile, true);  // Do the copy

                        ActualSize2 = ActualSize2 + new FileInfo(file).Length;//Increment size with each file
                        int percentage = (int)(((double)ActualSize2 / (double)Size) * 100);//progression's percentage of the save
                        file_remain = file_remain - 1; // File remain decrease when a file copy have been done
                        Directory.CreateDirectory(Path.GetDirectoryName(targetFile)); // Create a directory
                        File.Copy(file, targetFile, true);  // Do the copy
                        file_remain = file_remain - 1; // File remain decrease when a file copy have been done

                        //string appPath = @"C:\Users\" + Environment.UserName + @"\source\repos\Easy Save G8 UI\EasySave_G8_UI\Models\CryptoSoft\cryptosoft.exe";
                        //string destination = Destination + @"\" + Path.GetFileName(Source)+ @"\"+ Path.GetFileName(file); //Combine paths to correctlty create the directory
                        //DateTime TimeStartCS = DateTime.Now; //Get starting time
                        //Process appProcess = new Process(); //Create the process
                        //appProcess.StartInfo.FileName = appPath; //Starting CryptoSoft
                        //appProcess.StartInfo.Arguments = destination; //Pass the argument
                        //appProcess.Start(); //Start the process
                        //appProcess.WaitForExit();  //Wait for the app to complete
                        //appProcess.Close();  //Close the process

                        DateTime TimeEndCS = DateTime.Now; //Get finish time
                        TimeSpan DurationCS = TimeStartCS.Subtract(TimeEndCS); //Get the duration of the encrypting
                        double msDurationCS = DurationCS.TotalMilliseconds; //and transform it in milliseconds

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

                Model_PRIORITY model_PRIORITY = new Model_PRIORITY();
                List<String> priorityList = model_PRIORITY.priorityReturn();
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
                        string targetFile = file.Replace(Source, Destination2);

                        foreach (string ext in priorityList)
                        {
                            if (Path.GetExtension(file) == ext)
                            {
                                File.Copy(file, targetFile, true);  // Do the copy when the ext is equal tu the extension of the actual file
                            }
                        }
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

                                string appPath = @"C:\Users\" + Environment.UserName + @"\source\repos\Easy Save G8 UI\EasySave_G8_UI\Models\CryptoSoft\cryptosoft.exe";
                                string destination = Destination + @"\" + Path.GetFileName(Source) + @"\" + Path.GetFileName(sourceFile); //Combine paths to correctlty create the directory
                                DateTime TimeStartCS = DateTime.Now; //Get starting time
                                Process appProcess = new Process(); //Create the process
                                appProcess.StartInfo.FileName = appPath; //Starting CryptoSoft
                                appProcess.StartInfo.Arguments = destination; //Pass the argument
                                appProcess.Start(); //Start the process
                                appProcess.WaitForExit();  //Wait for the app to complete
                                appProcess.Close();  //Close the process

                                DateTime TimeEndCS = DateTime.Now; //Get finish time
                                TimeSpan DurationCS = TimeStartCS.Subtract(TimeEndCS); //Get the duration of the encrypting
                                double msDurationCS = DurationCS.TotalMilliseconds; //and transform it in milliseconds
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));// Copy the file that does not exist to the backup directory
                            File.Copy(sourceFile, destinationFile, false);

                            string appPath = @"C:\Users\" + Environment.UserName + @"\source\repos\Easy Save G8 UI\EasySave_G8_UI\Models\CryptoSoft\cryptosoft.exe";
                            string destination = Destination + @"\" + Path.GetFileName(Source) + @"\" + Path.GetFileName(sourceFile); //Combine paths to correctlty create the directory
                            DateTime TimeStartCS = DateTime.Now; //Get starting time
                            Process appProcess = new Process(); //Create the process
                            appProcess.StartInfo.FileName = appPath; //Starting CryptoSoft
                            appProcess.StartInfo.Arguments = destination; //Pass the argument
                            appProcess.Start(); //Start the process
                            appProcess.WaitForExit();  //Wait for the app to complete
                            appProcess.Close();  //Close the process

                            DateTime TimeEndCS = DateTime.Now; //Get finish time
                            TimeSpan DurationCS = TimeStartCS.Subtract(TimeEndCS); //Get the duration of the encrypting
                            double msDurationCS = DurationCS.TotalMilliseconds; //and transform it in milliseconds
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

                _semaphorejson.Wait();
                try { fileContent = File.ReadAllText(fileName); } //Bring content of filename in filecontent
                finally { _semaphorejson.Release(); }

                List<Model_AFT> ?values = new List<Model_AFT>(); //Create the list named values
                values = JsonConvert.DeserializeObject<List<Model_AFT>>(fileContent); //Deserialialize the data in JSON form
                values?.Add(this); //Add object ModelAFT in the list values
                
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(values, Newtonsoft.Json.Formatting.Indented); //Serialialize the data in JSON form
                
                _semaphorejson.Wait();
                try { File.WriteAllText(fileName, jsonString); } //Write json file
                finally { _semaphorejson.Release(); }

                XmlSerializer serializer = new XmlSerializer(typeof(List<Model_AFT>));
                _semaphorexml.Wait();
                try {
                    StreamWriter writer = new StreamWriter(fileName2);
                    serializer.Serialize(writer, values);
                    writer.Close();
                }
                finally { _semaphorexml.Release(); }
            }


            else if (!File.Exists(fileName))
            {
                List<Model_AFT> values = new List<Model_AFT>(); // create the list named values
                values.Add(this);// Add object ModelAFT in the list values
                var jsonString = JsonConvert.SerializeObject(values, Newtonsoft.Json.Formatting.Indented); //Serialialize the data in JSON form
                
                _semaphorejson.Wait();
                try { File.WriteAllText(fileName, jsonString); } // Write json file
                finally { _semaphorejson.Release(); }

                XmlSerializer serializer = new XmlSerializer(typeof(List<Model_AFT>));
                
                _semaphorexml.Wait();
                try
                {
                    StreamWriter writer = new StreamWriter(fileName2);
                    serializer.Serialize(writer, values);
                    writer.Close();
                }
                finally { _semaphorexml.Release(); }
            }
        }

    }
}