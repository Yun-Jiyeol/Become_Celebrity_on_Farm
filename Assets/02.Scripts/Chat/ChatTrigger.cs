using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    public ChatManager chatManager;

    public void OnDateChanged()
    {
        chatManager.ResetChatForNewDay();
    }
}
