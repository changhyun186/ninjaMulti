using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAim : MonoBehaviour
{
    public MyPlayer player;
    public OtherPlayer otherPlayer;
    // Update is called once per frame
    void Update()
    {
        if (player != null)
            player.positionPacket.targetPos = transform.position;
        if(otherPlayer != null)
            otherPlayer.positionPacket.targetPos = transform.position;
    }
}
