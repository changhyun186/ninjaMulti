using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public abstract void RecieveMoveInput(Vector3 vector);
    public abstract void RecieveMouseInput(Vector3 vector);
    
    public abstract void OnDie();
    public abstract void OnSpawn();

}
