using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum PlayerInteractType
{
    Point,
    Charge,
    Range
}

public class PlayerController : BaseController
{
    private PlayerAnimation playerAnimation;
    Vector3 tartgetPosition = new Vector3();
    public bool isAction = false;
    public bool isClick = false;
    public bool isNPCInteract = false;

    private int nownum;
    int DirectionSave = 0;

    [Header("Interact")]
    private bool CanSpawn = false;
    ChangedGround Groundtype = ChangedGround.Plow;
    PlayerInteractType nowInteractType;
    ItemType chooseItemType;
    GameObject PlayerInteractRange;
    GameObject MouseInteract;
    GameObject FishingGauge;
    public float ItemDamage = 0;
    int LastItemNum = 0;

    private RangeInteract readyRangeInteract = new RangeInteract();
    public class RangeInteract
    {
        public string[] _Tag;
        public int _Dir;
        public bool _isAll;
    }


    protected override void Start()
    {
        base.Start();

        playerAnimation = GetComponent<PlayerAnimation>();

        PlayerInteractRange = Instantiate(GameManager.Instance.PlayerRange);
        PlayerInteractRange.SetActive(false);
        MouseInteract = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower;
        FishingGauge = Instantiate(GameManager.Instance.FishingGauge);
        FishingGauge.SetActive(false);
        Invoke("lateStart", 0.1f);
    }

    void lateStart()
    {
        ChangeSlot(1);
    }

    protected override void FixedUpdate()
    {
        if (isAction) return;
        if (isNPCInteract) return;

        base.FixedUpdate();
    }
    void OnInteractNPC()
    {
        GameObject NPC = GetComponent<Player>().autoGetItem.ClosestNPC;

        if (NPC != null && !isNPCInteract)
        {
            dir = Vector2.zero;
            isNPCInteract = true;
            NPC.GetComponent<IInteract>().Interact();
        }
    }

    void OnMove(InputValue inputValue)
    {
        if (UIManager.Instance.InventoryIsOpen()) return;
        if (isAction == true) return;
        if (isNPCInteract) return;

        dir = inputValue.Get<Vector2>();
    }

    void OnClick(InputValue inputValue)
    {
        if (UIManager.Instance.InventoryIsOpen()) return;
        if (isAction) return;
        if (isNPCInteract) return;

        if (inputValue.isPressed)
        {
            isClick = true;
            GameManager.Instance.TurnOnAllColliders();
            Invoke("UseSomeThing", 0.1f);
        }
        else if (!inputValue.isPressed)
        {
            isClick = false;
        }
    }

    void UseSomeThing()
    {
        if (nowInteractType == PlayerInteractType.Point)
        {
            switch (chooseItemType)
            {
                case ItemType.Hoe:
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;

                    CheckAngle();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetFloat(gameObject.GetComponent<Player>().playerAnimation.SpeedParameterHash,
                        ItemManager.Instance.itemDataReader.itemsDatas[gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num].Speed);
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.HoeParameterHash);

                    if (GameManager.Instance.TagIsInMouse(new string[] { "Farmable" }))
                    {
                        if (GameManager.Instance.TagIsNotInMouse(new string[] { "Plow", "Tree", "Stone", "EndGrow" }))
                        {
                            CanSpawn = true;
                            Groundtype = ChangedGround.Plow;
                        }
                    }
                        
                    break;
                case ItemType.Watering:
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;

                    CheckAngle();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetFloat(gameObject.GetComponent<Player>().playerAnimation.SpeedParameterHash,
                        ItemManager.Instance.itemDataReader.itemsDatas[gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num].Speed);
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.WateringParameterHash);

                    if (GameManager.Instance.TagIsInMouse(new string[] { "Plow" }))
                    {
                        if (GameManager.Instance.TagIsNotInMouse(new string[] { "Watered" }))
                        {
                            CanSpawn = true;
                            Groundtype = ChangedGround.Watered;
                        }
                    }
                    break;
                case ItemType.Seed:
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;

                    if (GameManager.Instance.TagIsInMouse(new string[] { "Plow" }))
                    {
                        if (GameManager.Instance.TagIsNotInMouse(new string[] { "Seeded", "EndGrow" }))
                        {
                            GameObject ConnectedObejct = ItemManager.Instance.connectItem.FindObject(gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num);
                            if (ConnectedObejct != null)
                            {
                                GameObject go = Instantiate(ConnectedObejct);
                                go.transform.parent = GameManager.Instance.transform;
                                go.transform.position = tartgetPosition;
                            }
                            UseItemOnHand(1);
                        }
                    }
                    TryHandInteract();
                    break;
                case ItemType.TreeSeed:
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;

                    if (GameManager.Instance.TagIsInMouse(new string[] { "Farmable" }))
                    {
                        if (GameManager.Instance.TagIsNotInMouse(new string[] { "Plow", "Tree", "EndGrow", "Stone" , "Interactable"}))
                        {
                            GameObject ConnectedObejct = ItemManager.Instance.connectItem.FindObject(gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num);
                            if (ConnectedObejct != null)
                            {
                                GameObject go = Instantiate(ConnectedObejct);
                                go.transform.parent = GameManager.Instance.transform;
                                go.transform.position = tartgetPosition;
                            }
                            UseItemOnHand(1);
                        }
                    }
                    TryHandInteract();
                    break;
                case ItemType.Interia:
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;
                    tartgetPosition = gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.transform.position;

                    if (GameManager.Instance.TagIsInMouse(new string[] { "Farmable" }))
                    {
                        if (GameManager.Instance.TagIsNotInMouse(new string[] { "Plow", "Tree", "EndGrow", "Stone", "Interactable" }))
                        {
                            GameObject ConnectedObejct = ItemManager.Instance.connectItem.FindObject(gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num);
                            if (ConnectedObejct != null)
                            {
                                GameObject go = Instantiate(ConnectedObejct);
                                go.transform.parent = GameManager.Instance.transform;
                                go.transform.position = tartgetPosition;
                            }
                            UseItemOnHand(1);

                            AudioManager.Instance.PlaySFX(AudioManager.Instance.ReadyAudio["PutItem"]);
                            ChangeSlot(nownum);
                        }
                    }

                    break;
                case ItemType.Except:
                    if (!gameObject.GetComponent<CheckFieldOnMouse>().MouseFollower.activeSelf) return;

                    if (GameManager.Instance.TagIsInMouse(new string[] { "Interactable" }))
                    {
                        GameObject go = GameManager.Instance.TagOnMouse.Find(itemGameObject => itemGameObject.TryGetComponent<IInteractNum>(out _));
                        if (go != null)
                        {
                            Debug.Log(go);
                            go.GetComponent<IInteractNum>().Interact(gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num);
                        }
                    }

                    TryHandInteract();
                    break;
                default:
                    TryHandInteract();
                    break;
            }
        }
        else if (nowInteractType == PlayerInteractType.Range)
        {
            PlayerInteractRange.SetActive(true);
            PlayerInteractRange.transform.position = gameObject.transform.position;
            PlayerInteractRange.transform.localScale = Vector3.one * 2.5f * gameObject.GetComponent<Player>().stat.ActiveRange; //범위는 아이템에서 가져오기

            switch (chooseItemType)
            {
                case ItemType.Sickle:
                    tartgetPosition = GameManager.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
                    CheckAngle();
                    SaveDirextionInfo();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetFloat(gameObject.GetComponent<Player>().playerAnimation.SpeedParameterHash,
                        ItemManager.Instance.itemDataReader.itemsDatas[gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num].Speed);
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.SickleParameterHash);
                    readyRangeInteract = new RangeInteract()
                    {
                        _Tag = new string[] { "EndGrow" },
                        _Dir = DirectionSave,
                        _isAll = true
                    };
                    break;
                case ItemType.Axe:
                    tartgetPosition = GameManager.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
                    CheckAngle();
                    SaveDirextionInfo();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetFloat(gameObject.GetComponent<Player>().playerAnimation.SpeedParameterHash,
                        ItemManager.Instance.itemDataReader.itemsDatas[gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num].Speed);
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.AxeParameterHash);
                    readyRangeInteract = new RangeInteract()
                    {
                        _Tag = new string[] { "Tree" },
                        _Dir = DirectionSave,
                        _isAll = false
                    };
                    break;
                case ItemType.Pickaxe:
                    tartgetPosition = GameManager.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
                    CheckAngle();
                    SaveDirextionInfo();
                    isAction = true;
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetFloat(gameObject.GetComponent<Player>().playerAnimation.SpeedParameterHash,
                        ItemManager.Instance.itemDataReader.itemsDatas[gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num].Speed);
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.PickaxeParameterHash);
                    readyRangeInteract = new RangeInteract()
                    {
                        _Tag = new string[] { "Stone" },
                        _Dir = DirectionSave,
                        _isAll = false
                    };
                    break;
                default:
                    break;
            }
        }
        else if (nowInteractType == PlayerInteractType.Charge)
        {
            switch (chooseItemType)
            {
                case ItemType.FishingRod:
                    tartgetPosition = GameManager.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
                    CheckAngle();
                    isAction = true;

                    FishingGauge.SetActive(true);
                    FishingGauge.transform.position = gameObject.transform.position;
                    FishingGauge.GetComponent<FishingGauge>().MoveGauge(0f);

                    MouseInteract.SetActive(true);
                    MouseInteract.transform.position = new Vector3(gameObject.transform.position.x + dir.x * 2, gameObject.transform.position.y + dir.y * 2,0);

                    gameObject.GetComponent<Player>().playerAnimation.animator.SetTrigger(gameObject.GetComponent<Player>().playerAnimation.FishingParameterHash);
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetInteger(gameObject.GetComponent<Player>().playerAnimation.FishingStateParameterHash, 0);

                    StartCoroutine(FishingChargingCoroutine());
                    break;
                default:
                    break;
            }
        }
    }

    public bool UseItemOnHand(int num)
    {
        return gameObject.GetComponent<Player>().inventory.UseItem(nownum - 1, num);
    }

    IEnumerator FishingChargingCoroutine()
    {
        float Charge = 0f;
        float MaxCharge = 1f;
        int Mul = 1;

        while (true)
        {
            if (Charge > MaxCharge)
            {
                Mul = -1;
            }
            else if (Charge < 0)
            {
                Mul += 1;
            }

            Charge += Time.deltaTime * Mul;
            FishingGauge.GetComponent<FishingGauge>().MoveGauge(Charge / MaxCharge);

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                gameObject.GetComponent<Player>().playerAnimation.animator.SetInteger(gameObject.GetComponent<Player>().playerAnimation.FishingStateParameterHash, 1);
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        FishingGauge.SetActive(false);

        if (GameManager.Instance.TagIsInMouse(new string[] { "Fishable" }))
        {
            StartCoroutine(IdleFishing(Charge / MaxCharge));
        }
        else
        {
            gameObject.GetComponent<Player>().playerAnimation.animator.SetInteger(gameObject.GetComponent<Player>().playerAnimation.FishingStateParameterHash, 0);
            EndAction();
        }
    }

    IEnumerator IdleFishing(float percentage)
    {
        yield return new WaitForSeconds(1.5f);

        bool isHooked = false;
        float HookedTime = UnityEngine.Random.Range(3f,7f);
        float NowTime = 0f;

        while (true)
        {
            NowTime += Time.deltaTime;

            if (NowTime > HookedTime)
            {
                gameObject.GetComponent<Player>().playerAnimation.animator.SetInteger(gameObject.GetComponent<Player>().playerAnimation.FishingStateParameterHash, 2);
                isHooked = true;
                break;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                gameObject.GetComponent<Player>().playerAnimation.animator.SetInteger(gameObject.GetComponent<Player>().playerAnimation.FishingStateParameterHash, 0);
                break;
            }
            yield return null;
        }

        if (isHooked)
        {
            GameManager.Instance.minigameManager.fishingMinigame.OnAllFishingCollider();
            NowTime = 0;

            while (true)
            {
                NowTime += Time.deltaTime;

                if (NowTime > 2)
                {
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetInteger(gameObject.GetComponent<Player>().playerAnimation.FishingStateParameterHash, 0);
                    GameManager.Instance.minigameManager.fishingMinigame.OffAllFishingCollider();
                    break;
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    gameObject.GetComponent<Player>().playerAnimation.animator.SetInteger(gameObject.GetComponent<Player>().playerAnimation.FishingStateParameterHash, 3);
                    LastItemNum = GameManager.Instance.minigameManager.fishingMinigame.CheckHookedFish((int)((1- percentage) * 5));
                    GameManager.Instance.minigameManager.fishingMinigame.StartMinigame(ItemManager.Instance.itemDataReader.itemsDatas[LastItemNum].Item_Price / 5 + 1);
                    break;
                }
                yield return null;
            }
        }
    }

    public void EndFishing(bool isSuccess)
    {
        if (isSuccess)
        {
            gameObject.GetComponent<Player>().playerAnimation.animator.SetInteger(gameObject.GetComponent<Player>().playerAnimation.FishingStateParameterHash, 4);

            //낚시 퀘스트 진행도 보고
            var fishItem = ItemManager.Instance.itemDataReader.itemsDatas[LastItemNum];
            string fishName = fishItem.Item_name;

            foreach (var quest in QuestManager.Instance.GetActiveQuestTargets())
            {
                if (quest == fishName)
                {
                    QuestManager.Instance.ReportProgress(fishName, 1);
                    Debug.Log($"[Fishing] 퀘스트 보고됨: {fishName}");
                    break;
                }
            }

            GameManager.Instance.player.GetComponent<Player>().inventory.GetItem(ItemManager.Instance.itemDataReader.itemsDatas[LastItemNum], 1);
        }
        else
        {
            gameObject.GetComponent<Player>().playerAnimation.animator.SetInteger(gameObject.GetComponent<Player>().playerAnimation.FishingStateParameterHash, 0);
        }
        GameManager.Instance.minigameManager.fishingMinigame.OffAllFishingCollider();
    }

    void SaveDirextionInfo()
    {
        DirectionSave = 0;
        if (dir == new Vector2(-1, 0))
        {
            DirectionSave = 3;
        }
        else if (dir == new Vector2(1, 0))
        {
            DirectionSave = 1;
        }
        else if (dir == new Vector2(0, 1))
        {
            DirectionSave = 0;
        }
        else
        {
            DirectionSave = 2;
        }
    }

    void TryHandInteract()
    {
        if (GameManager.Instance.TagIsInMouse(new string[] { "EndGrow", "Interactable"}))
        {
            GameManager.Instance.TryHandInteract();
        }
    }


    void OnInventory(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            ChangeSlot(nownum);
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ToggleInventoryUI();

                if (UIManager.Instance.InventoryIsOpen())
                {
                    dir = Vector2.zero;
                }
            }
        }
    }
    public void EndAction()
    {
        dir = Vector2.zero;
        gameObject.GetComponent<Player>().playerAnimation.animator.SetFloat(gameObject.GetComponent<Player>().playerAnimation.SpeedParameterHash, 1f);
        isAction = false;
    }

    void CheckAngle()
    {
        float angleDegrees = Mathf.Atan2(tartgetPosition.x - gameObject.transform.position.x, tartgetPosition.y - gameObject.transform.position.y) * Mathf.Rad2Deg;

        if (angleDegrees >= -135 && angleDegrees < -45)
        {
            dir = new Vector2(-1, 0);
        }
        else if (angleDegrees >= -45 && angleDegrees < 45)
        {
            dir = new Vector2(0, 1);
        }
        else if (angleDegrees >= 45 && angleDegrees < 135)
        {
            dir = new Vector2(1, 0);
        }
        else
        {
            dir = new Vector2(0, -1);
        }
    }

    public void RangeInteractObject()
    {
        GameManager.Instance.TagIsInRange(readyRangeInteract._Tag, readyRangeInteract._Dir, readyRangeInteract._isAll);
    }

    public void SpawnObject()
    {
        if (!CanSpawn) return;
        CanSpawn = false;

        ItemManager.Instance.spawnGround.SpawnGrounds(Groundtype, tartgetPosition);
    }

    void OnOneSlot(InputValue inputValue)
    {
        ChangeSlot(1);
    }

    void OnTwoSlot(InputValue inputValue)
    {
        ChangeSlot(2);
    }

    void OnThreeSlot(InputValue inputValue)
    {
        ChangeSlot(3);
    }

    void OnFourSlot(InputValue inputValue)
    {
        ChangeSlot(4);
    }

    void OnFiveSlot(InputValue inputValue)
    {
        ChangeSlot(5);
    }

    void OnSixSlot(InputValue inputValue)
    {
        ChangeSlot(6);
    }

    void OnSevenSlot(InputValue inputValue)
    {
        ChangeSlot(7);
    }

    void OnEightSlot(InputValue inputValue)
    {
        ChangeSlot(8);
    }

    void OnNineSlot(InputValue inputValue)
    {
        ChangeSlot(9);
    }

    void OnTenSlot(InputValue inputValue)
    {
        ChangeSlot(10);
    }

    void OnElevenSlot(InputValue inputValue)
    {
        ChangeSlot(11);
    }

    void OnTwelveSlot(InputValue inputValue)
    {
        ChangeSlot(12);
    }

    void ChangeSlot(int num)
    {
        if (isAction) return;

        nownum = num;
        QuickSlotUIManager.Instance.SelectSlot(nownum - 1);

        if (gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num == 0)
        {
            chooseItemType = ItemType.Except;
            gameObject.GetComponent<Player>().stat.ActiveRange = 1;
            ItemDamage = 0;
        }
        else
        {
            chooseItemType = ItemManager.Instance.itemDataReader.itemsDatas[gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num].Item_Type;
            gameObject.GetComponent<Player>().stat.ActiveRange = ItemManager.Instance.itemDataReader.itemsDatas[gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num].Range;
            ItemDamage = ItemManager.Instance.itemDataReader.itemsDatas[gameObject.GetComponent<Player>().inventory.PlayerHave[nownum - 1].ItemData_num].Damage;
        }

        switch (chooseItemType)
        {
            case ItemType.Except:
            case ItemType.Hoe:
            case ItemType.Watering:
            case ItemType.Seed:
            case ItemType.Interia:
            case ItemType.TreeSeed:
                TryChangeType(PlayerInteractType.Point);
                break;

            case ItemType.Sickle:
            case ItemType.Axe:
            case ItemType.Pickaxe:
                TryChangeType(PlayerInteractType.Range);
                break;

            case ItemType.FishingRod:
                TryChangeType(PlayerInteractType.Charge);
                break;

        }
    }
    void TryChangeType(PlayerInteractType type)
    {
        if (nowInteractType == type) return;

        switch (type)
        {
            case PlayerInteractType.Range:
                nowInteractType = PlayerInteractType.Range;

                PlayerInteractRange.gameObject.SetActive(true);
                MouseInteract.SetActive(false);
                gameObject.GetComponent<CheckFieldOnMouse>().enabled = false;
                break;

            case PlayerInteractType.Point:
                nowInteractType = PlayerInteractType.Point;

                PlayerInteractRange.gameObject.SetActive(false);
                gameObject.GetComponent<CheckFieldOnMouse>().enabled = true;
                break;

            case PlayerInteractType.Charge:
                nowInteractType = PlayerInteractType.Charge;

                PlayerInteractRange.gameObject.SetActive(false);
                MouseInteract.SetActive(false);
                gameObject.GetComponent<CheckFieldOnMouse>().enabled = false;
                break;
        }
    }
}
