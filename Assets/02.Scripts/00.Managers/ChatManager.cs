using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("CSV 파일 이름")]
    public string userCsvFileName = "user";
    public string chatCsvFileName = "chat";

    [Header("UI")]
    public GameObject chatMessagePrefab;
    public Transform chatContentParent;
    public ScrollRect scrollRect;

    [Header("채팅 설정")]
    public float spawnInterval = 1.5f;
    public string currentDay = "Mon";
    public string currentWeather = "Sunny";

    private UserData userData = new UserData();
    private ChatData chatData = new ChatData();
    private List<ChatEntry> chatPool = new List<ChatEntry>();
    private List<GameObject> todayChatList = new List<GameObject>();

    void Start()
    {
        userData.LoadFromCSV(userCsvFileName);
        chatData.LoadFromCSV(chatCsvFileName);
        LoadChatPool();

        StartCoroutine(SpawnChatRoutine());
    }

    void LoadChatPool()
    {
        chatPool.Clear();

        foreach (var chat in chatData.ChatList)
        {
            bool dayMatch = string.IsNullOrEmpty(chat.DayCondition) || chat.DayCondition == currentDay;
            bool weatherMatch = string.IsNullOrEmpty(chat.WeatherCondition) || chat.WeatherCondition == currentWeather;

            if (dayMatch && weatherMatch)
            {
                chatPool.Add(chat);
            }
        }
    }

    IEnumerator SpawnChatRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomChat();
        }
    }

    void SpawnRandomChat()
    {
        if (userData.Users.Count == 0 || chatPool.Count == 0) return;

        string user = userData.Users[Random.Range(0, userData.Users.Count)];
        ChatEntry chat = chatPool[Random.Range(0, chatPool.Count)];

        GameObject newChat = Instantiate(chatMessagePrefab, chatContentParent);
        TMP_Text userText = newChat.transform.Find("User").GetComponent<TMP_Text>();
        TMP_Text messageText = newChat.transform.Find("User_Chat").GetComponent<TMP_Text>();

        userText.text = user;
        messageText.text = chat.UserChat;
        todayChatList.Add(newChat);

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentParent.GetComponent<RectTransform>());
        StartCoroutine(ScrollToBottomNextFrame());
    }

    IEnumerator ScrollToBottomNextFrame()
    {
        yield return null;
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void OnDateChanged(string newDay, string newWeather)
    {
        currentDay = newDay;
        currentWeather = newWeather;
        ResetChatForNewDay();
    }

    public void ResetChatForNewDay()
    {
        foreach (var chat in todayChatList)
        {
            Destroy(chat);
        }
        todayChatList.Clear();

        LoadChatPool();
    }
}