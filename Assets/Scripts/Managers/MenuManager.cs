// MenuManager.cs - MÜZİK EKLENMİŞ VERSİYON
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections; // IEnumerator için gerekli

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Toggle soundToggle;
    public TextMeshProUGUI highScoreDisplay;

    void Start()
    {
        // Coroutine ile müzik başlatma
        StartCoroutine(InitializeMenu());
    }

    // Menü başlatma - AudioManager'ı bekler
    IEnumerator InitializeMenu()
    {
        // Bir frame bekle (AudioManager yüklensin)
        yield return null;

        // Toggle durumunu güncelle
        UpdateToggleState();

        // En yüksek skoru göster
        UpdateHighScoreDisplay();

        // Menü müziğini başlat
        if (AudioManager.Instance != null)
        {
            Debug.Log("🎵 Menü müziği başlatılıyor");
            AudioManager.Instance.PlayMenuMusic();
        }
        else
        {
            Debug.LogError("❌ AudioManager bulunamadı!");
        }
    }

    // En yüksek skoru ekranda göster
    void UpdateHighScoreDisplay()
    {
        // PlayerPrefs'ten en yüksek skoru al
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Text'i güncelle
        highScoreDisplay.text = "High Score: " + highScore;
    }

    public void OpenSettings()
    {
        // BUTON SESİ ÇAL
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSound();
        }

        settingsPanel.SetActive(true);
        UpdateToggleState();
    }

    void UpdateToggleState()
    {
        if (AudioManager.Instance != null)
        {
            soundToggle.onValueChanged.RemoveAllListeners();
            soundToggle.isOn = AudioManager.Instance.IsSoundOn();
            soundToggle.onValueChanged.AddListener(AudioManager.Instance.ToggleSound);
        }
    }

    public void PlayGame()
    {
        // BUTON SESİ ÇAL
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSound();
        }

        SceneManager.LoadScene("GameScene");
    }

    public void CloseSettings()
    {
        // BUTON SESİ ÇAL
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSound();
        }

        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        // BUTON SESİ ÇAL
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSound();
        }

        Debug.Log("Oyun kapatılıyor");
        Application.Quit();
    }
}