// MenuManager.cs - GÜNCELLENMİŞ
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // YENİ - TextMeshPro için

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Toggle soundToggle;
    public TextMeshProUGUI highScoreDisplay; // YENİ - En yüksek skor text'i

    void Start()
    {
        // Toggle durumunu güncelle
        UpdateToggleState();

        // En yüksek skoru göster
        UpdateHighScoreDisplay();

        // Menü müziğini başlat
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuMusic();
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
        SceneManager.LoadScene("GameScene");
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Oyun kapatılıyor");
        Application.Quit();
    }
}