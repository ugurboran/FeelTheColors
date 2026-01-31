// ThemeManager.cs - PARALLAX BACKGROUND TEMA YÖNETİCİSİ
using NUnit.Framework;
using System.Threading.Tasks;
using TreeEditor;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement; // YENİ
using static ThemeManager;

public class ThemeManager : MonoBehaviour
{
    // Singleton - Tek instance olmasını garanti eder
    public static ThemeManager Instance;

    [Header("Tema Ayarları")]
    // Tüm temalar burada tanımlanır
    public Theme[] themes;

    [Header("Referanslar")]
    // Ana kamera referansı (fallback için)
    public Camera mainCamera;

    // Şu anki seçili tema index'i
    private int currentThemeIndex = 0;

    // Şu anki instantiate edilmiş background objesi
    private GameObject currentBackgroundInstance;

    // Tema bilgilerini tutan class
    [System.Serializable]
    public class Theme
    {
        [Header("Temel Bilgiler")]
        public string themeName;              // Tema adı (örn: "Okyanus")

        [Header("Görsel Ayarları")]
        public Color backgroundColor;          // Fallback kamera rengi (prefab yoksa)
        public GameObject backgroundPrefab;    // Parallax background prefab'ı ✨

        [Header("Renk Paleti")]
        public Color[] obstacleColors;         // Bu temadaki engel renkleri
        public Color playerColor;              // Oyuncu top rengi (opsiyonel)

        [Header("Kilit Sistemi")]
        public bool isUnlocked = true;         // Tema açık mı?
        public int unlockScore = 0;            // Açmak için gereken skor
    }

    // Unity yaşam döngüsü - Awake ilk çalışan fonksiyon
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Scene değişse bile yok olma
        }
        else
        {
            Destroy(gameObject); // Duplicate varsa yok et
            return;
        }

        // Kaydedilmiş temayı yükle
        LoadTheme();

        SceneManager.sceneLoaded += OnSceneLoaded; // Scene yüklenince çağrılır
    }

    // Oyun başladığında çalışır
    void Start()
    {
        // Başlangıçta temayı uygula
        ApplyTheme(currentThemeIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainCamera = Camera.main; // Yeni kamerayı bul
        ApplyTheme(currentThemeIndex); // Temayı uygula
    }

    // Belirli bir temayı uygula
    public void ApplyTheme(int themeIndex)
    {
        // Geçerli index kontrolü
        if (themeIndex < 0 || themeIndex >= themes.Length)
        {
            Debug.LogError("❌ Geçersiz tema index: " + themeIndex);
            return;
        }

        // Tema kilit kontrolü
        if (!themes[themeIndex].isUnlocked)
        {
            Debug.LogWarning($"🔒 {themes[themeIndex].themeName} teması kilitli!");
            return;
        }

        // Eski arka planı yok et
        if (currentBackgroundInstance != null)
        {
            Destroy(currentBackgroundInstance);
            Debug.Log("🗑️ Eski background silindi");
        }

        // Index'i güncelle
        currentThemeIndex = themeIndex;
        Theme theme = themes[themeIndex];

        // === PARALLAX BACKGROUND UYGULA ===
        if (theme.backgroundPrefab != null)
        {
            // Yeni parallax background'ı instantiate et
            currentBackgroundInstance = Instantiate(theme.backgroundPrefab);
            currentBackgroundInstance.transform.position = new Vector3(0, 0, -10);

            Debug.Log($"🌊 Parallax background oluşturuldu: {theme.themeName}");

            // Kamera rengini koyu yap (background görünsün)
            if (mainCamera != null)
            {
                mainCamera.backgroundColor = Color.black;
            }
        }
        else
        {
            // Prefab yoksa fallback olarak kamera rengini kullan
            Debug.LogWarning($"⚠️ {theme.themeName} için background prefab yok, solid color kullanılıyor");

            if (mainCamera != null)
            {
                mainCamera.backgroundColor = theme.backgroundColor;
            }
        }

        // === OBSTACLE RENKLERİNİ GÜNCELLE ===
        ObstacleSpawner spawner = FindObjectOfType<ObstacleSpawner>();
        if (spawner != null)
        {
            spawner.possibleColors = theme.obstacleColors;
            Debug.Log($"🎨 Obstacle renkleri güncellendi ({theme.obstacleColors.Length} renk)");
        }
        else
        {
            Debug.LogWarning("⚠️ ObstacleSpawner bulunamadı!");
        }

        Debug.Log($"✅ Tema uygulandı: {theme.themeName}");

        // Temayı kaydet
        SaveTheme();
    }

    // Sonraki temaya geç
    public void NextTheme()
    {
        int nextIndex = (currentThemeIndex + 1) % themes.Length;

        // Kilit kontrolü
        if (!themes[nextIndex].isUnlocked)
        {
            Debug.Log($"🔒 {themes[nextIndex].themeName} teması kilitli! Gereken skor: {themes[nextIndex].unlockScore}");
            return;
        }

        ApplyTheme(nextIndex);
    }

    // Önceki temaya geç
    public void PreviousTheme()
    {
        int prevIndex = (currentThemeIndex - 1 + themes.Length) % themes.Length;

        // Kilit kontrolü
        if (!themes[prevIndex].isUnlocked)
        {
            Debug.Log($"🔒 {themes[prevIndex].themeName} teması kilitli! Gereken skor: {themes[prevIndex].unlockScore}");
            return;
        }

        ApplyTheme(prevIndex);
    }

    // Belirli bir temayı aç
    public void UnlockTheme(int themeIndex)
    {
        if (themeIndex >= 0 && themeIndex < themes.Length)
        {
            if (!themes[themeIndex].isUnlocked)
            {
                themes[themeIndex].isUnlocked = true;
                PlayerPrefs.SetInt($"Theme_{themeIndex}_Unlocked", 1);
                PlayerPrefs.Save();

                Debug.Log($"🔓 {themes[themeIndex].themeName} teması açıldı!");

                // TODO: Unlock bildirim UI'ı göster (opsiyonel)
            }
        }
    }

    // Şu anki temayı döndür
    public Theme GetCurrentTheme()
    {
        if (currentThemeIndex >= 0 && currentThemeIndex < themes.Length)
        {
            return themes[currentThemeIndex];
        }

        Debug.LogError("❌ Geçersiz tema index!");
        return themes[0]; // Fallback - ilk tema
    }

    // Şu anki tema index'ini döndür
    public int GetCurrentThemeIndex()
    {
        return currentThemeIndex;
    }

    // Tüm temaları döndür (UI için)
    public Theme[] GetAllThemes()
    {
        return themes;
    }

    // Temayı PlayerPrefs'e kaydet
    void SaveTheme()
    {
        PlayerPrefs.SetInt("CurrentTheme", currentThemeIndex);
        PlayerPrefs.Save();

        Debug.Log($"💾 Tema kaydedildi: {currentThemeIndex}");
    }

    // Temayı PlayerPrefs'ten yükle
    void LoadTheme()
    {
        // Son seçili temayı yükle
        currentThemeIndex = PlayerPrefs.GetInt("CurrentTheme", 0);

        // Tema sayısı kontrolü (hata önleme)
        if (currentThemeIndex >= themes.Length)
        {
            currentThemeIndex = 0;
        }

        // Kilit durumlarını yükle
        for (int i = 0; i < themes.Length; i++)
        {
            // İlk tema her zaman açık, diğerleri kaydedilmiş duruma göre
            bool isUnlocked = PlayerPrefs.GetInt($"Theme_{i}_Unlocked", i == 0 ? 1 : 0) == 1;
            themes[i].isUnlocked = isUnlocked;
        }

        Debug.Log($"📂 Temalar yüklendi. Seçili tema: {currentThemeIndex}");
    }

    // Skora göre temaları otomatik aç
    public void CheckUnlocks(int currentScore)
    {
        bool anyUnlocked = false;

        for (int i = 0; i < themes.Length; i++)
        {
            // Henüz açılmamış ve skor yeterli mi?
            if (!themes[i].isUnlocked && currentScore >= themes[i].unlockScore)
            {
                UnlockTheme(i);
                anyUnlocked = true;
            }
        }

        if (anyUnlocked)
        {
            // TODO: "Yeni tema açıldı!" bildirimi göster (opsiyonel)
        }
    }

    // Obje yok edildiğinde çalışır
    void OnDestroy()
    {
        // Mevcut background'ı temizle
        if (currentBackgroundInstance != null)
        {
            Destroy(currentBackgroundInstance);
        }
    }
}