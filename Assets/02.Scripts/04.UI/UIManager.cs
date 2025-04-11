using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Prefabs")]
    public List<GameObject> uiPrefabs;

    [Header("UO Root")]
    public Transform uiRoot; //ĵ���� ���� �г� ��

    private Dictionary<string, UIBase> uiInstances = new Dictionary<string, UIBase>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("�ټ��� UIManager ����!");
            Destroy(gameObject);
            return;
        }

        // �̸� UI�� ���� ����
        foreach (GameObject prefab in uiPrefabs)
        {
            GameObject go = Instantiate(prefab, uiRoot);
            go.SetActive(false);
            UIBase ui = go.GetComponent<UIBase>();
            if (ui != null)
            {
                uiInstances.Add(prefab.name, ui);
            }
        }
    }
    public T Show<T>() where T : UIBase
    {
        foreach (var ui in uiInstances.Values)
        {
            if (ui is T)
            {
                ui.Show();
                return ui as T;
            }
        }

        Debug.LogWarning($"UI of type {typeof(T)} not found.");
        return null;
    }
    public void Hide<T>() where T : UIBase
    {
        foreach (var ui in uiInstances.Values)
        {
            if (ui is T)
            {
                ui.Hide();
                return;
            }
        }
    }

    //public void ShowCharacterChoiceUI()
    //{
    //    GameObject ui = Instantiate(characterChoiceUIPrefab, uiRoot);
    //    CharacterChoice choice = ui.GetComponent<CharacterChoice>();
    //    choice.Setup();
    //}
    
}
