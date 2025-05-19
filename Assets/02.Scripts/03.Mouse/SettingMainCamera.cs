using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMainCamera : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.camera = Camera.main;
    }
}
