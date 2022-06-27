using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    [SerializeField]
    Transform playerAgent;
    MyPlayer player;
    CameraController cameraController;
    [SerializeField]
    UnityEvent<Vector3> onRayHitDir;
    [SerializeField]
    UnityEvent<Vector3> onRayHitPos;
    [SerializeField]
    UnityEvent<Vector3> onClickRayHitPos,onClickRayHitDir, onRightClickRayHitPos, onRightClickRayHitDir;
    [SerializeField]
    float DistanceBetweeenCamAndPlayer;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<MyPlayer>();
        cameraController = FindObjectOfType<CameraController>();
        //SetCursor(false);
    }

    public void SetCursor(bool isOn)
    {
        Cursor.visible = isOn;
        Cursor.lockState = isOn ? CursorLockMode.None : CursorLockMode.Locked;
        

    }
    public void SetCursor()
    {
        print(Cursor.lockState);
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        print(Cursor.lockState);

    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
       
        if (Physics.Raycast(player.MainCam.transform.position+ player.MainCam.transform.forward*DistanceBetweeenCamAndPlayer, player.MainCam.transform.forward, out hit, 1000))
        {
            Vector3 dir = hit.point - player.transform.position;
            dir.Normalize();

            onRayHitDir?.Invoke(dir);
            onRayHitPos?.Invoke(hit.point);
            
            
        }
        
    }

    public void OnClickLeft()
    {
        RaycastHit hit;

        if (Physics.Raycast(player.MainCam.transform.position + player.MainCam.transform.forward * DistanceBetweeenCamAndPlayer, player.MainCam.transform.forward, out hit, 1000))
        {
            Vector3 dir = hit.point - player.transform.position;
            dir.Normalize();
            onClickRayHitDir?.Invoke(dir);
            onClickRayHitPos?.Invoke(hit.point);
        }
    }
    public void OnClickRight()
    {
        RaycastHit hit;

        if (Physics.Raycast(player.MainCam.transform.position + player.MainCam.transform.forward * DistanceBetweeenCamAndPlayer, player.MainCam.transform.forward, out hit, 1000))
        {
            Vector3 dir = hit.point - player.transform.position;
            dir.Normalize();
            onRightClickRayHitDir?.Invoke(dir);
            onRightClickRayHitPos?.Invoke(hit.point);
        }
    }
}
