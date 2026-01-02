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
}// AudioManager.cs - GENİŞLETİLMİŞ VERSİYON
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton - her yerden erişilebilir
    public static AudioManager Instance;

    // Ses açık mı kapalı mı
    private bool isSoundOn = true;

    // Audio Source bileşenleri
    private AudioSource musicSource;     // Müzik için
    private AudioSource sfxSource;       // Ses efektleri için

    // Ses efektleri (Inspector'da atanacak)
    [Header("Ses Efektleri")]
    public AudioClip colorChangeSound;   // Renk değişimi
    public AudioClip scoreSound;         // Puan kazanma
    public AudioClip gameOverSound;      // Oyun bitişi
    public AudioClip buttonClickSound;   // Buton tıklama

    // Müzikler (Inspector'da atanacak)
    [Header("Müzikler")]
    public AudioClip menuMusic;          // Ana menü müziği
    public AudioClip gameMusic;          // Oyun müziği

    // Oyun başlamadan önce çalışır
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            // Sahne değişse bile yok olmasın
            DontDestroyOnLoad(gameObject);

            // Audio Source bileşenlerini oluştur
            SetupAudioSources();
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
        UpdateVolume();
    }

    // Audio Source bileşenlerini oluştur
    void SetupAudioSources()
    {
        // Müzik için Audio Source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;      // Müzik döngüde çalsın
        musicSource.playOnAwake = false;
        musicSource.volume = 0.5f;    // Müzik biraz daha sessiz

        // Ses efektleri için Audio Source
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = 1f;
    }

    // Ses seviyesini güncelle
    void UpdateVolume()
    {
        float volume = isSoundOn ? 1f : 0f;

        if (musicSource != null)
        {
            musicSource.volume = isSoundOn ? 0.5f : 0f;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
    }

    // Sesi aç/kapat - Toggle çağırır
    public void ToggleSound(bool isOn)
    {
        isSoundOn = isOn;

        // Ses seviyesini ayarla
        UpdateVolume();

        // Ayarı kaydet
        PlayerPrefs.SetInt("Sound", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Sesin durumunu kontrol et
    public bool IsSoundOn()
    {
        return isSoundOn;
    }

    // Ses efekti çal
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && isSoundOn && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Müzik çal
    public void PlayMusic(AudioClip clip)
    {
        if (clip != null && musicSource != null)
        {
            // Eğer aynı müzik çalıyorsa değiştirme
            if (musicSource.clip == clip && musicSource.isPlaying)
                return;

            musicSource.clip = clip;

            if (isSoundOn)
            {
                musicSource.Play();
            }
        }
    }

    // Müziği durdur
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    // --- KOLAYLIK FONKSİYONLARI ---

    // Renk değişimi sesi çal
    public void PlayColorChangeSound()
    {
        PlaySFX(colorChangeSound);
    }

    // Puan sesi çal
    public void PlayScoreSound()
    {
        PlaySFX(scoreSound);
    }

    // Game Over sesi çal
    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSound);
    }

    // Buton tıklama sesi çal
    public void PlayButtonClickSound()
    {
        PlaySFX(buttonClickSound);
    }

    // Menü müziği başlat
    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    // Oyun müziği başlat
    public void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }
}