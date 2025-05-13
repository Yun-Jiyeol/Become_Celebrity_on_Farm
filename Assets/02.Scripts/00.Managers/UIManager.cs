using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Prefabs")]
    public List<GameObject> uiPrefabs;

    [Header("UI Root")]
    public Transform uiRoot; //캔버스 밑의 패널 등


    private Dictionary<string, UIBase> uiInstances = new Dictionary<string, UIBase>();

    [Header("Ingame UI")]
    public GameObject inventoryUI; // <- 인벤토리 UI 연결

    [Header("Ingame UI")]
    public UIHpBar hpBar;
    public UIEnergyBar energyBar;

    private void Start()
    {
        var player = FindObjectOfType<PlayerStats>();
        if (player == null)
        {
            Debug.LogError("[UIManager] PlayerStats 못 찾음!");
            return;
        }

        if (hpBar != null)
        {
            hpBar.Init(player);
            Debug.Log("[UIManager] HpBar Init 완료");
        }
        else
        {
            Debug.LogError("[UIManager] HpBar 못 찾음");
        }

        if (energyBar != null)
        {
            energyBar.Init(player);
            Debug.Log("[UIManager] EnergyBar Init 완료");
        }
        else
        {
            Debug.LogError("[UIManager] EnergyBar 못 찾음");
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("다수의 UIManager 존재!");
            Destroy(gameObject);
            return;
        }

        // 미리 UI를 전부 생성
        foreach (GameObject prefab in uiPrefabs)
        {
            GameObject go = Instantiate(prefab, uiRoot);
            RectTransform rect = go.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero; // UI의 위치를 캔버스 중앙으로 설정
                rect.localScale = Vector3.one; // UI의 크기를 원래대로 설정
            }

            go.SetActive(false);

            UIBase ui = go.GetComponent<UIBase>();
            if (ui != null)
            {
                uiInstances.Add(prefab.name, ui);
            }
        }
    }

    public T Show<T>() where T : UIBase //타입 T에 해당하는 ui를 찾아서 화면에 보여줌. 제네릭T 함수는 uibase를 상속한 아무 타입이나 받을 수 있음
    {
        foreach (var ui in uiInstances.Values)
        {
            if (ui is T)
            {
                ui.Show();
                return ui as T;
            }
        }

        Debug.LogWarning($"[UIManager] {typeof(T)} 타입의 UI를 찾을 수 없음.");
        return null;
    }
    public void Hide<T>() where T : UIBase //타입 T에 해당하는 ui를 찾아서 화면에서 숨김
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

    // 인벤토리 열고 닫기 Toggle
    public void ToggleInventoryUI()
    {
        if (inventoryUI == null)
        {
            Debug.LogWarning("InventoryUI가 연결되지 않음.");
            return;
        }

        bool isActive = inventoryUI.activeSelf;
        inventoryUI.SetActive(!isActive);

        if (isActive)
            TooltipManager.Instance?.HideTooltip();
            InventoryUIManager.Instance?.ForceReturnHoldingItem();
    }

    // 인벤토리 열려있는지 확인용
    public bool InventoryIsOpen()
    {
        return inventoryUI != null && inventoryUI.activeSelf;
    }
}