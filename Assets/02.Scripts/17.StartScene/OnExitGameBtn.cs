using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnExitGameBtn : StartSceneBtn
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        SceneChangerManager.Instance.OnClick_QuitGame();
    }
}
