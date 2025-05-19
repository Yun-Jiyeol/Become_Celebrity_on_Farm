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
            bgmSource.loop = true; // ��������� ���� �ݺ� ���
            bgmSource.Play();
        }
        else if (clipToPlay == null)
        {
            Debug.LogWarning("����Ϸ��� BGM Ŭ���� null�Դϴ�.");
        }
    }

    // ���� ������� ���� �Լ�
    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    // ȿ���� ��� �Լ� (���ļ� ��� ����)
    public void PlaySFX(AudioClip clipToPlay)
    {
        if (sfxSource != null && clipToPlay != null)
        {
            sfxSource.PlayOneShot(clipToPlay);
        }
        else if (clipToPlay == null)
        {
            Debug.LogWarning("����Ϸ��� SFX Ŭ���� null�Դϴ�.");
        }
    }
}
