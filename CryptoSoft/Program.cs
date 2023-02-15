using System;
using System.IO;
using System.Runtime.InteropServices;

void EncryptFile(string inputFile, string outputFile, string extension, string path)
{
    using (var fin = new FileStream(inputFile, FileMode.Open))
    using (var fout = new FileStream(outputFile, FileMode.Create))
    {
        byte[] buffer = new byte[4096]; //lis bit par bit le fichier
        while (true)
        {
            int bytesRead = fin.Read(buffer);
            if (bytesRead == 0)
                break;
            EncryptBytes(buffer, bytesRead);
            fout.Write(buffer, 0, bytesRead);
        }
    }
}

//const ulong Secret = 7540984602987086044; //la clé de chiffrement
var RandomInt64 = new Random();
long cipherKey = RandomInt64.NextInt64();

string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
string filePath = Path.Combine(desktopPath, "cipher key.txt");

long key = cipherKey;

using (StreamWriter writer = new StreamWriter(filePath))
{
    writer.Write(key);
}

void EncryptBytes(byte[] buffer, int count)
{

    for (int i = 0; i < count; i++)
        buffer[i] = (byte)(buffer[i] ^ cipherKey); //opération xor
}

EncryptFile("C:\\Users\\theal\\OneDrive\\Bureau\\screen1.png", "C:\\Users\\theal\\OneDrive\\Bureau\\test1\\screen1.png"); //encrypte
EncryptFile("C:\\Users\\theal\\OneDrive\\Bureau\\test1\\screen1.png", "C:\\Users\\theal\\OneDrive\\Bureau\\test2\\screen1.png"); //décrypte le fichier encrypté pour tester