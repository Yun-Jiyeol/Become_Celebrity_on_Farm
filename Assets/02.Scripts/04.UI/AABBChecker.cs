using UnityEngine;

public class AABBChecker : MonoBehaviour
{
    void LateUpdate()
    {
        RectTransform[] allRects = Resources.FindObjectsOfTypeAll<RectTransform>();

        foreach (RectTransform rt in allRects)
        {
            // Transform이 유효한 UI 오브젝트인지 확인
            if (rt == null || rt.gameObject == null)
                continue;

            try
            {
                var width = rt.rect.width;
                var height = rt.rect.height;

                if (float.IsNaN(width) || float.IsNaN(height))
                {
                    Debug.LogError($"[AABB 문제] {rt.gameObject.name}  NaN 포함됨!");
                }
                else if (width <= 0 || height <= 0)
                {
                    Debug.LogError($"[AABB 문제] {rt.gameObject.name} 크기가 0 또는 음수 → Width: {width}, Height: {height}");
                }
            }
            catch
            {
                Debug.LogError($"[AABB 예외] {rt.gameObject.name} rect 접근 중 예외 발생!");
            }
        }
    }
}
