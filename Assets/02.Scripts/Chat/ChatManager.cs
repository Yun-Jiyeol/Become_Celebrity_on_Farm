using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("데이터")]
    public ChatData chatData;

    [Header("UI")]
    public GameObject chatMessagePrefab;
    public Transform chatContentParent;
    public ScrollRect scrollRect;

    [Header("채팅 설정")]
    public float spawnInterval = 1.5f;

    private List<GameObject> todayChatList = new List<GameObject>();
    private List<string> tempChatMessages;

    void Start()
    {
        // 메시지 복사본 만들어서 런타임에서만 사용
        tempChatMessages = new List<string>(chatData.user_Chat);
        StartCoroutine(SpawnChatRoutine());
    }

    // 채팅 생성 루틴
    IEnumerator SpawnChatRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomChat();
        }
    }

    // 채팅 메시지 추가 후 스크롤을 아래로 내리는 함수
    IEnumerator ScrollToBottomNextFrame()
    {
        yield return null; // 한 프레임 대기
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentParent.GetComponent<RectTransform>());
        scrollRect.verticalNormalizedPosition = 0f;  // 스크롤을 가장 아래로
    }

    // 랜덤한 채팅 메시지 생성
    void SpawnRandomChat()
    {
        if (chatData.user.Count == 0 || tempChatMessages.Count == 0) return;

        string randomUser = chatData.user[Random.Range(0, chatData.user.Count)];
        int randomIndex = Random.Range(0, tempChatMessages.Count);
        string randomMessage = tempChatMessages[randomIndex];
        tempChatMessages.RemoveAt(randomIndex); // 복사본에서만 제거!

        // 새 채팅 메시지 객체 생성
        GameObject newChat = Instantiate(chatMessagePrefab, chatContentParent);
        TMP_Text userText = newChat.transform.Find("User").GetComponent<TMP_Text>();
        TMP_Text messageText = newChat.transform.Find("User_Chat").GetComponent<TMP_Text>();

        userText.text = randomUser;
        messageText.text = randomMessage;

        todayChatList.Add(newChat);

        // Layout 갱신 강제
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentParent.GetComponent<RectTransform>());

        // 채팅 목록 스크롤을 아래로
        StartCoroutine(ScrollToBottomNextFrame());
    }

    // 새로운 날을 시작할 때 채팅 초기화
    public void ResetChatForNewDay()
    {
        foreach (var chat in todayChatList)
        {
            Destroy(chat);
        }
        todayChatList.Clear();
    }
}