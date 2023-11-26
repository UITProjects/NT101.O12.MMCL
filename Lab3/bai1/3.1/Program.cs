using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DZen.Security.Cryptography;
using System.Linq;

class HashCalculator
{
    static void Main()
    {
        Console.WriteLine("Choose an input type:");
        Console.WriteLine("1. Text string");
        Console.WriteLine("2. Hex string");
        Console.WriteLine("3. File");

        int choice;
        if (int.TryParse(Console.ReadLine(), out choice))
        {
            switch (choice)
            {
                case 1:
                    Console.Write("Enter the text string: ");
                    string textString = Console.ReadLine();
                    CalculateAndDisplayHash(textString);
                    break;

                case 2:
                    Console.Write("Enter the hex string: ");
                    string hexString = Console.ReadLine();
                    CalculateAndDisplayHashFromHex(hexString);
                    break;

                case 3:
                    Console.Write("Enter the path to the file: ");
                    string filePath = Console.ReadLine();
                    CalculateAndDisplayFileHash(filePath);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Exiting.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Exiting.");
        }
    }

    static void CalculateAndDisplayHash(string input)
    {
        byte[] data = Encoding.UTF8.GetBytes(input);

        using (MD5 md5 = MD5.Create())
        {
            byte[] md5Hash = md5.ComputeHash(data);
            Console.WriteLine("MD5: " + BitConverter.ToString(md5Hash).Replace("-", ""));
        }

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] sha256Hash = sha256.ComputeHash(data);
            Console.WriteLine("SHA-256: " + BitConverter.ToString(sha256Hash).Replace("-", ""));
        }

        using (SHA3 sha3 = SHA3.Create())
        {
            byte[] sha3Hash = sha3.ComputeHash(data);
            Console.WriteLine("SHA-3: " + BitConverter.ToString(sha3Hash).Replace("-", ""));
        }

        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] sha1Hash = sha1.ComputeHash(data);
            Console.WriteLine("SHA-1: " + BitConverter.ToString(sha1Hash).Replace("-", ""));
        }
    }

    static void CalculateAndDisplayHashFromHex(string hexInput)
    {
        byte[] data = Enumerable.Range(0, hexInput.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hexInput.Substring(x, 2), 16))
            .ToArray();

        CalculateAndDisplayHash(Encoding.UTF8.GetString(data));
    }

    static void CalculateAndDisplayFileHash(string filePath)
    {
        const int bufferSize = 8192; // 8 KB buffer size, adjust as needed

        if (File.Exists(filePath))
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] md5Hash = md5.ComputeHash(fileStream);
                    Console.WriteLine("MD5: " + BitConverter.ToString(md5Hash).Replace("-", ""));
                }

                using (SHA256 sha256 = SHA256.Create())
                {
                    fileStream.Seek(0, SeekOrigin.Begin); // Reset the file stream position
                    byte[] sha256Hash = sha256.ComputeHash(fileStream);
                    Console.WriteLine("SHA-256: " + BitConverter.ToString(sha256Hash).Replace("-", ""));
                }

                using (SHA3 sha3 = SHA3.Create())
                {
                    fileStream.Seek(0, SeekOrigin.Begin); // Reset the file stream position
                    byte[] sha3Hash = sha3.ComputeHash(fileStream);
                    Console.WriteLine("SHA-3: " + BitConverter.ToString(sha3Hash).Replace("-", ""));
                }

                using (SHA1 sha1 = SHA1.Create())
                {
                    fileStream.Seek(0, SeekOrigin.Begin); // Reset the file stream position
                    byte[] sha1Hash = sha1.ComputeHash(fileStream);
                    Console.WriteLine("SHA-1: " + BitConverter.ToString(sha1Hash).Replace("-", ""));
                }
            }
        }
        else
        {
            Console.WriteLine("File not found. Exiting.");
        }
    }
}
