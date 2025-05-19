using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnStartGameBtn : StartSceneBtn
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Instance.ReadyAudio["BGBGM"]);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.StopBGM();
        SceneChangerManager.Instance.OnClick_LoadScene(SceneChangerManager.Instance.sceneNamesInBuild[1]);
    }
}
