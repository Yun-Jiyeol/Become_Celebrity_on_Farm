using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatData", menuName = "Streaming/ChatData")]
public class ChatData : ScriptableObject
{
    public List<string> user;        // ������ ����Ʈ
    public List<string> user_Chat;   // ���� ä�� ���� ����Ʈ
}