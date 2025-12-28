// BallController.cs
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Inspector'da ayarlanacak renkler dizisi
    public Color[] availableColors;

    // Şu anda hangi renkteyiz (0, 1, 2, 3...)
    private int currentColorIndex = 0;

    // Topun rengini değiştirmek için sprite renderer bileşeni
    private SpriteRenderer spriteRenderer;

    // Oyun başladığında bir kez çalışır
    void Start()
    {
        // Bu objenin sprite renderer bileşenini al
        spriteRenderer = GetComponent<SpriteRenderer>();

        // İlk rengi ayarla
        ChangeColor();
    }

    // Her frame'de çalışır (saniyede onlarca kez)
    void Update()
    {
        // Fare sol tıklaması VEYA mobilde ekrana dokunma kontrolü
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // Rengi değiştir
            ChangeColor();
        }
    }

    // Topun rengini bir sonraki renge değiştirir
    void ChangeColor()
    {
        // Sıradaki renk indexine geç (0->1->2->3->0...)
        // % operatörü sayıyı sıfırlar (örn: 4 % 4 = 0)
        currentColorIndex = (currentColorIndex + 1) % availableColors.Length;

        // Sprite'ın rengini yeni renge ayarla
        spriteRenderer.color = availableColors[currentColorIndex];
    }

    // Dışarıdan şu anki rengi almak için (Obstacle kullanacak)
    public Color GetCurrentColor()
    {
        // Şu anki rengi döndür
        return availableColors[currentColorIndex];
    }
}