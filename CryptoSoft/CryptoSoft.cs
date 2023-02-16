using System;
using System.IO;
using System.Runtime.InteropServices;

TimeSpan CryptoSoft(string Source, string Destination)
{
    using (var fin = new FileStream(Source, FileMode.Open))
    using (var fout = new FileStream(Destination, FileMode.Create))
    {
        byte[] buffer = new byte[4096];
        DateTime TimeStart = DateTime.Now; //Get starting time
        while (true)
        {
            int bytesRead = fin.Read(buffer); //Read the file bit by bit
            if (bytesRead == 0)
                break;
            EncryptBytes(buffer, bytesRead); //Encrypt the file bit by bit
            fout.Write(buffer, 0, bytesRead);
        }
        DateTime TimeEnd = DateTime.Now; //Get finish time
        TimeSpan Duration = TimeStart.Subtract(TimeEnd); //Get the duration of the encrypting
        double msDuration = Duration.TotalMilliseconds; //and transform it in milliseconds

        return Duration;
    }
}

static long getKey(string filePath) //Will get the key by reading the file created by EasySave
{
    string inputString;

    using (StreamReader reader = new StreamReader(filePath)) //Open the file for reading
    {
        inputString = reader.ReadToEnd(); //Read the entire contents of the file as a string
    }

    long key;
    if (!long.TryParse(inputString, out key))
    {
        throw new ArgumentException("The input file contains invalid characters.");
    }

    return key;
}
long cipherKey = getKey(@"C:\Users" + Environment.UserName + @"\AppData\Roaming\EasySave\cipher key.txt");

void EncryptBytes(byte[] buffer, int count)
{

    for (int i = 0; i < count; i++)
        buffer[i] = (byte)(buffer[i] ^ cipherKey); //XOR operation bit by bit
}
