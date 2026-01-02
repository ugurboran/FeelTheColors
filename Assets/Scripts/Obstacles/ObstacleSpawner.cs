// ObstacleSpawner.cs - ZORLUK ARTIŞI İLE GÜNCELLENMİŞ
using UnityEngine;
using System.Collections; // Coroutine için

public class ObstacleSpawner : MonoBehaviour
{
    // Inspector'da atanacak engel prefab'ı
    public GameObject obstaclePrefab;

    // Kullanılabilir renkler dizisi (Inspector'da ayarlanacak)
    public Color[] possibleColors;

    // Başlangıç spawn aralığı
    public float initialSpawnInterval = 2f;

    // Başlangıç engel hızı
    public float initialObstacleSpeed = 3f;

    // Engellerin spawn olabileceği en düşük Y pozisyonu
    public float minY = -3f;

    // Engellerin spawn olabileceği en yüksek Y pozisyonu
    public float maxY = 3f;

    [Header("Bildirim Paneli")]
    public GameObject notificationPanel; // Bildirim paneli (opsiyonel)

    // Zorluk artış ayarları
    [Header("Zorluk Artışı Ayarları")]
    public int scoreIntervalForSpeed = 5;        // Her 5 puanda hız artar
    public float speedIncrement = 0.5f;          // Hız artış miktarı
    public float maxSpeed = 8f;                  // Maksimum hız

    public int scoreIntervalForSpawnRate = 10;   // Her 10 puanda spawn aralığı azalır
    public float spawnRateDecrement = 0.1f;      // Spawn aralığı azalma miktarı
    public float minSpawnInterval = 0.8f;        // Minimum spawn aralığı

    // Şu anki değerler
    private float currentSpawnInterval;
    private float currentObstacleSpeed;

    // Zamanlayıcı değişkeni (spawn aralığını kontrol eder)
    private float timer = 0f;

    // Son zorluk artışı yapılan skor
    private int lastSpeedIncreaseScore = 0;
    private int lastSpawnRateDecreaseScore = 0;

    // Başlangıçta çalışır
    void Start()
    {
        // Başlangıç değerlerini ayarla
        currentSpawnInterval = initialSpawnInterval;
        currentObstacleSpeed = initialObstacleSpeed;
    }

    // Her frame'de çalışır
    void Update()
    {
        // Zamanlayıcıyı artır (geçen süreyi ekle)
        timer += Time.deltaTime;

        // Zamanlayıcı spawn aralığına ulaştı mı?
        if (timer >= currentSpawnInterval)
        {
            // Yeni engel oluştur
            SpawnObstacle();

            // Zamanlayıcıyı sıfırla
            timer = 0f;
        }

        // Zorluk artışını kontrol et
        UpdateDifficulty();
    }

    // Zorluk seviyesini güncelle
    void UpdateDifficulty()
    {
        if (GameManager.Instance == null) return;

        int currentScore = GameManager.Instance.GetCurrentScore();

        // HIZ ARTIŞI
        if (currentScore >= lastSpeedIncreaseScore + scoreIntervalForSpeed)
        {
            if (currentObstacleSpeed < maxSpeed)
            {
                currentObstacleSpeed += speedIncrement;

                if (currentObstacleSpeed > maxSpeed)
                {
                    currentObstacleSpeed = maxSpeed;
                }

                lastSpeedIncreaseScore = currentScore;

                // Bildirim göster
                ShowDifficultyNotification();

                Debug.Log("Hız arttı! Yeni hız: " + currentObstacleSpeed);
            }
        }

        // SPAWN ARALIĞI AZALTMA
        if (currentScore >= lastSpawnRateDecreaseScore + scoreIntervalForSpawnRate)
        {
            if (currentSpawnInterval > minSpawnInterval)
            {
                currentSpawnInterval -= spawnRateDecrement;

                if (currentSpawnInterval < minSpawnInterval)
                {
                    currentSpawnInterval = minSpawnInterval;
                }

                lastSpawnRateDecreaseScore = currentScore;

                // Bildirim göster
                ShowDifficultyNotification();

                Debug.Log("Spawn aralığı azaldı! Yeni aralık: " + currentSpawnInterval);
            }
        }
    }

    // Zorluk artışı bildirimini göster
    void ShowDifficultyNotification()
    {
        if (notificationPanel != null)
        {
            StartCoroutine(ShowNotificationCoroutine());
        }
    }

    // Bildirimi göster ve 2 saniye sonra gizle
    IEnumerator ShowNotificationCoroutine()
    {
        // Paneli göster
        notificationPanel.SetActive(true);

        // 2 saniye bekle (real time - pause etkilemesin)
        yield return new WaitForSecondsRealtime(2f);

        // Paneli gizle
        notificationPanel.SetActive(false);
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

        // Engele SAĞDAN SOLA hareket hızı ver (mevcut hız ile)
        rb.linearVelocity = Vector2.left * currentObstacleSpeed;

        // 10 saniye sonra bu engeli yok et (bellek tasarrufu)
        Destroy(obstacle, 10f);
    }
}