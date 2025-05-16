using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnStartGameBtn : StartSceneBtn
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        SceneChangerManager.Instance.OnClick_LoadScene(SceneChangerManager.Instance.sceneNamesInBuild[1]);
    }
}
