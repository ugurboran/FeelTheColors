// AudioManager.cs - MÜZİK TOGGLE EKLENMİŞ VERSİYON
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton - her yerden erişilebilir
    public static AudioManager Instance;

    // Ses açık mı kapalı mı
    private bool isSoundOn = true;

    // Müzik açık mı kapalı mı - YENİ
    private bool isMusicOn = true;

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

        // Kaydedilmiş ses ayarlarını yükle (1 = açık, 0 = kapalı)
        isSoundOn = PlayerPrefs.GetInt("Sound", 1) == 1;
        isMusicOn = PlayerPrefs.GetInt("Music", 1) == 1; // YENİ

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
        // Ses efektleri volume
        if (sfxSource != null)
        {
            sfxSource.volume = isSoundOn ? 1f : 0f;
        }

        // Müzik volume - YENİ
        if (musicSource != null)
        {
            musicSource.volume = isMusicOn ? 0.5f : 0f;
        }
    }

    // Ses efektlerini aç/kapat - Toggle çağırır
    public void ToggleSound(bool isOn)
    {
        isSoundOn = isOn;

        // Ses efektleri seviyesini ayarla
        if (sfxSource != null)
        {
            sfxSource.volume = isOn ? 1f : 0f;
        }

        // Ayarı kaydet
        PlayerPrefs.SetInt("Sound", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Müziği aç/kapat - YENİ
    public void ToggleMusic(bool isOn)
    {
        isMusicOn = isOn;

        if (musicSource != null)
        {
            musicSource.volume = isOn ? 0.5f : 0f;

            // Eğer kapatılıyorsa müziği durdur
            if (!isOn && musicSource.isPlaying)
            {
                musicSource.Pause();
            }
            // Eğer açılıyorsa ve durdurulmuşsa devam ettir
            else if (isOn && !musicSource.isPlaying && musicSource.clip != null)
            {
                musicSource.Play();
            }
        }

        // Ayarı kaydet
        PlayerPrefs.SetInt("Music", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Sesin durumunu kontrol et
    public bool IsSoundOn()
    {
        return isSoundOn;
    }

    // Müziğin durumunu kontrol et - YENİ
    public bool IsMusicOn()
    {
        return isMusicOn;
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

            // Müzik açıksa çal - DEĞİŞTİRİLDİ
            if (isMusicOn)
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