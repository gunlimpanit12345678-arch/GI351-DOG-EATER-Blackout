using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image redBar;
    [SerializeField] private Image whiteBar;

    [Header("Settings")]
    [SerializeField] private float whiteBarDelay = 0.5f; // หน่วงเวลาก่อนสีขาวจะเริ่มลด
    [SerializeField] private float whiteBarSpeed = 2f;    // ความเร็วในการลดของสีขาว

    private float currentHealthPercent = 1f; // ค่าเลือดปัจจุบัน (0.0 ถึง 1.0)
    private Coroutine whiteBarCoroutine;
    private HealthBarShake shakeEffect;

    private void Awake()
    {
        shakeEffect = GetComponent<HealthBarShake>();
    }

    // เรียกฟังก์ชันนี้เมื่อโดนโจมตี โดยส่งค่า (เลือดปัจจุบัน / เลือดสูงสุด) เข้ามา
    public void TakeDamage(float newHealthPercent)
    {
        currentHealthPercent = Mathf.Clamp01(newHealthPercent);

        // 1. สีแดงลดทันที
        redBar.fillAmount = currentHealthPercent;

        // 2. สั่งให้หลอดเลือดสั่น
        if (shakeEffect != null)
        {
            shakeEffect.TriggerShake();
        }

        // 3. เริ่มนับถอยหลังเพื่อลดหลอดสีขาวตาม
        if (whiteBarCoroutine != null)
        {
            StopCoroutine(whiteBarCoroutine);
        }
        whiteBarCoroutine = StartCoroutine(UpdateWhiteBar());
    }

    private IEnumerator UpdateWhiteBar()
    {
        // รอก่อนเริ่มลดสีขาว
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
    }
}
