using System.Collections;
using UnityEngine;

public class PlayerBlinkEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        Debug.Log("PlayerBlinkEffect: Start 호출됨");

        GameObject playerGO = GameManager.Instance?.player;

        if (playerGO == null)
        {
            Debug.LogWarning("PlayerBlinkEffect: GameManager.Instance.player가 null입니다!");
            return;
        }

        Player playerScript = playerGO.GetComponent<Player>();
        if (playerScript == null)
        {
            Debug.LogWarning("PlayerBlinkEffect: Player 컴포넌트를 찾을 수 없습니다!");
            return;
        }

        spriteRenderer = playerScript.spriteRenderer;

        if (spriteRenderer == null)
        {
            Debug.LogWarning("PlayerBlinkEffect: Player에서 SpriteRenderer를 찾지 못했습니다!");
            return;
        }

        originalColor = spriteRenderer.color;
        Debug.Log("PlayerBlinkEffect: SpriteRenderer 연결 성공!");
    }

    public void TriggerBlink()
    {
        if (spriteRenderer != null)
            StartCoroutine(BlinkRoutine());
        else
            Debug.LogWarning("PlayerBlinkEffect: TriggerBlink 호출 시 spriteRenderer가 null입니다!");
    }

    IEnumerator BlinkRoutine()
    {
        Color hitColor = Color.red;
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}