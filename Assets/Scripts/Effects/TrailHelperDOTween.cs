// TrailHelperDOTween.cs - SADECE SOLA KUYRUK
using DG.Tweening;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;
using static UnityEditorInternal.ReorderableList;
using Sequence = DG.Tweening.Sequence;

public class TrailHelperDOTween : MonoBehaviour
{
    [Header("Kuyruk Ayarları")]
    public float speed = 2f;              // Hareket süresi (daha uzun)
    public float distance = 0.5f;         // Ne kadar SAĞA gidecek
    public float resetDelay = 0.05f;      // Başa dönme (çok hızlı)

    private Vector3 startLocalPos;
    private Sequence tailSequence;

    void Start()
    {
        startLocalPos = transform.localPosition;
        StartTailAnimation();

        Debug.Log("🦊 Kuyruk animasyonu başladı");
    }

    void StartTailAnimation()
    {
        tailSequence = DOTween.Sequence();

        // SAĞA git (yavaşça) ← DEĞİŞTİ (+ işareti)
        tailSequence.Append(
            transform.DOLocalMoveX(startLocalPos.x + distance, speed)
                .SetEase(Ease.Linear)
        );

        // Başa dön (anında veya hızlı)
        tailSequence.Append(
            transform.DOLocalMoveX(startLocalPos.x, resetDelay)
                .SetEase(Ease.OutQuad)
        );

        // Sonsuz döngü
        tailSequence.SetLoops(-1);
    }

    void OnDestroy()
    {
        tailSequence?.Kill();
    }
}