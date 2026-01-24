// ObstacleSpawner.cs - OBJECT POOLING + COROUTİNE YÖNETİMİ
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    // === TEMEL AYARLAR ===

    [Header("Temel Ayarlar")]
    // Kullanılabilir renkler dizisi (Inspector'da ayarlanacak)
    public Color[] possibleColors;

    // Başlangıç spawn aralığı (saniye)
    public float initialSpawnInterval = 2.5f;

    // Başlangıç engel hızı
    public float initialObstacleSpeed = 2f;

    // Engellerin spawn olabileceği en düşük Y pozisyonu
    public float minY = -3f;

    // Engellerin spawn olabileceği en yüksek Y pozisyonu
    public float maxY = 3f;

    // === OBJECT POOLING ===

    [Header("Object Pool")]
    // Object pool referansı (havuzdan obstacle almak için)
    public ObstaclePool obstaclePool;

    // === BİLDİRİM PANELİ ===

    [Header("Bildirim Paneli")]
    // Zorluk artışı bildirimi gösterecek panel (opsiyonel)
    public GameObject notificationPanel;

    // === ZORLUK ARTIŞI AYARLARI ===

    [Header("Zorluk Artışı Ayarları")]
    // Her kaç puanda bir hız artacak
    public int scoreIntervalForSpeed = 5;

    // Her artışta ne kadar hız eklenecek
    public float speedIncrement = 0.5f;

    // Maksimum ulaşılabilir hız
    public float maxSpeed = 8f;

    // Her kaç puanda bir spawn aralığı azalacak
    public int scoreIntervalForSpawnRate = 10;

    // Her azaltmada ne kadar azalacak
    public float spawnRateDecrement = 0.1f;

    // Minimum spawn aralığı (daha aşağı inmez)
    public float minSpawnInterval = 0.8f;

    // === PRIVATE DEĞİŞKENLER ===

    // Şu anki spawn aralığı
    private float currentSpawnInterval;

    // Şu anki engel hızı
    private float currentObstacleSpeed;

    // Zamanlayıcı değişkeni (spawn aralığını kontrol eder)
    private float timer = 0f;

    // Son zorluk artışı yapılan skor değerleri
    private int lastSpeedIncreaseScore = 0;
    private int lastSpawnRateDecreaseScore = 0;

    // Aktif coroutine'leri takip eden dictionary
    // Her obstacle için sadece 1 coroutine olmasını garanti eder
    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

    // === UNITY FONKSIYONLARI ===

    // Başlangıçta bir kez çalışır
    void Start()
    {
        // Başlangıç değerlerini ayarla
        currentSpawnInterval = initialSpawnInterval;
        currentObstacleSpeed = initialObstacleSpeed;

        // Pool referansı kontrolü
        if (obstaclePool == null)
        {
            Debug.LogError("❌ ObstaclePool atanmamış!");
        }
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

    // Obje yok edildiğinde çalışır (scene değişiminde)
    void OnDestroy()
    {
        // Tüm coroutine'leri durdur
        StopAllCoroutines();

        // Dictionary'yi temizle
        activeCoroutines.Clear();
    }

    // === ZORLUK SİSTEMİ ===

    // Zorluk seviyesini günceller
    void UpdateDifficulty()
    {
        // GameManager yoksa çık
        if (GameManager.Instance == null) return;

        // Şu anki skoru al
        int currentScore = GameManager.Instance.GetCurrentScore();

        // === HIZ ARTIŞI ===
        // Skor belirli aralığa ulaştı mı ve maksimum hıza ulaşmadık mı?
        if (currentScore >= lastSpeedIncreaseScore + scoreIntervalForSpeed)
        {
            if (currentObstacleSpeed < maxSpeed)
            {
                // Hızı artır
                currentObstacleSpeed += speedIncrement;

                // Maksimum hızı aşma kontrolü
                if (currentObstacleSpeed > maxSpeed)
                {
                    currentObstacleSpeed = maxSpeed;
                }

                // Son artış skorunu güncelle
                lastSpeedIncreaseScore = currentScore;

                // Bildirim göster
                ShowDifficultyNotification();

                Debug.Log("⚡ Hız arttı! Yeni hız: " + currentObstacleSpeed);
            }
        }

        // === SPAWN ARALIĞI AZALTMA ===
        // Skor belirli aralığa ulaştı mı ve minimum aralığa ulaşmadık mı?
        if (currentScore >= lastSpawnRateDecreaseScore + scoreIntervalForSpawnRate)
        {
            if (currentSpawnInterval > minSpawnInterval)
            {
                // Spawn aralığını azalt (daha sık spawn olsun)
                currentSpawnInterval -= spawnRateDecrement;

                // Minimum aralık kontrolü
                if (currentSpawnInterval < minSpawnInterval)
                {
                    currentSpawnInterval = minSpawnInterval;
                }

                // Son azaltma skorunu güncelle
                lastSpawnRateDecreaseScore = currentScore;

                // Bildirim göster
                ShowDifficultyNotification();

                Debug.Log("⚡ Spawn aralığı azaldı! Yeni aralık: " + currentSpawnInterval);
            }
        }
    }

    // === BİLDİRİM SİSTEMİ ===

    // Zorluk artışı bildirimini göster
    void ShowDifficultyNotification()
    {
        // Panel atanmışsa bildirimi göster
        if (notificationPanel != null)
        {
            StartCoroutine(ShowNotificationCoroutine());
        }
    }

    // Bildirimi göster ve 2 saniye sonra gizle
    IEnumerator ShowNotificationCoroutine()
    {
        // Paneli aktif et
        notificationPanel.SetActive(true);

        // 2 saniye bekle (oyun duraklatılmış olsa bile bekle)
        yield return new WaitForSecondsRealtime(2f);

        // Paneli gizle
        notificationPanel.SetActive(false);
    }

    // === ENGEL OLUŞTURMA SİSTEMİ ===

    // Yeni bir engel oluşturur (object pooling ile)
    void SpawnObstacle()
    {
        // Havuzdan obstacle al
        GameObject obstacle = obstaclePool.GetObstacle();

        // NULL kontrolü (güvenlik)
        if (obstacle == null)
        {
            Debug.LogError("❌ Pool'dan null obstacle döndü!");
            return;
        }

        // === POZİSYON AYARLARI ===

        // MinY ve MaxY arasında rastgele bir Y pozisyonu seç
        float randomY = Random.Range(minY, maxY);

        // Spawn pozisyonunu oluştur (X: spawner'ın pozisyonu, Y: rastgele)
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0);
        obstacle.transform.position = spawnPosition;
        obstacle.transform.rotation = Quaternion.identity;

        // === RENK AYARLARI ===

        // Mevcut renklerden rastgele birini seç
        Color randomColor = possibleColors[Random.Range(0, possibleColors.Length)];

        // Obstacle script'ine rengi ayarla
        Obstacle obstacleScript = obstacle.GetComponent<Obstacle>();
        obstacleScript.lineColor = randomColor;

        // Sprite renderer'a rengi uygula
        SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
        sr.color = randomColor;

        // === HIZ AYARLARI ===

        // Engelin Rigidbody2D bileşenini al
        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();

        // Engele SOLA (Vector2.left) doğru hız ver
        rb.linearVelocity = Vector2.left * currentObstacleSpeed;

        // === COROUTİNE YÖNETİMİ ===

        // Bu obstacle için daha önceden çalışan coroutine var mı?
        if (activeCoroutines.ContainsKey(obstacle))
        {
            // Eski coroutine'i durdur (duplicate engellemek için)
            StopCoroutine(activeCoroutines[obstacle]);
            activeCoroutines.Remove(obstacle);
        }

        // Yeni coroutine başlat (10 saniye sonra havuza geri dön)
        Coroutine returnCoroutine = StartCoroutine(ReturnObstacleAfterDelay(obstacle, 10f));

        // Coroutine'i dictionary'de sakla
        activeCoroutines[obstacle] = returnCoroutine;
    }

    // Belirli süre sonra obstacle'ı havuza geri döndür
    IEnumerator ReturnObstacleAfterDelay(GameObject obstacle, float delay)
    {
        // Belirtilen süre kadar bekle
        yield return new WaitForSeconds(delay);

        // NULL ve aktiflik kontrolü
        // (Oyun bitmiş veya obstacle collision sonrası döndürülmüş olabilir)
        if (obstacle != null && obstacle.activeInHierarchy)
        {
            // Obstacle'ı havuza geri ver
            obstaclePool.ReturnObstacle(obstacle);
        }

        // Coroutine dictionary'den kaldır (temizlik)
        if (obstacle != null && activeCoroutines.ContainsKey(obstacle))
        {
            activeCoroutines.Remove(obstacle);
        }
    }
}