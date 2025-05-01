using UnityEngine;
using System;

public class Season : MonoBehaviour
{
    public enum SeasonType
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    [Header("���� ���� ���� (�׽�Ʈ��)")]
    public bool overrideSeason = false;
    public SeasonType manualSeason;

    private int currentDay = 0; // TimeManager�� ������ ����
    private readonly string[] seasonNames = { "��", "����", "����", "�ܿ�" };

    private SeasonType lastSeason = SeasonType.Spring;

    public event Action<SeasonType> OnSeasonChanged; // ���� ���� �̺�Ʈ �߰�

    public SeasonType CurrentSeason
    {
        get
        {
            if (overrideSeason)
            {
                return manualSeason;
            }

            int seasonLength = 28;
            int seasonIndex = (currentDay / seasonLength) % 4;
            return (SeasonType)seasonIndex;
        }
    }

    public string CurrentSeasonName => seasonNames[(int)CurrentSeason];

    public void SetCurrentDay(int day)
    {
        currentDay = day;
        UpdateSeason(); // ��¥�� �ٲ�� ���� ������Ʈ ȣ��
    }

    public void SetCurrentSeason(SeasonType season)
    {
        overrideSeason = true;
        manualSeason = season;
        UpdateSeason();
    }

    public void UpdateSeason()
    {
        SeasonType current = CurrentSeason;
        if (current != lastSeason)
        {
            lastSeason = current;
            OnSeasonChanged?.Invoke(current); // ������ �ٲ������ �̺�Ʈ �ߵ�
        }
    }
}
