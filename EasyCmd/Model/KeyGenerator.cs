using System;
using System.Security.Cryptography;

public class KeyGenerator
{
    // This method generates a 256-bit encryption key using the AES algorithm
    public static string GenerateEncryptionKey()
    {
        // Create a new instance of the AES algorithm
        using (Aes aes = Aes.Create())
        {
            // Set the key size to 256 bits
            aes.KeySize = 256;
            // Generate a new encryption key
            aes.GenerateKey();
            // Convert the key to a Base64 string and return it
            return Convert.ToBase64String(aes.Key);
        }
    }
}

class Program
{
    static void Main()
    {
        // Generate an encryption key using the KeyGenerator class
        string encryptionKey = KeyGenerator.GenerateEncryptionKey();
        // Print the generated encryption key to the console
        Console.WriteLine("Generated Encryption Key: " + encryptionKey);
    }
}