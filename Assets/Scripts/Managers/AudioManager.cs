// AudioManager.cs
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton - her yerden erişilebilir
    public static AudioManager Instance;

    // Ses açık mı kapalı mı
    private bool isSoundOn = true;

    // Oyun başlamadan önce çalışır
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            // Sahne değişse bile yok olmasın
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Zaten var, bu objeyi yok et
            Destroy(gameObject);
            return;
        }

        // Kaydedilmiş ses ayarını yükle (1 = açık, 0 = kapalı)
        isSoundOn = PlayerPrefs.GetInt("Sound", 1) == 1;

        // Ses seviyesini ayarla
        AudioListener.volume = isSoundOn ? 1f : 0f;
    }

    // Sesi aç/kapat - Toggle çağırır
    public void ToggleSound(bool isOn)
    {
        isSoundOn = isOn;

        // Ses seviyesini ayarla
        AudioListener.volume = isOn ? 1f : 0f;

        // Ayarı kaydet
        PlayerPrefs.SetInt("Sound", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Sesin durumunu kontrol et
    public bool IsSoundOn()
    {
        return isSoundOn;
    }
}