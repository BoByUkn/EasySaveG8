using System;
using System.IO;
using System.Runtime.InteropServices;

TimeSpan CryptoSoft(string Source, string Destination)
{
    using (var fin = new FileStream(Source, FileMode.Open))
    using (var fout = new FileStream(Destination, FileMode.Create))
    {
        byte[] buffer = new byte[4096]; //lis bit par bit le fichier
        DateTime TimeStart = DateTime.Now;
        while (true)
        {
            int bytesRead = fin.Read(buffer);
            if (bytesRead == 0)
                break;
            EncryptBytes(buffer, bytesRead);
            fout.Write(buffer, 0, bytesRead);
        }
        DateTime TimeEnd = DateTime.Now;
        TimeSpan Duration = TimeStart.Subtract(TimeEnd);

        return Duration;
    }
}

//const ulong Secret = 7540984602987086044; //la clé de chiffrement
//var RandomInt64 = new Random();
//long cipherKey = RandomInt64.NextInt64();

//string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
string filePath = @"C:\Users" + Environment.UserName + @"\AppData\Roaming\EasySave\cipher key.txt";
static long getKey(string filePath)
{
    string inputString;

    // Open the file for reading
    using (StreamReader reader = new StreamReader(filePath))
    {
        // Read the entire contents of the file as a string
        inputString = reader.ReadToEnd();
    }

    // Convert the string to a long
    long key;
    if (!long.TryParse(inputString, out key))
    {
        throw new ArgumentException("The input file contains invalid characters.");
    }

    return key;
}
long cipherKey = getKey(filePath);

//using (StreamWriter writer = new StreamWriter(filePath))

//{
//    writer.Write(key);
//}

void EncryptBytes(byte[] buffer, int count)
{

    for (int i = 0; i < count; i++)
        buffer[i] = (byte)(buffer[i] ^ cipherKey); //opération xor
}


//CryptoSoft("C:\\Users\\theal\\OneDrive\\Bureau\\screen1.png", "C:\\Users\\theal\\OneDrive\\Bureau\\test1\\screen1.png"); //encrypte
//CryptoSoft("C:\\Users\\theal\\OneDrive\\Bureau\\test1\\screen1.png", "C:\\Users\\theal\\OneDrive\\Bureau\\test2\\screen1.png"); //décrypte le fichier encrypté pour tester
