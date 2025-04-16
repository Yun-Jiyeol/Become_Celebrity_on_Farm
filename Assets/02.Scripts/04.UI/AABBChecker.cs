using UnityEngine;

public class AABBChecker : MonoBehaviour
{
    void LateUpdate()
    {
        RectTransform[] allRects = Resources.FindObjectsOfTypeAll<RectTransform>();

        foreach (RectTransform rt in allRects)
        {
            // Transform�� ��ȿ�� UI ������Ʈ���� Ȯ��
            if (rt == null || rt.gameObject == null)
                continue;

            try
            {
                var width = rt.rect.width;
                var height = rt.rect.height;

                if (float.IsNaN(width) || float.IsNaN(height))
                {
                    Debug.LogError($"[AABB ����] {rt.gameObject.name}  NaN ���Ե�!");
                }
                else if (width <= 0 || height <= 0)
                {
                    Debug.LogError($"[AABB ����] {rt.gameObject.name} ũ�Ⱑ 0 �Ǵ� ���� �� Width: {width}, Height: {height}");
                }
            }
            catch
            {
                Debug.LogError($"[AABB ����] {rt.gameObject.name} rect ���� �� ���� �߻�!");
            }
        }
    }
}
