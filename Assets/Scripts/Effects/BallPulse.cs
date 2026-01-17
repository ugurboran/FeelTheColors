// BallPulse.cs
using UnityEngine;

public class BallPulse : MonoBehaviour
{
    // Pulse hızı
    public float pulseSpeed = 3f;

    // Pulse miktarı (ne kadar büyüyüp küçülecek)
    public float pulseAmount = 0.05f;

    // Orijinal scale
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Sinüs dalgası ile büyü-küçül
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale + Vector3.one * pulse;
    }
}