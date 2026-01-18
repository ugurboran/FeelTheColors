// PlayerSubtleMovement.cs - SAĞA + YUKARI-AŞAĞI
using DG.Tweening;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class PlayerSubtleMovement : MonoBehaviour
{
    [Header("Yatay Hareket (Sağa-Sola)")]
    public float moveSpeed = 1.5f;
    public float moveDistance = 0.2f;
    public float returnSpeed = 1.0f;
    public Ease moveEase = Ease.Linear;
    public Ease returnEase = Ease.InOutSine; // YENİ - Geri dönüş ease'i

    [Header("Dikey Hareket (Yukarı-Aşağı)")]
    public bool enableVerticalFloat = true;
    public float floatSpeed = 2f;          // Yukarı-aşağı hızı
    public float floatHeight = 0.1f;       // Ne kadar yukarı-aşağı
    public Ease floatEase = Ease.InOutSine;

    private Vector3 originalPosition;
    private DG.Tweening.Sequence moveSequence;
    private Tween floatTween;

    void Start()
    {
        originalPosition = transform.position;
        StartHorizontalMovement();

        if (enableVerticalFloat)
        {
            StartVerticalFloat();
        }
    }

    void StartHorizontalMovement()
    {
        moveSequence = DOTween.Sequence();

        // SAĞA git
        moveSequence.Append(
            transform.DOMoveX(originalPosition.x + moveDistance, moveSpeed)
                .SetEase(moveEase)
        );

        // Başa dön (smooth) - DEĞİŞTİ
        moveSequence.Append(
            transform.DOMoveX(originalPosition.x, returnSpeed)
                .SetEase(returnEase) // Smooth ease
        );

        moveSequence.SetLoops(-1);
    }

    void StartVerticalFloat()
    {
        // Yukarı-aşağı sallanım (bağımsız)
        floatTween = transform.DOMoveY(originalPosition.y + floatHeight, floatSpeed)
            .SetEase(floatEase)
            .SetLoops(-1, LoopType.Yoyo); // Gidip gel
    }

    void OnDestroy()
    {
        moveSequence?.Kill();
        floatTween?.Kill();
        transform.position = originalPosition;
    }
}