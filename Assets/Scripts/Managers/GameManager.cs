// GameManager.cs - GÜNCELLENMİŞ VERSİYON
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton pattern - Tüm scriptlerden erişilebilir tek instance
    public static GameManager Instance;

    // Game Over durumunu kontrol için - YENİ
    public static bool IsGameOver { get; private set; } = false;

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

        // Oyun başladığında GameOver durumunu sıfırla - YENİ
        IsGameOver = false;
    }

    /*
    void Start()
    {
        // Müziği geciktirerek başlat
        Invoke("PlayGameMusic", 0.1f);

        // Oyun müziğini başlat
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameMusic();
        }
    }
    */


    // Puan ekle (Obstacle tarafından çağrılır)
    public void AddScore()
    {
        // Skoru 1 artır
        score++;

        // UI'daki text'i güncelle
        scoreText.text = "SCORE: " + score;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayScoreSound();
        }
    }

    // Oyunu bitir (Obstacle tarafından çağrılır)
    public void GameOver()
    {
        IsGameOver = true; // YENİ - Hemen işaretle

        // Explosion efektini çalıştır
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            ParticleSystem explosion = player.transform.Find("ExplosionParticles")?.GetComponent<ParticleSystem>();
            if (explosion != null)
            {
                explosion.Play();
            }

            // Üzgün yüz - YENİ! ✨
            FaceController face = player.GetComponentInChildren<FaceController>();
            if (face != null)
            {
                face.ShowSad();
            }

        }



        // En yüksek skoru kontrol et ve güncelle
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // Game Over panelindeki skorları güncelle
        finalScoreText.text = "Your score: " + score;
        highScoreText.text = "High Score: " + highScore;

        // Game Over sesi çal
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverSound();
        }

        // Oyunu durdur
        Time.timeScale = 0;

        // Game Over panelini göster
        gameOverPanel.SetActive(true);
    }

    // Oyunu yeniden başlat (UI Button tarafından çağrılır)
    public void RestartGame()
    {
        // BUTON SESİ ÇAL
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSound();
        }

        IsGameOver = false; // YENİ - Sıfırla

        // Zamanı normale döndür
        Time.timeScale = 1;

        // Şu anki sahneyi yeniden yükle (oyunu baştan başlat)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Ana menüye dön (Button tarafından çağrılır)
    public void LoadMainMenu()
    {
        // BUTON SESİ ÇAL
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSound();
        }

        IsGameOver = false; // YENİ - Sıfırla

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

    // Mevcut skoru döndür (dışarıdan erişim için)
    public int GetCurrentScore()
    {
        return score;
    }
}