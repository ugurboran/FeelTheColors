// BallController.cs - GÜNCELLENMİŞ
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; // YENİ


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

    private TrailRenderer trailRenderer; // YENİ

    // Oyun başladığında bir kez çalışır
    void Start()
    {
        // Bu objenin sprite renderer bileşenini al
        spriteRenderer = GetComponent<SpriteRenderer>();

        trailRenderer = GetComponentInChildren<TrailRenderer>(); // YENİ

        // DEBUG LOG - YENİ
        if (trailRenderer != null)
        {
            Debug.Log("✅ Trail Renderer bulundu!");
        }
        else
        {
            Debug.LogError("❌ Trail Renderer BULUNAMADI!");
        }

        // İlk rengi ayarla
        ChangeColor();
    }

    // Her frame'de çalışır
    void Update()
    {


        // Oyun pause veya game over ise dokunuşları algılama - YENİ
        if (PauseManager.IsGamePaused || GameManager.IsGameOver)
        {
            return; // Hiçbir şey yapma
        }

        // MOUSE veya TOUCH kontrolü - YENİ SİSTEM
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Fare UI üzerinde mi kontrol et - YENİ
            if (!IsPointerOverUI())
            {
                ChangeColor();
            }
        }

        // Dokunmatik ekran kontrolü
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            // Dokunuş UI üzerinde mi kontrol et - YENİ
            if (!IsPointerOverUI())
            {
                ChangeColor();
            }
        }
    }

    // UI üzerinde mi kontrol et - YENİ FONKSİYON
    bool IsPointerOverUI()
    {
        // Mouse için
        if (Mouse.current != null)
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        // Touch için
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            int touchId = Touchscreen.current.primaryTouch.touchId.ReadValue();
            return EventSystem.current.IsPointerOverGameObject(touchId);
        }

        return false;
    }

    // Topun rengini bir sonraki renge değiştirir
    void ChangeColor()
    {
        // Sıradaki renk indexine geç (0->1->2->3->0...)
        currentColorIndex = (currentColorIndex + 1) % availableColors.Length;

        // Sprite'ın rengini yeni renge ayarla
        spriteRenderer.color = availableColors[currentColorIndex];

        // Trail rengini de değiştir - YENİ
        UpdateTrailColor();

        // Particle efektini çalıştır
        PlayColorChangeEffect();

        // Renk değişimi sesi çal
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayColorChangeSound();
        }
    }

    // Trail rengini güncelle - YENİ FONKSİYON
    void UpdateTrailColor()
    {

        Debug.Log("🎨 UpdateTrailColor çağrıldı"); // DEBUG LOG


        if (trailRenderer != null)
        {

            Debug.Log("✅ Trail rengini değiştiriyoruz"); // DEBUG LOG

            // Gradient oluştur (baştan sona fade)
            Gradient gradient = new Gradient();

            // Renk noktaları
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].color = availableColors[currentColorIndex];
            colorKeys[0].time = 0f;
            colorKeys[1].color = availableColors[currentColorIndex];
            colorKeys[1].time = 1f;

            // Alpha noktaları (baştan sona fade out)
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0].alpha = 1f;
            alphaKeys[0].time = 0f;
            alphaKeys[1].alpha = 0f;
            alphaKeys[1].time = 1f;

            gradient.SetKeys(colorKeys, alphaKeys);

            // Trail'e uygula
            trailRenderer.colorGradient = gradient;
        }
        else
        {
            Debug.LogError("❌ trailRenderer NULL!"); // DEBUG LOG
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