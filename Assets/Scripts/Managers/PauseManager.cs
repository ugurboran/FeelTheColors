// PauseManager.cs - YENİ INPUT SYSTEM İÇİN
using UnityEngine;
using UnityEngine.InputSystem; // YENİ SATIR

public class PauseManager : MonoBehaviour
{
    // Pause paneli (Inspector'da atanacak)
    public GameObject pausePanel;

    // Settings paneli (opsiyonel - pause içinde ayarlar için)
    //public GameObject settingsPanel;

    // Oyun duraklatıldı mı
    private bool isPaused = false;

    // Her frame çalışır
    void Update()
    {
        // ESC tuşuna basıldığında pause/resume (PC test için) - YENİ SİSTEM
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Oyunu duraklat - PauseButton çağırır
    public void PauseGame()
    {
        // Oyun duraklatıldı olarak işaretle
        isPaused = true;

        // Zamanı durdur
        Time.timeScale = 0f;

        // Pause panelini göster
        pausePanel.SetActive(true);
    }

    // Oyuna devam et - ResumeButton çağırır
    public void ResumeGame()
    {
        // Oyun devam ediyor olarak işaretle
        isPaused = false;

        // Zamanı normale döndür
        Time.timeScale = 1f;

        // Pause panelini gizle
        pausePanel.SetActive(false);

        // Settings paneli açıksa onu da kapat
        /*
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }
        */
    }

    // Settings panelini aç (pause içinde)
    /*
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    // Settings panelini kapat
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    */

    // Oyun duraklatılmış mı kontrol et (dışarıdan erişim için)
    public bool IsPaused()
    {
        return isPaused;
    }
}