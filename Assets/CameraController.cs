using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCam;
    [SerializeField]
    public float EffectDistance = 5,WalkEffectDistance =5 ,RunEffectDistance=15;
    [SerializeField]
    float increaseInfinitly;
    [SerializeField]
    public float increaseAmount;
    [SerializeField]
    public float NearToZero=0.1f;
    [SerializeField]
    public bool isEffecting = false;
    Transform Parent;
    [SerializeField]
    float CameraMoveSpeed;
    [SerializeField]
    Transform targetAim,Axis;

    [Header("3ÀÎÄª")]
    [SerializeField]
    float zDistance;
    [SerializeField, Range(0, 10)]
    float Height;
    [SerializeField]
    float maxHeight;
    [SerializeField]
    Vector3 HeightVec,LookUp;
    [Header("XRot")]
    [SerializeField]
    float XRot;
    [SerializeField,Range(0, 1f)]
    float XRotForPosIncreaseAmount;
    [SerializeField]
    float XRotForHeightIncreaseAmount;
    [Header("YRot")]
    public float YRot;
    //[SerializeField, Range(0, 1f)]
    //float XRotForPosIncreaseAmount;
    //[SerializeField]
    //float XRotForHeightIncreaseAmount;

    // Start is called before the first frame update
    void Start()
    {
        Parent = transform.parent;
        isEffecting = true;
        mainCam = Camera.main;
        StartCoroutine(CameraEffect());
    }

    
    IEnumerator CameraEffect()
    {
        var pos = transform.localPosition;
        var Start = pos;
        var Last = pos;


        Start.x -= EffectDistance;
        Last.x += EffectDistance;

        while (true)
        {
            if ((Mathf.Abs(transform.localPosition.x)>NearToZero) || (isEffecting) )
            {
                var start = Start; 
                start.x-= EffectDistance;
                var last = Last;
                last.x += EffectDistance;

                increaseInfinitly += Time.deltaTime * increaseAmount;
                float inter = (Mathf.Sin(increaseInfinitly) + 1) / 2;
                transform.localPosition = Vector3.Slerp(start, last,inter);
                
            }

            yield return null;
        }

    }
    // Update is called once per frame
    void Update()
    {
        Parent.rotation =Quaternion.Lerp(Parent.rotation, Quaternion.Euler(new Vector3(XRot,YRot,0))* Quaternion.Euler(LookUp),0.5f);

        var targetPos = Axis.position;
        targetPos.y += XRot*XRotForPosIncreaseAmount;
        Debug.DrawRay(targetPos, -zDistance * Axis.forward, Color.blue);
        targetPos -= zDistance*Axis.forward;
        Debug.DrawRay(targetPos,  Quaternion.Euler(HeightVec) * Axis.forward * zDistance, Color.green);
        //targetPos += Quaternion.Euler(-90,0,0)*Axis.forward*Height;
        targetPos.y += Height + XRot * XRotForHeightIncreaseAmount;
        targetPos.y = Mathf.Clamp(targetPos.y,targetAim.position.y, targetAim.position.y+maxHeight);
        
        Parent.position =Vector3.Lerp(Parent.position, targetPos,CameraMoveSpeed);
    }
    public void RecieveMouseInput(Vector3 dir)
    {
        XRot += dir.x;
        YRot += dir.y;
        XRot = Mathf.Clamp(XRot, -90, 60);
        //playerAiming.Aim(cameraController);
    }

}
//var targetVec = targetAim.rotation.eulerAngles;
//targetVec.x = 0;
//targetVec.z = 0;
//Parent.rotation = Quaternion.Euler(targetVec + new Vector3(XRot, 0, 0)) * Quaternion.Euler(LookUp);

//var targetPos = Axis.position;
//targetPos.y += XRot * XRotForPosIncreaseAmount;
//Debug.DrawRay(targetPos, -zDistance * Axis.forward, Color.blue);
//targetPos -= zDistance * Axis.forward;
//Debug.DrawRay(targetPos, Quaternion.Euler(HeightVec) * Axis.forward * zDistance, Color.green);
////targetPos += Quaternion.Euler(-90,0,0)*Axis.forward*Height;
//targetPos.y += Height + XRot * XRotForHeightIncreaseAmount;
//targetPos.y = Mathf.Clamp(targetPos.y, targetAim.position.y, Mathf.Infinity);

//Parent.position = Vector3.Lerp(Parent.position, targetPos, CameraMoveSpeed);