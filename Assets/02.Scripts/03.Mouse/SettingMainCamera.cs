using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMainCamera : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.camera = Camera.main;
        AudioManager.Instance.PlayBGM(AudioManager.Instance.ReadyAudio["MainBGM"]);
        AudioManager.Instance.bgmSource.volume = 0.1f;
    }
}
