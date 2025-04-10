using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatData", menuName = "Streaming/ChatData")]
public class ChatData : ScriptableObject
{
    public List<string> user;        // 유저명 리스트
    public List<string> user_Chat;   // 유저 채팅 내용 리스트
}