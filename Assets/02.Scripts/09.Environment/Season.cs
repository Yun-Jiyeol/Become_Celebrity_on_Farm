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

    private int currentMonth = 0; // TimeManager�� ����
    private readonly string[] seasonNames = { "��", "����", "����", "�ܿ�" };

    private SeasonType lastSeason = SeasonType.Spring;

    public event Action<SeasonType> OnSeasonChanged; // ���� ���� �̺�Ʈ


    public SeasonType CurrentSeason
    {
        get
        {
            if (overrideSeason)
            {
                return manualSeason;
            }

            // ���� ����� currentMonth ����
            int seasonIndex = currentMonth % 4;
            return (SeasonType)seasonIndex;
        }
    }

    public string CurrentSeasonName => seasonNames[(int)CurrentSeason];

    // TimeManager���� ��(month)�� ������Ʈ�� �� ȣ��
    public void SetCurrentMonth(int month)
    {
        currentMonth = month;
        UpdateSeason();
    }

    // �׽�Ʈ�� ������ ���� ����
    public void SetCurrentSeason(SeasonType season)
    {
        overrideSeason = true;
        manualSeason = season;
        UpdateSeason();
    }

    // ������ �ٲ�� �̺�Ʈ �߻�
    public void UpdateSeason()
    {
        SeasonType current = CurrentSeason;
        if (current != lastSeason)
        {
            lastSeason = current;
            OnSeasonChanged?.Invoke(current);
        }
    }
}
