using System.Collections.Generic;

[System.Serializable]
public class ChatEntry
{
    public string UserChat;
    public string DayCondition;
    public string WeatherCondition;

    public ChatEntry(string chat, string day, string weather)
    {
        UserChat = chat;
        DayCondition = day;
        WeatherCondition = weather;
    }
}

[System.Serializable]
public class ChatData
{
    public List<ChatEntry> ChatList = new List<ChatEntry>();

    public void LoadFromCSV(string fileName)
    {
        var rawData = CSVReader.Read(fileName);
        foreach (var row in rawData)
        {
            if (row.ContainsKey("User_Chat"))
            {
                string chat = row["User_Chat"];
                string day = row.ContainsKey("Day_Condition") ? row["Day_Condition"] : "";
                string weather = row.ContainsKey("Weather_Condition") ? row["Weather_Condition"] : "";
                ChatList.Add(new ChatEntry(chat, day, weather));
            }
        }
    }
}