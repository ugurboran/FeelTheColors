// FaceController.cs - YÜZ İFADESİ DEĞİŞTİRİCİ
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    [Header("Yüz Sprite'ları")]
    public Sprite normalFace;    // Oyun başlangıç
    public Sprite happyFace;     // Engel geçince
    public Sprite excitedFace;   // Renk değişince
    public Sprite sadFace;       // Game over

    [Header("Animasyon Ayarları")]
    public float expressionDuration = 0.5f;  // İfade ne kadar sürsün
    public float punchScale = 0.15f;         // İfade değişiminde büyüme

    private SpriteRenderer faceRenderer;
    private Vector3 originalScale;

    void Start()
    {
        faceRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

        // Başlangıçta normal yüz
        SetFace(normalFace, false);
    }

    // Yüz değiştir
    void SetFace(Sprite newFace, bool animate = true)
    {
        if (faceRenderer != null && newFace != null)
        {
            faceRenderer.sprite = newFace;

            if (animate)
            {
                // Hafif büyüme animasyonu
                transform.DOKill();
                transform.DOPunchScale(Vector3.one * punchScale, 0.3f, 5, 0.5f);
            }
        }
    }

    // Normal yüz (varsayılan)
    public void ShowNormal()
    {
        SetFace(normalFace, false);
    }

    // Mutlu yüz (engel geçince)
    public void ShowHappy()
    {
        SetFace(happyFace, true);

        // 0.5 saniye sonra normale dön
        DOVirtual.DelayedCall(expressionDuration, () => {
            SetFace(normalFace, false);
        });
    }

    // Heyecanlı yüz (renk değişince)
    public void ShowExcited()
    {
        SetFace(excitedFace, true);

        // 0.3 saniye sonra normale dön (daha hızlı)
        DOVirtual.DelayedCall(0.3f, () => {
            SetFace(normalFace, false);
        });
    }

    // Üzgün yüz (game over)
    public void ShowSad()
    {
        SetFace(sadFace, true);
    }
}