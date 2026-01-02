// BallController.cs - YENİ INPUT SYSTEM İÇİN
using UnityEngine;
using UnityEngine.InputSystem; // YENİ SATIR

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
        // MOUSE veya TOUCH kontrolü - YENİ SİSTEM
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Fare ile tıklama
            ChangeColor();
        }

        // Dokunmatik ekran kontrolü
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            // Mobil dokunuş
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

        // Renk değişimi sesi çal
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayColorChangeSound();
        }
    }

    // Dışarıdan şu anki rengi almak için (Obstacle kullanacak)
    public Color GetCurrentColor()
    {
        // Şu anki rengi döndür
        return availableColors[currentColorIndex];
    }
}