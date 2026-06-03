using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    /*
    * 외계인 또는 슈터, 자폭 외계인이 생성하는 파티클을 저장하는 풀 매니저
    * 이동거리에 따라 점점 외계인 많아지는 구조이기 때문에 초반에 미리 프리팹 Instantiate X
    * 유니티에서 자체적으로 풀에 대한 패키지를 지원하지만 코드로 구현해보았다.
    */

    public static PoolManager Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject[] alienPrefabs;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    // 리스트로 구현된 Pools
    List<GameObject>[] alienPools;          // 외계인 배열 풀
    List<GameObject> bulletPool = new();    // 총알 파티클 풀
    List<GameObject> explosionPool = new(); // 폭팔 파티클 풀

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
