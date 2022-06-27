using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pooled", menuName = "Pool/Pooled")]
public class Pooled : ScriptableObject
{
    [SerializeField]
    public PoolableMono poolableMono;
    public string poolName;
}