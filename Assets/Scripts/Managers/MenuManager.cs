// MenuManager.cs - GÜNCELLENMİŞ
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Toggle soundToggle;

    // Oyun başladığında çalışır
    void Start()
    {
        // Toggle durumunu güncelle
        UpdateToggleState();
    }

    // Ayarlar menüsü her açıldığında toggle durumunu güncelle
    public void OpenSettings()
    {
        // Settings panelini göster
        settingsPanel.SetActive(true);

        // Toggle durumunu güncelle
        UpdateToggleState();
    }

    // Toggle durumunu AudioManager'dan al ve güncelle
    void UpdateToggleState()
    {
        if (AudioManager.Instance != null)
        {
            // OnValueChanged event'ini tetiklememek için
            // önce listener'ı kaldır, sonra tekrar ekle
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