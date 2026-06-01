using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    public GameObject[] alienPrefabs;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    List<GameObject>[] alienPools;
    List<GameObject> bulletPool = new();
    List<GameObject> explosionPool = new();

    void Awake()
    {
        Instance = this;
        
        alienPools = new List<GameObject>[alienPrefabs.Length];

        for (int i = 0; i < alienPools.Length; i++)
        {
            alienPools[i] = new List<GameObject>();
        }
    }

    public GameObject GetAlien(int i)
    {
        return GetFromPool(alienPools[i], alienPrefabs[i]);
    }

    public GameObject GetBullet()
    {
        return GetFromPool(bulletPool, bulletPrefab);
    }

    public GameObject GetExplosion()
    {
        return GetFromPool(explosionPool, explosionPrefab);
    }

    GameObject GetFromPool(List<GameObject> pool, GameObject prefab)
    {
        GameObject select = null;

        foreach (GameObject item in pool)
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(prefab, transform);
            pool.Add(select);
        }

        return select;
    }
}
