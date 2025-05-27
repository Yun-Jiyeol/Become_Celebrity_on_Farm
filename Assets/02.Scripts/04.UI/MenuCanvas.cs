using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    public Slider MasterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        audioManager.MenuCanvas = gameObject;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        MasterSlider.value = audioManager.SaveVolumeMain;
        BGMSlider.value = audioManager.SaveVolumeBGM;
        SFXSlider.value = audioManager.SaveVolumeSFX;
    }
}
