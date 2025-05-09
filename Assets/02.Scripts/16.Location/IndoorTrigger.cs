using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IndoorTrigger : MonoBehaviour
{
    public bool isIndoorZone = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // �÷��̾ ������ ���� �۵�
        {
            PlayerLocation.Instance?.SetIndoorState(isIndoorZone);
        }
    }
}
