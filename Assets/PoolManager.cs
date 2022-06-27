using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleTon<PoolManager>
{
    [SerializeField]
    PoolingSO poolingSO;
    Dictionary<string, Queue<PoolableMono>> poolQueue = new Dictionary<string, Queue<PoolableMono>>();

    private void Awake()
    {
        
        CreatePool("bullet",100);
        CreatePool("bulletImpact",20);
        CreatePool("blood", 20);
    }

    public void CreatePool(string poolName, int Count = 20)
    {

        if (!poolQueue.ContainsKey(poolName))
            poolQueue[poolName] = new Queue<PoolableMono>();

        PoolableMono prefab = GetObject(poolName);
        
        for (int i = 0; i < Count; i++)
        {
            var instance = Instantiate(prefab, transform);
            instance.gameObject.SetActive(false);
            poolQueue[poolName].Enqueue(instance);
        }
    }


    public PoolableMono GetObject(string poolName)
    {
        return poolingSO.pooled.Find(x => x.poolName == poolName);
    }


    public void Push(PoolableMono poolableMono)
    {
        if (poolQueue[poolableMono.poolName] == null)
            poolQueue[poolableMono.poolName] = new Queue<PoolableMono>();

        poolQueue[poolableMono.poolName].Enqueue(poolableMono);
        poolableMono.gameObject.SetActive(false);
        poolableMono.transform.SetParent(transform);
        poolableMono.OnDie();
    }

    public PoolableMono Pop(string poolName)
    {
        if (poolQueue[poolName] == null || poolQueue[poolName].Count == 0)
            return null;
        else
        {
            var pop = poolQueue[poolName].Dequeue();
            pop.transform.SetParent(null);
            pop.gameObject.SetActive(true);
            pop.OnSpawn();
            return pop;
            
        }
    }


}
