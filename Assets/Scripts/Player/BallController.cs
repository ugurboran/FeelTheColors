// BallController.cs - GÜNCELLENMİŞ
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    // Inspector'da ayarlanacak renkler dizisi
    public Color[] availableColors;

    // Renk değişimi particle efekti (Inspector'da atanacak)
    public ParticleSystem colorChangeParticles;

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

    // Her frame'de çalışır
    void Update()
    {
        // Oyun durdurulmuşsa dokunuşları algılama - YENİ
        if (Time.timeScale == 0f)
        {
            return; // Hiçbir şey yapma
        }

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
        currentColorIndex = (currentColorIndex + 1) % availableColors.Length;

        // Sprite'ın rengini yeni renge ayarla
        spriteRenderer.color = availableColors[currentColorIndex];

        // Particle efektini çalıştır
        PlayColorChangeEffect();

        // Renk değişimi sesi çal
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayColorChangeSound();
        }
    }

    /*
    // Renk değişimi particle efektini çalıştır
    void PlayColorChangeEffect()
    {
        if (colorChangeParticles != null)
        {
            // Particle'ın rengini topun yeni rengiyle aynı yap
            var main = colorChangeParticles.main;
            main.startColor = availableColors[currentColorIndex];

            // Particle'ı çalıştır
            colorChangeParticles.Play();
        }
    }
    */

    // Dışarıdan şu anki rengi almak için (Obstacle kullanacak)
    public Color GetCurrentColor()
    {
        // Şu anki rengi döndür
        return availableColors[currentColorIndex];
    }

    void PlayColorChangeEffect()
    {
        Debug.Log("PlayColorChangeEffect çağrıldı!"); // TEST LOG

        if (colorChangeParticles != null)
        {
            Debug.Log("Particle bulundu, çalıştırılıyor!"); // TEST LOG

            var main = colorChangeParticles.main;
            main.startColor = availableColors[currentColorIndex];

            colorChangeParticles.Play();
        }
        else
        {
            Debug.Log("HATA: colorChangeParticles NULL!"); // HATA LOG
        }
    }
}