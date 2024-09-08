using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Threading;

class CsvUdpSender
{
    // Convert a value to its byte representation based on type defined in XML
    static byte[] CastValueToBytes(string value, string type)
    {
        try
        {
            switch (type.ToLower())
            {
                case "int":
                    int intValue = int.Parse(value);
                    return BitConverter.GetBytes(intValue);

                case "float":
                    float floatValue = float.Parse(value);
                    return BitConverter.GetBytes(floatValue);

                case "double":
                    double doubleValue = double.Parse(value);
                    return BitConverter.GetBytes(doubleValue);

                case "byte":
                    byte byteValue = byte.Parse(value);
                    return new byte[] { byteValue };

                case "boolean":
                    bool boolValue = (value == "1" || value.ToLower() == "true");
                    return BitConverter.GetBytes(boolValue);

                case "char":
                    char charValue = value.Length > 0 ? value[0] : ' ';
                    return BitConverter.GetBytes(charValue);

                default:
                    // Treat as a string, convert it to bytes
                    return System.Text.Encoding.UTF8.GetBytes(value);
            }
        }
        catch
        {
            // Return a default value in case of failure
            switch (type.ToLower())
            {
                case "int":
                    return BitConverter.GetBytes(0);
                case "float":
                    return BitConverter.GetBytes(0.0f);
                case "double":
                    return BitConverter.GetBytes(0.0);
                case "byte":
                    return new byte[] { 0 };
                case "boolean":
                    return BitConverter.GetBytes(false);
                case "char":
                    return BitConverter.GetBytes(' ');
                default:
                    return System.Text.Encoding.UTF8.GetBytes(""); // Empty string
            }
        }
    }

    // Read CSV file and return a list of rows (each row is a list of strings)
    static List<List<string>> ReadCsv(string csvFilePath)
    {
        var csvData = new List<List<string>>();
        using (var reader = new StreamReader(csvFilePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',').ToList();
                csvData.Add(values);
            }
        }
        return csvData;
    }

    // Parse XML file to get the data types for each column
    static List<string> ParseXml(string xmlFilePath)
    {
        var dataTypes = new List<string>();
        var xmlDoc = XDocument.Load(xmlFilePath);
        var columns = xmlDoc.Descendants("Column");

        foreach (var column in columns)
        {
            dataTypes.Add(column.Value);
        }
        return dataTypes;
    }

    // Send a row as bytes over UDP
    static void SendRowUdp(byte[] data, string destinationIp, int destinationPort)
    {
        using (var udpClient = new UdpClient())
        {
            udpClient.Send(data, data.Length, destinationIp, destinationPort);
        }
    }

    static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: csCastSendcsv <csv file> <destination IP> <destination port>");
            return;
        }

        string csvFilePath = args[0];
        string destinationIp = args[1];
        int destinationPort = int.Parse(args[2]);
        int intervalMs = 10;

        // Step 1: Read the CSV file
        var csvData = ReadCsv(csvFilePath);
        if (csvData.Count == 0)
        {
            Console.WriteLine("Error: CSV file is empty or could not be read.");
            return;
        }

        // Step 2: Parse the XML file to get data types for each column
        var dataTypes = ParseXml("CastType.xml");
        if (dataTypes.Count == 0)
        {
            Console.WriteLine("Error: Could not read data types from XML.");
            return;
        }

        // Step 3: Cast each value to bytes and send each row over UDP
        foreach (var row in csvData)
        {
            var byteList = new List<byte>();

            for (int i = 0; i < row.Count; i++)
            {
                byte[] castedBytes = CastValueToBytes(row[i], i < dataTypes.Count ? dataTypes[i] : "string");
                byteList.AddRange(castedBytes); // Add casted bytes to the byte list
            }

            // Send the byte array via UDP
            SendRowUdp(byteList.ToArray(), destinationIp, destinationPort);
            Thread.Sleep(intervalMs); // Sleep for the given interval
        }

        Console.WriteLine("Data sent successfully.");
    }
}
