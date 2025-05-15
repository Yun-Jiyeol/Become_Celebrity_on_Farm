using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;

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
        if(audioClips != null)
        {
            SettingDictionary();
        }
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
}
