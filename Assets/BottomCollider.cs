using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCollider : MonoBehaviour
{
    public Transform[] targets;
    [SerializeField]
    float offSetY;
    // Update is called once per frame
    void Update()
    {
        float minY = Mathf.Infinity;
        foreach(var tp in targets)
        {
            var pos = tp.position;
            if (minY > pos.y)
                minY = pos.y; 
        }
        var targetPos = transform.position;
        targetPos.y = minY+offSetY;
        transform.position = targetPos;
    }
}
