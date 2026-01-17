// BallAnimations.cs - DOTWEEN ANIMASYONLARI
using DG.Tweening; // DOTween
using UnityEngine;

public class BallAnimations : MonoBehaviour
{
    [Header("Pulse Animasyonu")]
    public float pulseDuration = 0.8f;
    public float pulseScale = 1.15f;
    public Ease pulseEase = Ease.InOutSine;

    [Header("Float Animasyonu")]
    public float floatDuration = 2f;
    public float floatHeight = 0.15f;
    public Ease floatEase = Ease.InOutSine;

    [Header("Rotate Animasyonu")]
    public bool enableRotation = true;
    public float rotationDuration = 3f;
    public Ease rotationEase = Ease.Linear;

    private Vector3 originalPosition;
    private Sequence pulseSequence;
    private Tween floatTween;
    private Tween rotateTween;

    void Start()
    {
        originalPosition = transform.position;

        // Animasyonları başlat
        StartPulseAnimation();
        StartFloatAnimation();

        if (enableRotation)
        {
            StartRotationAnimation();
        }
    }

    void StartPulseAnimation()
    {
        // Sequence ile büyü-küçül döngüsü
        pulseSequence = DOTween.Sequence();
        pulseSequence.Append(transform.DOScale(pulseScale, pulseDuration / 2).SetEase(pulseEase));
        pulseSequence.Append(transform.DOScale(1f, pulseDuration / 2).SetEase(pulseEase));
        pulseSequence.SetLoops(-1); // Sonsuz döngü
    }

    void StartFloatAnimation()
    {
        // Yukarı-aşağı float
        floatTween = transform.DOMoveY(originalPosition.y + floatHeight, floatDuration)
            .SetEase(floatEase)
            .SetLoops(-1, LoopType.Yoyo); // Gidip gel
    }

    void StartRotationAnimation()
    {
        // 360 derece dönüş
        rotateTween = transform.DORotate(new Vector3(0, 0, 360), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(rotationEase)
            .SetLoops(-1, LoopType.Restart);
    }

    void OnDestroy()
    {
        // Animasyonları temizle
        pulseSequence?.Kill();
        floatTween?.Kill();
        rotateTween?.Kill();
    }
}