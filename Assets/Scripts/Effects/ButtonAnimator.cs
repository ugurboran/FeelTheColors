// ButtonAnimator.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Basınca ne kadar küçülecek
    public float scaleMultiplier = 0.9f;

    // Animasyon hızı
    public float animationSpeed = 10f;

    // Orijinal scale
    private Vector3 originalScale;

    // Hedef scale
    private Vector3 targetScale;

    // Başlangıçta çalışır
    void Start()
    {
        // Orijinal scale'i kaydet
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    // Her frame çalışır
    void Update()
    {
        // Yumuşak geçiş
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * animationSpeed);
    }

    // Butona basıldığında
    public void OnPointerDown(PointerEventData eventData)
    {
        // Küçült
        targetScale = originalScale * scaleMultiplier;
    }

    // Buton bırakıldığında
    public void OnPointerUp(PointerEventData eventData)
    {
        // Normal boyuta dön
        targetScale = originalScale;
    }
}