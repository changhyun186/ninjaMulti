using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("OnTrigherRope");
        if (other.tag == "GrapTarget")
        {
            var GS = transform.parent.GetComponent<GrapSkill>();
           GS.isLocked = true;
            GS.InitMinDistance();
        }

    }
}
