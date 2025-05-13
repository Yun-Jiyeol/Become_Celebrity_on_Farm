using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public MinigameManager minigameManager;

    public GameObject player;
    public Camera camera;
    public GameObject MouseFollower;
    public GameObject PlayerRange;
    public GameObject FishingGauge;

    public List<GameObject> OnActive;
    public List<FishingCollider> FishingRange;
    public List<GameObject> TagOnMouse;

    Coroutine colliderCoroutine;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        camera = Camera.main;
    }

    public static GameManager Instance
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


    public void TryHandInteract()
    {
        if(TagOnMouse != null)
        {
            foreach(GameObject go in TagOnMouse)
            {
                if(go.TryGetComponent<SeedGrow>(out SeedGrow SG))
                {
                    SG.HandInteract();
                    return;
                }
                else if(go.tag == "Interactable")
                {
                    go.GetComponent<IInteract>().Interact();
                    return;
                }
            }
        }
    }

    public bool TagIsInMouse(string[] _tag)
    {
        foreach(string tag in _tag)
        {
            if(TagOnMouse.Count == 0) return false;

            for(int i =0;  i< TagOnMouse.Count; i++)
            {
                if(tag == TagOnMouse[i].transform.tag) return true;
            }
        }
        return false;
    }
    public bool TagIsNotInMouse(string[] _tag)
    {
        foreach (string tag in _tag)
        {
            if (TagOnMouse.Count == 0) return true;

            for (int i = 0; i < TagOnMouse.Count; i++)
            {
                if (tag == TagOnMouse[i].transform.tag) return false;
            }
        }
        return true;
    }
    public void TagIsInRange(string[] _tag, int _dir, bool isAll)
    {
        if (TagOnMouse.Count == 0) return;

        List< IInteract > saveInteract = new List< IInteract >();

        for (int i = 0; i < TagOnMouse.Count; i++)
        {
            foreach(string tag in _tag)
            {
                if (TagOnMouse[i].transform.tag == tag)
                {
                    float angleDegrees = Mathf.Atan2(TagOnMouse[i].transform.position.x - player.transform.position.x, TagOnMouse[i].transform.position.y - player.transform.position.y) * Mathf.Rad2Deg;
                    bool isin = false;
                    switch (_dir)
                    {
                        case 0:
                            if (angleDegrees <= 60 && angleDegrees >= -60)
                            {
                                isin = true;
                            }
                            break;
                        case 1:
                            if (angleDegrees <= 150 && angleDegrees >= 30)
                            {
                                isin = true;
                            }
                            break;
                        case 2:
                            if (angleDegrees <= -120 || angleDegrees >= 120)
                            {
                                isin = true;
                            }
                            break;
                        case 3:
                            if (angleDegrees <= -30 && angleDegrees >= -150)
                            {
                                isin = true;
                            }
                            break;
                    }

                    if (isin)
                    {
                        if (TagOnMouse[i].TryGetComponent<IInteract>(out IInteract temp))
                        {
                            saveInteract.Add(temp);
                            if (!isAll)
                            {
                                temp.Interact();
                                return;
                            }
                        }
                    }
                }
            }
        }

        foreach (var i in saveInteract)
        {
            i.Interact();
        }
    }

    public void TurnOnAllColliders()
    {
        if (OnActive == null) return;

        foreach (GameObject go in OnActive)
        {
            if(go.activeSelf == true)
            {
                go.GetComponent<SaveOnGM>().OnCollider();
            }
        }

        if (colliderCoroutine != null) StopCoroutine(colliderCoroutine);
        colliderCoroutine = StartCoroutine(OffCoroutineCollider());
    }

    void TurnOffAllColliders()
    {
        if (OnActive == null) return;

        foreach (GameObject go in OnActive)
        {
            if (go.activeSelf == true)
            {
                go.GetComponent<SaveOnGM>().OffCollider();
            }
        }
    }

    IEnumerator OffCoroutineCollider()
    {
        yield return new WaitForSeconds(2f);

        TurnOffAllColliders();
    }

    public void OneDayAfter()
    {
        List<GameObject> forActivefalse = new List<GameObject>();
        List<SeedGrow> forGrow = new List<SeedGrow>();

        for(int i = 0; i< OnActive.Count; i++)
        {
            if(OnActive[i].TryGetComponent<SeedGrow>(out SeedGrow temp)) //자라는 식물일 시
            {
                forGrow.Add(temp);
            }
            else if(OnActive[i].transform.tag == "Watered") //물바닥 일 시
            {
                forActivefalse.Add(OnActive[i]);
            }
        }

        if (forActivefalse.Count != 0)
        {
            foreach (GameObject go in forActivefalse)
            {
                go.SetActive(false);
            }
        }
        if (forGrow.Count != 0)
        {
            foreach (SeedGrow sg in forGrow)
            {
                sg.Grow(10);
            }
        }
    }

    public void OneSeasonAfter()
    {
        for (int i = 0; i < OnActive.Count; i++)
        {
            if (OnActive[i].TryGetComponent<SeedGrow>(out SeedGrow temp)) //자라는 식물일 시
            {
                temp.OnSettingSeason();
            }
        }
    }
}
