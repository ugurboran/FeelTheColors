// GameManager.cs - GÜNCELLENMİŞ VERSİYON
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton pattern - Tüm scriptlerden erişilebilir tek instance
    public static GameManager Instance;

    // Skoru gösterecek UI Text bileşeni
    public TextMeshProUGUI scoreText;

    // Oyun bitince gösterilecek panel
    public GameObject gameOverPanel;

    // Game Over panelindeki text'ler
    public TextMeshProUGUI finalScoreText;      // YENİ - Mevcut skor
    public TextMeshProUGUI highScoreText;       // YENİ - En yüksek skor

    // Oyuncunun puanı
    private int score = 0;

    // En yüksek skor
    private int highScore = 0;

    // Obje oluşturulmadan önce çalışır
    void Awake()
    {
        // Bu objeyi Instance olarak kaydet (singleton)
        Instance = this;

        // En yüksek skoru yükle (PlayerPrefs'ten)
        // Eğer daha önce kaydedilmemişse varsayılan 0
        highScore = PlayerPrefs.GetInt("HighScore", 0);
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
        // En yüksek skoru kontrol et ve güncelle
        if (score > highScore)
        {
            highScore = score;
            // En yüksek skoru kaydet
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // Game Over panelindeki skorları güncelle
        finalScoreText.text = "Score: " + score;
        highScoreText.text = "High Score: " + highScore;

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

    // Ana menüye dön (Button tarafından çağrılır)
    public void LoadMainMenu()
    {
        // Zamanı normale döndür
        Time.timeScale = 1;

        // Ana menü sahnesini yükle
        SceneManager.LoadScene("MainScene");
    }

    // En yüksek skoru döndür (dışarıdan erişim için)
    public int GetHighScore()
    {
        return highScore;
    }
}