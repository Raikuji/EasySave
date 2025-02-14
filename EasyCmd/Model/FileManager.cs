﻿using System.Diagnostics;
using System.Text;

namespace CryptoSoft;

/// <summary>
/// File manager class
/// This class is used to encrypt and decrypt files
/// </summary>
public class FileManager
{
    private string FilePath { get; }
    private string Key { get; }

    public FileManager(string path, string key)
    {
        FilePath = path;
        Key = key;
    }

    /// <summary>
    /// Check if the file exists
    /// </summary>
    private bool CheckFile()
    {
        if (File.Exists(FilePath))
            return true;

        Console.WriteLine("File not found.");
        Thread.Sleep(1000);
        return false;
    }

    /// <summary>
    /// Encrypts or decrypts the file with XOR encryption
    /// </summary>
    public int TransformFile()
    {
        if (!CheckFile()) return -1;
        Stopwatch stopwatch = Stopwatch.StartNew();
        var fileBytes = File.ReadAllBytes(FilePath);
        var keyBytes = ConvertToByte(Key);
        fileBytes = XorMethod(fileBytes, keyBytes);
        File.WriteAllBytes(FilePath, fileBytes);
        stopwatch.Stop();
        return (int)stopwatch.ElapsedMilliseconds;
    }

    /// <summary>
    /// Convert a string to a byte array
    /// </summary>
    private static byte[] ConvertToByte(string text)
    {
        return Encoding.UTF8.GetBytes(text);
    }

    /// <summary>
    /// XOR encryption method
    /// </summary>
    private static byte[] XorMethod(IReadOnlyList<byte> fileBytes, IReadOnlyList<byte> keyBytes)
    {
        var result = new byte[fileBytes.Count];
        for (var i = 0; i < fileBytes.Count; i++)
        {
            result[i] = (byte)(fileBytes[i] ^ keyBytes[i % keyBytes.Count]);
        }

        return result;
    }
}