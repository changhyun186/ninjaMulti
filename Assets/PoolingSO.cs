using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pooled", menuName = "Pool/Pooler")]
public class PoolingSO : ScriptableObject
{
    [SerializeField]
    public List<PoolableMono> pooled;
    
}

