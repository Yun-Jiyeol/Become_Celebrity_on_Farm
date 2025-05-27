using System.Collections;
using UnityEngine;

public class PlayerBlinkEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        Debug.Log("PlayerBlinkEffect: Start ȣ���");

        GameObject playerGO = GameManager.Instance?.player;

        if (playerGO == null)
        {
            Debug.LogWarning("PlayerBlinkEffect: GameManager.Instance.player�� null�Դϴ�!");
            return;
        }

        Player playerScript = playerGO.GetComponent<Player>();
        if (playerScript == null)
        {
            Debug.LogWarning("PlayerBlinkEffect: Player ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }

        spriteRenderer = playerScript.spriteRenderer;

        if (spriteRenderer == null)
        {
            Debug.LogWarning("PlayerBlinkEffect: Player���� SpriteRenderer�� ã�� ���߽��ϴ�!");
            return;
        }

        originalColor = spriteRenderer.color;
        Debug.Log("PlayerBlinkEffect: SpriteRenderer ���� ����!");
    }

    public void TriggerBlink()
    {
        if (spriteRenderer != null)
            StartCoroutine(BlinkRoutine());
        else
            Debug.LogWarning("PlayerBlinkEffect: TriggerBlink ȣ�� �� spriteRenderer�� null�Դϴ�!");
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