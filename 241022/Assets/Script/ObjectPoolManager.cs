using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum ePoolingObj
    {
        MyUnit,
        Arrow,
        Skill,
        Enemy,
        Max,
    }


    [System.Serializable]
    public class Pool
    {
        public ePoolingObj tag;
        public GameObject prefab;  
        public int size;
    }

    [SerializeField] private List<Pool> pools;
    private Dictionary<ePoolingObj, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<ePoolingObj, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(ePoolingObj type, GameObject pos, Quaternion rotation, GameObject parent = null)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogError($"Pool with tag {type} doesn't exist.");
            return null;
        }

        GameObject obj;

        // 풀에 남은 오브젝트가 없으면 동적 확장
        if (poolDictionary[type].Count == 0)
        {
            Pool pool = pools.Find(p => p.tag == type);
            obj = Instantiate(pool.prefab, transform);
        }
        else
        {
            obj = poolDictionary[type].Dequeue();
        }

        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(pos.transform.position, rotation);

        if (parent != null)
            obj.transform.SetParent(parent.transform);
        return obj;
    }

    public void ReturnToPool(ePoolingObj type, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);

        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogError($"Pool with tag {type} doesn't exist.");
            Destroy(obj);
            return;
        }

        poolDictionary[type].Enqueue(obj);
    }
}
