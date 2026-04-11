using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [Header("Pools")]
    public List<Pool> pools;

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, this.transform);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectQueue);
        }
    }

    // Ambil object dari pool
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool dengan tag '{tag}' tidak ditemukan!");
            return null;
        }

        Queue<GameObject> queue = poolDictionary[tag];

        // Kalau pool habis, expand otomatis
        if (queue.Count == 0)
        {
            Pool pool = pools.Find(p => p.tag == tag);
            GameObject newObj = Instantiate(pool.prefab, this.transform);
            newObj.SetActive(false);
            queue.Enqueue(newObj);
            Debug.Log($"Pool '{tag}' expanded!");
        }

        GameObject obj = queue.Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        // Notify object bahwa dia di-spawn
        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnSpawn();

        return obj;
    }

    // Kembalikan object ke pool
    public void ReturnToPool(string tag, GameObject obj)
    {
        obj.SetActive(false);

        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnDespawn();

        poolDictionary[tag].Enqueue(obj);
    }
}

// Interface untuk object yang bisa di-pool
public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
}