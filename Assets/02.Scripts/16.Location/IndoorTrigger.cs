using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IndoorTrigger : MonoBehaviour
{
    public bool isIndoorZone = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // 플레이어가 들어왔을 때만 작동
        {
            PlayerLocation.Instance?.SetIndoorState(isIndoorZone);
        }
    }
}
