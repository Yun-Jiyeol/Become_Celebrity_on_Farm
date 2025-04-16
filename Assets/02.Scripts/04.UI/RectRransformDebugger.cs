using UnityEngine;

public class RectTransformDebugger : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("RectTransformDebugger 시작");
    }
    void LateUpdate()
    {
        RectTransform[] allRects = Resources.FindObjectsOfTypeAll<RectTransform>();
        foreach (var rt in allRects)
        {
            if (rt == null || rt.gameObject == null)
                continue;

            Vector3 pos = rt.position;
            Vector2 size = rt.rect.size;
            Vector3 scale = rt.lossyScale;

            if (float.IsNaN(size.x) || float.IsNaN(size.y) || size.x <= 0 || size.y <= 0)
            {
                Debug.LogError($"[Rect 문제] {rt.name}  Size: {size}, Pos: {pos}, Scale: {scale}");
            }

            if (scale == Vector3.zero)
            {
                Debug.LogError($"[Scale 문제] {rt.name}  스케일이 0임! 위치: {pos}");
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
