// ObstacleSpawner.cs
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // Inspector'da atanacak engel prefab'ı
    public GameObject obstaclePrefab;

    // Kullanılabilir renkler dizisi (Inspector'da ayarlanacak)
    public Color[] possibleColors;

    // Kaç saniyede bir engel oluşturulacak
    public float spawnInterval = 2f;

    // Engellerin sola doğru hareket hızı
    public float obstacleSpeed = 3f;

    // Engellerin spawn olabileceği en düşük Y pozisyonu
    public float minY = -3f;

    // Engellerin spawn olabileceği en yüksek Y pozisyonu
    public float maxY = 3f;

    // Zamanlayıcı değişkeni (spawn aralığını kontrol eder)
    private float timer = 0f;

    // Her frame'de çalışır
    void Update()
    {
        // Zamanlayıcıyı artır (geçen süreyi ekle)
        timer += Time.deltaTime;

        // Zamanlayıcı spawn aralığına ulaştı mı?
        if (timer >= spawnInterval)
        {
            // Yeni engel oluştur
            SpawnObstacle();

            // Zamanlayıcıyı sıfırla
            timer = 0f;
        }
    }

    // Yeni bir engel oluşturur
    void SpawnObstacle()
    {
        // MinY ve MaxY arasında rastgele bir Y pozisyonu seç
        float randomY = Random.Range(minY, maxY);

        // Spawn pozisyonunu oluştur (X: spawner'ın pozisyonu, Y: rastgele)
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0);

        // Prefab'dan yeni engel objesi oluştur
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        // Mevcut renklerden rastgele birini seç
        Color randomColor = possibleColors[Random.Range(0, possibleColors.Length)];

        // Oluşturulan engelin rengini ayarla
        obstacle.GetComponent<Obstacle>().lineColor = randomColor;

        // Engelin Rigidbody2D bileşenini al
        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();

        // Engele SAĞDAN SOLA (Vector2.left) hareket hızı ver
        rb.velocity = Vector2.left * obstacleSpeed;

        // 10 saniye sonra bu engeli yok et (bellek tasarrufu)
        Destroy(obstacle, 10f);
    }
}
