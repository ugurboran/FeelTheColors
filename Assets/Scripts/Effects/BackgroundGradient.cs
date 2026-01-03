// BackgroundGradient.cs
using UnityEngine;

public class BackgroundGradient : MonoBehaviour
{
    // İki renk arasında geçiş yapacağız
    public Color color1 = new Color(0.1f, 0.1f, 0.3f); // Koyu mavi
    public Color color2 = new Color(0.3f, 0.1f, 0.3f); // Mor

    // Geçiş hızı
    public float transitionSpeed = 0.5f;

    // Kamera referansı
    private Camera mainCamera;

    // Geçiş için değişkenler
    private float t = 0f;
    private bool increasing = true;

    // Başlangıçta çalışır
    void Start()
    {
        // Main Camera'yı al
        mainCamera = Camera.main;

        // Eğer kamera yoksa hata ver
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera bulunamadı!");
        }
    }

    // Her frame çalışır
    void Update()
    {
        // Eğer kamera yoksa çık
        if (mainCamera == null) return;

        // t değerini güncelle (0 ile 1 arasında)
        if (increasing)
        {
            t += Time.deltaTime * transitionSpeed;
            if (t >= 1f)
            {
                t = 1f;
                increasing = false;
            }
        }
        else
        {
            t -= Time.deltaTime * transitionSpeed;
            if (t <= 0f)
            {
                t = 0f;
                increasing = true;
            }
        }

        // İki renk arasında yumuşak geçiş yap
        Color currentColor = Color.Lerp(color1, color2, t);

        // Kameranın arka plan rengini değiştir
        mainCamera.backgroundColor = currentColor;
    }
}