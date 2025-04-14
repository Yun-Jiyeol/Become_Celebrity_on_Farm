using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("������")]
    public ChatData chatData;

    [Header("UI")]
    public GameObject chatMessagePrefab;
    public Transform chatContentParent;
    public ScrollRect scrollRect;

    [Header("ä�� ����")]
    public float spawnInterval = 1.5f;

    private List<GameObject> todayChatList = new List<GameObject>();
    private List<string> tempChatMessages;

    void Start()
    {
        // �޽��� ���纻 ���� ��Ÿ�ӿ����� ���
        tempChatMessages = new List<string>(chatData.user_Chat);
        StartCoroutine(SpawnChatRoutine());
    }

    // ä�� ���� ��ƾ
    IEnumerator SpawnChatRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomChat();
        }
    }

    // ä�� �޽��� �߰� �� ��ũ���� �Ʒ��� ������ �Լ�
    IEnumerator ScrollToBottomNextFrame()
    {
        yield return null; // �� ������ ���
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentParent.GetComponent<RectTransform>());
        scrollRect.verticalNormalizedPosition = 0f;  // ��ũ���� ���� �Ʒ���
    }

    // ������ ä�� �޽��� ����
    void SpawnRandomChat()
    {
        if (chatData.user.Count == 0 || tempChatMessages.Count == 0) return;

        string randomUser = chatData.user[Random.Range(0, chatData.user.Count)];
        int randomIndex = Random.Range(0, tempChatMessages.Count);
        string randomMessage = tempChatMessages[randomIndex];
        tempChatMessages.RemoveAt(randomIndex); // ���纻������ ����!

        // �� ä�� �޽��� ��ü ����
        GameObject newChat = Instantiate(chatMessagePrefab, chatContentParent);
        TMP_Text userText = newChat.transform.Find("User").GetComponent<TMP_Text>();
        TMP_Text messageText = newChat.transform.Find("User_Chat").GetComponent<TMP_Text>();

        userText.text = randomUser;
        messageText.text = randomMessage;

        todayChatList.Add(newChat);

        // Layout ���� ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentParent.GetComponent<RectTransform>());

        // ä�� ��� ��ũ���� �Ʒ���
        StartCoroutine(ScrollToBottomNextFrame());
    }

    // ���ο� ���� ������ �� ä�� �ʱ�ȭ
    public void ResetChatForNewDay()
    {
        foreach (var chat in todayChatList)
        {
            Destroy(chat);
        }
        todayChatList.Clear();
    }
}