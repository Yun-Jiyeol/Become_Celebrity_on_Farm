using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Prefabs")]
    public List<GameObject> uiPrefabs;

    [Header("UI Root")]
    public Transform uiRoot; //ĵ���� ���� �г� ��


    private Dictionary<string, UIBase> uiInstances = new Dictionary<string, UIBase>();

    [Header("Ingame UI")]
    public GameObject inventoryUI; // <- �κ��丮 UI ����


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

    public T Show<T>() where T : UIBase //Ÿ�� T�� �ش��ϴ� ui�� ã�Ƽ� ȭ�鿡 ������. ���׸�T �Լ��� uibase�� ����� �ƹ� Ÿ���̳� ���� �� ����
    {
        foreach (var ui in uiInstances.Values)
        {
            if (ui is T)
            {
                ui.Show();
                return ui as T;
            }
        }

        Debug.LogWarning($"[UIManager] {typeof(T)} Ÿ���� UI�� ã�� �� ����.");
        return null;
    }
    public void Hide<T>() where T : UIBase //Ÿ�� T�� �ش��ϴ� ui�� ã�Ƽ� ȭ�鿡�� ����
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

    // �κ��丮 ���� �ݱ� Toggle
    public void ToggleInventoryUI()
    {
        if (inventoryUI == null)
        {
            Debug.LogWarning("InventoryUI�� ������� ����.");
            return;
        }

        bool isActive = inventoryUI.activeSelf;
        inventoryUI.SetActive(!isActive);
    }

    // �κ��丮 �����ִ��� Ȯ�ο�
    public bool InventoryIsOpen()
    {
        return inventoryUI != null && inventoryUI.activeSelf;
    }

    private void Start()
    {
        //�÷��̾� ���� �� ui ����
        var player = FindObjectOfType<PlayerStats>();
        if (player == null)
        {
            Debug.LogError("PlayerStats ��ã��!");
            return;
        }

        var hpBar = UIManager.Instance.Show<UIHpBar>();
        var energyBar = UIManager.Instance.Show<UIEnergyBar>();

        if (hpBar != null) hpBar.Init(player);
        if (energyBar != null) energyBar.Init(player);
    }
}
