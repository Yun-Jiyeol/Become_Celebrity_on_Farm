using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public GameObject alex, tori;
    public Animator alexAnimator, toriAnimator;
    void Start()
    {
        alexAnimator = alex.GetComponent<Animator>();
        toriAnimator = tori.GetComponent<Animator>();
    }

    public void ShowAlex()
    {
        alex.SetActive(true);
        tori.SetActive(false);
        alexAnimator.Play("Alex_Idle");
    }
    public void ShowTori()
    {
        tori.SetActive(true);
        alex.SetActive(false);
        toriAnimator.Play("Tori_Idle");
    }
}
