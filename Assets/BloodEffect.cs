using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : PoolableMono
{
    public override void OnDie()
    {
    }

    public override void OnSpawn()
    {
        Invoke("Kill", 1);
        
    }

}
