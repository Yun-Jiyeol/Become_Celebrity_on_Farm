using UnityEngine;

public class RendererDebugger : MonoBehaviour
{
    void Start()
    {
        Debug.Log("[RendererDebugger] ����� ");

    }

    void LateUpdate()
    {
        var allRenderers = Resources.FindObjectsOfTypeAll<Renderer>();

        foreach (var r in allRenderers)
        {
            if (r == null || r.gameObject == null)
                continue;

            var bounds = r.bounds;
            var scale = r.transform.lossyScale;

            if (bounds.size == Vector3.zero)
            {
                Debug.LogError($"[������ ����] {r.gameObject.name}  bounds size = zero �� Pos: {r.transform.position}");
            }

            if (scale == Vector3.zero)
            {
                Debug.LogError($"[������ ����] {r.gameObject.name}  scale = zero �� Pos: {r.transform.position}");
            }

            if (r is SpriteRenderer sr && sr.sprite == null)
            {
                Debug.LogError($"[������ ����] {r.gameObject.name}  SpriteRenderer�� sprite ����!");
            }
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

