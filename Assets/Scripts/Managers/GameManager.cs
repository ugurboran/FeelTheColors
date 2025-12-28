// GameManager.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton pattern - Tüm scriptlerden erişilebilir tek instance
    public static GameManager Instance;

    // Skoru gösterecek UI Text bileşeni
    public Text scoreText;

    // Oyun bitince gösterilecek panel
    public GameObject gameOverPanel;

    // Oyuncunun puanı
    private int score = 0;

    // Obje oluşturulmadan önce çalışır
    void Awake()
    {
        // Bu objeyi Instance olarak kaydet (singleton)
        Instance = this;
    }

    // Puan ekle (Obstacle tarafından çağrılır)
    public void AddScore()
    {
        // Skoru 1 artır
        score++;

        // UI'daki text'i güncelle
        scoreText.text = "Skor: " + score;
    }

    // Oyunu bitir (Obstacle tarafından çağrılır)
    public void GameOver()
    {
        // Oyunu durdur (zamanı dondur)
        Time.timeScale = 0;

        // Game Over panelini göster
        gameOverPanel.SetActive(true);
    }

    // Oyunu yeniden başlat (UI Button tarafından çağrılır)
    public void RestartGame()
    {
        // Zamanı normale döndür
        Time.timeScale = 1;

        // Şu anki sahneyi yeniden yükle (oyunu baştan başlat)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}