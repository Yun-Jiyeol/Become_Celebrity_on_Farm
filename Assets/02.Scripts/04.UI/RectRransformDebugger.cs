using UnityEngine;

public class RectTransformDebugger : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("RectTransformDebugger ����");
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
                Debug.LogError($"[Rect ����] {rt.name}  Size: {size}, Pos: {pos}, Scale: {scale}");
            }

            if (scale == Vector3.zero)
            {
                Debug.LogError($"[Scale ����] {rt.name}  �������� 0��! ��ġ: {pos}");
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
