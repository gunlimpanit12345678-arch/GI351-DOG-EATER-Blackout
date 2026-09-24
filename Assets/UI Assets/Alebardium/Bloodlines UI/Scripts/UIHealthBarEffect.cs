using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarEffect : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image redBar;   // หลอดเลือดแดง (เพื่อนจะเป็นคนปรับ fillAmount หลอดนี้)
    [SerializeField] private Image whiteBar; // หลอดเลือดขาว (สคริปต์นี้จะคุมเอง)

    [Header("Shake Settings")]
    [SerializeField] private bool enableShake = true;
    [SerializeField] private float shakeDuration = 0.2f; // ระยะเวลาสั่น
    [SerializeField] private float shakeMagnitude = 8f;  // ความแรงในการสั่น

    [Header("White Bar Settings")]
    [SerializeField] private float whiteBarDelay = 0.4f; // หน่วงเวลาก่อนสีขาวจะลดตาม
    [SerializeField] private float whiteBarSpeed = 1.5f; // ความเร็วในการลดของสีขาว

    private float lastRedFill;
    private RectTransform rectTransform;
    private Vector3 originalPosition;

    private Coroutine whiteBarCoroutine;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    private void Start()
    {
        if (redBar != null)
        {
            lastRedFill = redBar.fillAmount;
            if (whiteBar != null)
            {
                whiteBar.fillAmount = redBar.fillAmount;
            }
        }
    }

    private void Update()
    {
        if (redBar == null || whiteBar == null) return;

        // 1. ตรวจจับเมื่อ "เลือดลดลง" (ค่า fillAmount ของสีแดงน้อยกว่าค่าก่อนหน้า)
        if (redBar.fillAmount < lastRedFill)
        {
            // สั่งสั่นหลอดเลือด
            if (enableShake)
            {
                TriggerShake();
            }

            // เริ่มนับถอยหลังเพื่อลดหลอดสีขาวตาม
            if (whiteBarCoroutine != null) StopCoroutine(whiteBarCoroutine);
            whiteBarCoroutine = StartCoroutine(UpdateWhiteBar());
        }
        // 2. ถ้าเป็นการ "เพิ่มเลือด (Heal)" ให้หลอดขาวเพิ่มตามทันที
        else if (redBar.fillAmount > lastRedFill)
        {
            if (whiteBarCoroutine != null) StopCoroutine(whiteBarCoroutine);
            whiteBar.fillAmount = redBar.fillAmount;
        }

        // อัปเดตค่าล่าสุดเก็บไว้เช็คในเฟรมถัดไป
        lastRedFill = redBar.fillAmount;
    }

    // --- ระบบลดหลอดสีขาวตามช้าๆ ---
    private IEnumerator UpdateWhiteBar()
    {
        // รอก่อนเริ่มลด
        yield return new WaitForSeconds(whiteBarDelay);

        // ค่อยๆ ลด Fill Amount ของสีขาวลงมาให้เท่ากับสีแดง
        while (whiteBar.fillAmount > redBar.fillAmount)
        {
            whiteBar.fillAmount = Mathf.MoveTowards(
                whiteBar.fillAmount,
                redBar.fillAmount,
                whiteBarSpeed * Time.deltaTime
            );
            yield return null;
        }

        whiteBar.fillAmount = redBar.fillAmount;
    }

    // --- ระบบสั่นเมื่อโดนตี ---
    private void TriggerShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            rectTransform.anchoredPosition = originalPosition;
        }
        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            rectTransform.anchoredPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
    }
}
