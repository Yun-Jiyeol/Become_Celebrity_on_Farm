using UnityEngine;

public class RendererDebugger : MonoBehaviour
{
    void Start()
    {
        Debug.Log("[RendererDebugger] ½ÇÇàµÊ ");

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
                Debug.LogError($"[·»´õ·¯ ¹®Á¦] {r.gameObject.name}  bounds size = zero ¡æ Pos: {r.transform.position}");
            }

            if (scale == Vector3.zero)
            {
                Debug.LogError($"[·»´õ·¯ ¹®Á¦] {r.gameObject.name}  scale = zero ¡æ Pos: {r.transform.position}");
            }

            if (r is SpriteRenderer sr && sr.sprite == null)
            {
                Debug.LogError($"[·»´õ·¯ ¹®Á¦] {r.gameObject.name}  SpriteRenderer¿¡ sprite ¾øÀ½!");
            }
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

