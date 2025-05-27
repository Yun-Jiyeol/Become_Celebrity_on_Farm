using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;

    public GameObject MenuCanvas;

    public AudioMixer mainAudioMixer;
    string masterVolumeParameterName = "Master";
    string bgmVolumeParameterName = "BGM";
    string sfxVolumeParameterName = "SFX";

    public float SaveVolumeMain = 0;
    public float SaveVolumeBGM = 0;
    public float SaveVolumeSFX = 0;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip[] audioClips;
    public Dictionary<string, AudioClip> ReadyAudio = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (audioClips != null)
        {
            SettingDictionary();
        }
    }
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {
        SetMainVolume(1);
        SetBGMVolume(0.5f);
        SetSFXVolume(0.5f);
    }

    private void SettingDictionary()
    {
        ReadyAudio.Clear();

        foreach (AudioClip audio in audioClips)
        {
            ReadyAudio.Add(audio.name, audio);
        }
    }

    public void PlayBGM(AudioClip clipToPlay)
    {
        if (bgmSource != null && clipToPlay != null)
        {
            bgmSource.clip = clipToPlay;
            bgmSource.loop = true; // 배경음악은 보통 반복 재생
            bgmSource.Play();
        }
        else if (clipToPlay == null)
        {
            Debug.LogWarning("재생하려는 BGM 클립이 null입니다.");
        }
    }

    // 현재 배경음악 중지 함수
    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    // 효과음 재생 함수 (겹쳐서 재생 가능)
    public void PlaySFX(AudioClip clipToPlay)
    {
        if (sfxSource != null && clipToPlay != null)
        {
            sfxSource.PlayOneShot(clipToPlay);
        }
        else if (clipToPlay == null)
        {
            Debug.LogWarning("재생하려는 SFX 클립이 null입니다.");
        }
    }

    public void SetMainVolume(float _input)
    {
        SaveVolumeMain = _input;
        float dbVolume = (_input > 0) ? Mathf.Log10(_input) * 30 + 10 : -80;
        mainAudioMixer.SetFloat(masterVolumeParameterName, dbVolume);
    }

    public void SetBGMVolume(float _input)
    {
        SaveVolumeBGM = _input;
        float dbVolume = (_input > 0) ? Mathf.Log10(_input) * 30 + 10 : -80;
        mainAudioMixer.SetFloat(bgmVolumeParameterName, dbVolume);
    }

    public void SetSFXVolume(float _input)
    {
        SaveVolumeSFX = _input;
        float dbVolume = (_input > 0) ? Mathf.Log10(_input) * 30 + 10 : -80;
        mainAudioMixer.SetFloat(sfxVolumeParameterName, dbVolume);
    }

    public void OnMenuBtn()
    {
        MenuCanvas.SetActive(true);
    }
    public void OffMenuBtn()
    {
        MenuCanvas.SetActive(false);
    }
}
