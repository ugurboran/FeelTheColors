// ObstacleSpawner.cs - OBJECT POOLING İLE GÜNCELLENMİŞ
using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Temel Ayarlar")]
    public Color[] possibleColors;

    public float initialSpawnInterval = 2.5f;
    public float initialObstacleSpeed = 2f;

    public float minY = -3f;
    public float maxY = 3f;

    [Header("Object Pool - YENİ! ✨")]
    public ObstaclePool obstaclePool; // Pool referansı

    [Header("Bildirim Paneli")]
    public GameObject notificationPanel;

    [Header("Zorluk Artışı Ayarları")]
    public int scoreIntervalForSpeed = 5;
    public float speedIncrement = 0.5f;
    public float maxSpeed = 8f;

    public int scoreIntervalForSpawnRate = 10;
    public float spawnRateDecrement = 0.1f;
    public float minSpawnInterval = 0.8f;

    private float currentSpawnInterval;
    private float currentObstacleSpeed;
    private float timer = 0f;

    private int lastSpeedIncreaseScore = 0;
    private int lastSpawnRateDecreaseScore = 0;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        currentObstacleSpeed = initialObstacleSpeed;

        if (obstaclePool == null)
        {
            Debug.LogError("❌ ObstaclePool atanmamış!");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }

        UpdateDifficulty();
    }

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
                ShowDifficultyNotification();

                Debug.Log("⚡ Hız arttı! Yeni hız: " + currentObstacleSpeed);
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
                ShowDifficultyNotification();

                Debug.Log("⚡ Spawn aralığı azaldı! Yeni aralık: " + currentSpawnInterval);
            }
        }
    }

    void ShowDifficultyNotification()
    {
        if (notificationPanel != null)
        {
            StartCoroutine(ShowNotificationCoroutine());
        }
    }

    IEnumerator ShowNotificationCoroutine()
    {
        notificationPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        notificationPanel.SetActive(false);
    }

    void SpawnObstacle()
    {
        // Havuzdan obstacle al - YENİ! ✨
        GameObject obstacle = obstaclePool.GetObstacle();

        // Pozisyon ayarla
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0);
        obstacle.transform.position = spawnPosition;
        obstacle.transform.rotation = Quaternion.identity;

        // Renk ayarla
        Color randomColor = possibleColors[Random.Range(0, possibleColors.Length)];

        Obstacle obstacleScript = obstacle.GetComponent<Obstacle>();
        obstacleScript.lineColor = randomColor;

        SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
        sr.color = randomColor;

        // Hız ver
        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.left * currentObstacleSpeed;

        // Otomatik geri dönüş - YENİ! ✨
        StartCoroutine(ReturnObstacleAfterDelay(obstacle, 10f));
    }

    // Obstacle'ı belirli süre sonra havuza geri ver - YENİ! ✨
    IEnumerator ReturnObstacleAfterDelay(GameObject obstacle, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Hala aktifse havuza geri ver
        if (obstacle != null && obstacle.activeInHierarchy)
        {
            obstaclePool.ReturnObstacle(obstacle);
        }
    }
}