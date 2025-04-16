using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CSVReader
{
    public static List<Dictionary<string, string>> Read(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

        if (csvFile == null)
        {
            Debug.LogError("CSV file not found: " + fileName);
            return dataList;
        }

        StringReader reader = new StringReader(csvFile.text);
        string[] headers = reader.ReadLine().Split(',');

        while (reader.Peek() > -1)
        {
            string[] values = reader.ReadLine().Split(',');
            Dictionary<string, string> row = new Dictionary<string, string>();

            for (int i = 0; i < headers.Length; i++)
            {
                row[headers[i]] = values[i];
            }
            dataList.Add(row);
        }

        return dataList;
    }
}