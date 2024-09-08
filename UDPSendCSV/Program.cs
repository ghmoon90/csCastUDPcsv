using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 3 || args.Length > 4)
        {
            Console.WriteLine("Usage: Program <csvFilePath> <destinationIP> <destinationPort> [iterations]");
            return;
        }

        string csvFilePath = args[0];     // Source CSV file path
        string udpAddress = args[1];      // Destination IP address
        int udpPort = int.Parse(args[2]); // Destination port

        // Default iterations to 1 if not provided
        int iterations = args.Length == 4 ? int.Parse(args[3]) : 1;

        int intervalMs = 10; // Interval in milliseconds (10ms)

        SendCsvOverUdp(csvFilePath, udpAddress, udpPort, iterations, intervalMs);
    }

    static void SendCsvOverUdp(string csvFilePath, string udpAddress, int udpPort, int iterations, int intervalMs)
    {
        UdpClient udpClient = new UdpClient();
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(udpAddress), udpPort);

        string[][] csvData = ReadCsv(csvFilePath);

        for (int iter = 0; iter < iterations; iter++)
        {
            foreach (var row in csvData)
            {
                byte[] byteArray = ConvertRowToBytes(row);
                udpClient.Send(byteArray, byteArray.Length, endPoint);
                Thread.Sleep(intervalMs); // Sleep for the given interval
            }
        }

        udpClient.Close();
        Console.WriteLine("Completed sending data.");
    }

    static string[][] ReadCsv(string csvFilePath)
    {
        // Reads CSV and returns 2D string array where each row is an array of strings
        return File.ReadAllLines(csvFilePath)
                    .Select(line => line.Split(','))
                    .ToArray();
    }

    static byte[] ConvertRowToBytes(string[] row)
    {
        // Converts each element in the row to a byte (from decimal 10-digit format)
        return row.Select(digit => ConvertToByte(digit)).ToArray();
    }

    static byte ConvertToByte(string digitStr)
    {
        // Convert the string representing a decimal number (0-255) into a byte
        int digit = int.Parse(digitStr);
        if (digit < 0 || digit > 255)
        {
            throw new ArgumentOutOfRangeException($"Value {digit} is out of byte range (0-255).");
        }
        return (byte)digit;
    }
}
