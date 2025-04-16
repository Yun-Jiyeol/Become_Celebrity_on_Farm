using UnityEngine;

public class Season : MonoBehaviour
{
    public enum SeasonType
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    private int currentDay = 0; // TimeManager�� ������ ����
    private readonly string[] seasonNames = { "��", "����", "����", "�ܿ�" };

    public SeasonType CurrentSeason
    {
        get
        {
<<<<<<< Updated upstream
            int seasonLength = 28; 
=======
            int seasonLength = 28; // �� ������ 30��
>>>>>>> Stashed changes
            int seasonIndex = (currentDay / seasonLength) % 4;
            return (SeasonType)seasonIndex;
        }
    }

    public string CurrentSeasonName => seasonNames[(int)CurrentSeason];

    public void SetCurrentDay(int day)
    {
        currentDay = day;
    }

    public void UpdateSeason()
    {
        // ���� ������Ʈ�� ȣ���� �� ����. (�ʿ�� �߰� ���� ����)
    }
}