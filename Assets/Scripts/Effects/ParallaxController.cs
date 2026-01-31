// ParallaxController.cs - PARALLAX YÖNETİCİSİ
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [Header("Parallax Katmanları")]
    public Transform[] layers; // Tüm parallax layer'ları

    [Header("Hız Ayarları")]
    public float[] parallaxFactors; // Her layer için hız çarpanı (0.1, 0.3, 0.5, 1.0)
    public float baseSpeed = 2f;    // Temel hız (obstacle hızıyla aynı)

    [Header("Loop Ayarları")]
    public float resetPositionX = 20f; // Ne zaman başa dönecek
    public float startPositionX = -20f; // Başa dönüş pozisyonu

    void Update()
    {
        // ObstacleSpawner'dan hızı al
        ObstacleSpawner spawner = FindObjectOfType<ObstacleSpawner>();
        if (spawner != null)
        {
            baseSpeed = spawner.currentObstacleSpeed; // Şu anki engel hızı
        }

        // Her layer'ı hareket ettir
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i] != null && i < parallaxFactors.Length)
            {
                // Sola hareket et (parallax faktörü ile)
                float speed = baseSpeed * parallaxFactors[i];
                layers[i].position += Vector3.left * speed * Time.deltaTime;

                // Loop kontrolü (sağa geçince başa dön)
                if (layers[i].position.x < startPositionX)
                {
                    Vector3 pos = layers[i].position;
                    pos.x = resetPositionX;
                    layers[i].position = pos;
                }
            }
        }
    }
}
