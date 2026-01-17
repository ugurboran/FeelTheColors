// TrailHelperDOTween.cs
using UnityEngine;
using DG.Tweening;

public class TrailHelperDOTween : MonoBehaviour
{
    public float moveDuration = 1f;
    public float moveDistance = 0.15f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;

        // Sağa-sola smooth hareket
        transform.DOLocalMoveX(startPos.x + moveDistance, moveDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}