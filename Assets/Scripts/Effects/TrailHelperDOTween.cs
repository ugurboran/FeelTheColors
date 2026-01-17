// TrailHelperDOTween.cs
using UnityEngine;
using DG.Tweening;

public class TrailHelperDOTween : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveDuration = 1.5f;
    public float moveDistance = 0.12f;
    public Ease moveEase = Ease.InOutSine;

    private Vector3 startLocalPos;
    private Tween moveTween;

    void Start()
    {
        startLocalPos = transform.localPosition;
        StartMovement();
    }

    void StartMovement()
    {
        // Sağa-sola yumuşak hareket
        moveTween = transform.DOLocalMoveX(startLocalPos.x + moveDistance, moveDuration)
            .SetEase(moveEase)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void OnDestroy()
    {
        moveTween?.Kill();
    }
}