// ObstaclePool.cs - OBJECT POOLING SİSTEMİ
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [Header("Pool Ayarları")]
    public GameObject obstaclePrefab;        // Obstacle prefab'ı
    public int poolSize = 10;                // Havuz boyutu
    public Transform poolParent;             // Pool parent objesi (opsiyonel)

    private List<GameObject> pool;           // Obstacle havuzu

    void Start()
    {
        InitializePool();
    }

    // Havuzu başlat
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

    // Yeni obstacle oluştur
    GameObject CreateNewObstacle()
    {
        GameObject obj = Instantiate(obstaclePrefab);

        // Parent ayarla (opsiyonel - hiyerarşi düzenli olsun)
        if (poolParent != null)
        {
            obj.transform.SetParent(poolParent);
        }

        obj.SetActive(false); // Başlangıçta kapalı
        return obj;
    }

    // Havuzdan obstacle al
    public GameObject GetObstacle()
    {
        // Havuzda kullanılabilir obstacle ara
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Havuzda yer yok, yeni obstacle oluştur (dinamik genişleme)
        Debug.LogWarning("⚠️ Pool doldu! Yeni obstacle ekleniyor...");
        GameObject newObj = CreateNewObstacle();
        pool.Add(newObj);
        newObj.SetActive(true);
        return newObj;
    }

    // Obstacle'ı havuza geri ver
    public void ReturnObstacle(GameObject obj)
    {
        obj.SetActive(false);

        // Pozisyonu sıfırla (opsiyonel)
        obj.transform.position = Vector3.zero;
    }

    // Tüm aktif obstacle'ları havuza geri ver
    public void ReturnAllObstacles()
    {
        foreach (GameObject obj in pool)
        {
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
            }
        }
    }
}