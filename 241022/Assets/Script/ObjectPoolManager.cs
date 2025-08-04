using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPoolManager;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ObjectPoolManager>();
            return instance;
        }
    }

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    [System.Serializable]
    public class Pool
    {
        public string tag;       
        public GameObject prefab;  
        public int size;           
    }

    [SerializeField] private List<Pool> pools;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public void AddUnitPool(Dictionary<int, UnitsData> units)
    {
        foreach (var kvp in units)
        {
            UnitsData unitData = kvp.Value;
            string tag = unitData.unitType.ToString();

            if (poolDictionary.ContainsKey(tag))
                continue;

            GameObject prefab = Resources.Load<GameObject>(unitData.prefabPath);
            if (prefab == null)
            {
                Debug.LogWarning($"[PoolManager] {unitData.unitName} 프리팹을 찾을 수 없습니다. 경로: {unitData.prefabPath}");
                continue;
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();

            int poolSize = 3; 
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(tag, objectPool);
        }
    }

    public void AddPool(string tag, GameObject prefab, int size)
    {
        prefab.transform.SetParent(gameObject.transform);
        if (!poolDictionary.ContainsKey(tag))
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(tag, objectPool);
        }
        else
        {
            Debug.LogWarning($"Pool with tag {tag} already exists!");
        }
    }


    public GameObject GetObjPool(string tag, GameObject parent, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject poolOBJ = poolDictionary[tag].Count > 0
            ? poolDictionary[tag].Dequeue()
            : Instantiate(pools.Find(x => x.tag == tag).prefab);

        poolOBJ.SetActive(true);
        poolOBJ.transform.position = parent.transform.position;
        poolOBJ.transform.rotation = rotation;
        //poolOBJ.transform.parent = parent.transform;

        return poolOBJ;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(gameObject.transform);
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return;
        }

        poolDictionary[tag].Enqueue(obj);
    }
}
