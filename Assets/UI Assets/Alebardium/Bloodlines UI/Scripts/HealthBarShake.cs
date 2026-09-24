using System.Collections;
using UnityEngine;

public class HealthBarShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float duration = 0.2f; // ระยะเวลาที่สั่น
    [SerializeField] private float magnitude = 10f;  // ความแรงในการสั่น

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void TriggerShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            rectTransform.anchoredPosition = originalPosition; // รีเซ็ตตำแหน่งก่อนเริ่มสั่นใหม่
        }
        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // สุ่มตำแหน่ง X และ Y ให้อยู่ในช่วง magnitude
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            rectTransform.anchoredPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null; // รอเฟรมถัดไป
        }

        rectTransform.anchoredPosition = originalPosition; // คืนค่าตำแหน่งเดิม
    }
}
