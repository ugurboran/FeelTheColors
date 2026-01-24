// ObstaclePool.cs - NULL CHECK İLE GÜNCELLENMİŞ
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [Header("Pool Ayarları")]
    public GameObject obstaclePrefab;
    public int poolSize = 15;
    public Transform poolParent;

    private List<GameObject> pool;

    void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = CreateNewObstacle();
            pool.Add(obj);
        }

        Debug.Log($"✅ Object Pool oluşturuldu: {poolSize} obstacle");
    }

    GameObject CreateNewObstacle()
    {
        GameObject obj = Instantiate(obstaclePrefab);

        if (poolParent != null)
        {
            obj.transform.SetParent(poolParent);
        }

        Obstacle obstacleScript = obj.GetComponent<Obstacle>();
        if (obstacleScript != null)
        {
            obstacleScript.SetPool(this);
        }

        obj.SetActive(false);
        return obj;
    }

    public GameObject GetObstacle()
    {
        // NULL CHECK EKLE - YENİ! ✨
        // Önce null objeleri temizle
        pool.RemoveAll(item => item == null);

        // Havuzda kullanılabilir obstacle ara
        foreach (GameObject obj in pool)
        {
            // NULL CHECK - YENİ! ✨
            if (obj != null && !obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Havuzda yer yok, yeni obstacle oluştur
        Debug.LogWarning("⚠️ Pool doldu! Yeni obstacle ekleniyor...");
        GameObject newObj = CreateNewObstacle();
        pool.Add(newObj);
        newObj.SetActive(true);
        return newObj;
    }

    public void ReturnObstacle(GameObject obj)
    {
        // NULL CHECK - YENİ! ✨
        if (obj == null)
        {
            Debug.LogWarning("⚠️ Null obstacle döndürülmeye çalışıldı!");
            return;
        }

        obj.SetActive(false);

        // Rigidbody'yi sıfırla
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void ReturnAllObstacles()
    {
        // NULL CHECK ile temizle - YENİ! ✨
        pool.RemoveAll(item => item == null);

        foreach (GameObject obj in pool)
        {
            if (obj != null && obj.activeInHierarchy)
            {
                ReturnObstacle(obj);
            }
        }
    }

    // Debug
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            int active = 0;
            int inactive = 0;
            int nullCount = 0; // NULL sayısı - YENİ!

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

            Debug.Log($"📊 Pool: Aktif={active}, İnaktif={inactive}, Null={nullCount}, Toplam={pool.Count}");
        }
    }
}