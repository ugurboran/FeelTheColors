// ObstaclePool.cs - OBJECT POOLING + YENİ INPUT SYSTEM
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // YENİ INPUT SYSTEM

public class ObstaclePool : MonoBehaviour
{
    [Header("Pool Ayarları")]
    // Havuzda oluşturulacak obstacle prefab'ı
    public GameObject obstaclePrefab;

    // Başlangıçta kaç adet obstacle oluşturulacak
    public int poolSize = 15;

    // Pool objeleri bu parent'ın altında organize edilir (opsiyonel)
    public Transform poolParent;

    // Obstacle havuzu (liste)
    private List<GameObject> pool;

    // Başlangıçta bir kez çalışır
    void Start()
    {
        InitializePool();
    }

    // Pool'u başlat (obstacle'ları önceden oluştur)
    void InitializePool()
    {
        pool = new List<GameObject>();

        // Belirtilen sayıda obstacle oluştur
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = CreateNewObstacle();
            pool.Add(obj);
        }

        Debug.Log($"✅ Object Pool oluşturuldu: {poolSize} obstacle");
    }

    // Yeni bir obstacle oluştur ve pool'a ekle
    GameObject CreateNewObstacle()
    {
        // Prefab'dan obstacle instantiate et
        GameObject obj = Instantiate(obstaclePrefab);

        // Parent ayarla (hiyerarşi düzenli olsun)
        if (poolParent != null)
        {
            obj.transform.SetParent(poolParent);
        }

        // Obstacle script'ine pool referansını ver
        Obstacle obstacleScript = obj.GetComponent<Obstacle>();
        if (obstacleScript != null)
        {
            obstacleScript.SetPool(this);
        }

        // Başlangıçta inaktif yap
        obj.SetActive(false);
        return obj;
    }

    // Havuzdan kullanılabilir bir obstacle al
    public GameObject GetObstacle()
    {
        // NULL objeleri temizle (güvenlik)
        pool.RemoveAll(item => item == null);

        // Havuzda inaktif (kullanılabilir) obstacle ara
        foreach (GameObject obj in pool)
        {
            if (obj != null && !obj.activeInHierarchy)
            {
                // Obstacle'ı aktif et ve döndür
                obj.SetActive(true);
                return obj;
            }
        }

        // Havuzda yer yok, dinamik olarak yeni obstacle ekle
        Debug.LogWarning("⚠️ Pool doldu! Yeni obstacle ekleniyor...");
        GameObject newObj = CreateNewObstacle();
        pool.Add(newObj);
        newObj.SetActive(true);
        return newObj;
    }

    // Obstacle'ı havuza geri döndür
    public void ReturnObstacle(GameObject obj)
    {
        // NULL kontrolü
        if (obj == null)
        {
            Debug.LogWarning("⚠️ Null obstacle döndürülmeye çalışıldı!");
            return;
        }

        // Obstacle'ı deaktif et
        obj.SetActive(false);

        // Rigidbody hızını sıfırla (temizlik)
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // Tüm aktif obstacle'ları havuza geri döndür (restart için)
    public void ReturnAllObstacles()
    {
        // NULL objeleri temizle
        pool.RemoveAll(item => item == null);

        // Tüm aktif obstacle'ları döndür
        foreach (GameObject obj in pool)
        {
            if (obj != null && obj.activeInHierarchy)
            {
                ReturnObstacle(obj);
            }
        }
    }

    // Debug - F tuşu ile pool durumunu göster
    void Update()
    {
        // YENİ INPUT SYSTEM - Keyboard kontrolü
        // Keyboard mevcut mu ve F tuşuna basıldı mı?
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            // Pool istatistiklerini topla
            int active = 0;      // Aktif (kullanımda) obstacle sayısı
            int inactive = 0;    // İnaktif (havuzda bekleyen) obstacle sayısı
            int nullCount = 0;   // NULL (silinmiş) obstacle sayısı

            foreach (GameObject obj in pool)
            {
                if (obj == null)
                {
                    nullCount++;
                }
                else if (obj.activeInHierarchy)
                {
                    active++;
                }
                else
                {
                    inactive++;
                }
            }

            // İstatistikleri konsola yazdır
            Debug.Log($"📊 Pool Durumu: Aktif={active}, İnaktif={inactive}, Null={nullCount}, Toplam={pool.Count}");
        }
    }
}