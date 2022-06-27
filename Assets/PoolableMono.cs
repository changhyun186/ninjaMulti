using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableMono : MonoBehaviour
{
    public string poolName;
    public abstract void OnSpawn();
    public abstract void OnDie();

    public void Kill()
    {
        PoolManager.Instance.Push(this);
    }
}
