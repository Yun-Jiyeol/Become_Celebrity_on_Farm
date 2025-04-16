using System.Collections.Generic;

[System.Serializable]
public class UserData
{
    public List<string> Users = new List<string>();

    public void LoadFromCSV(string fileName)
    {
        var rawData = CSVReader.Read(fileName);
        foreach (var row in rawData)
        {
            if (row.ContainsKey("User"))
            {
                Users.Add(row["User"]);
            }
        }
    }
}