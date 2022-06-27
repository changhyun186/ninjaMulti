using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    Transform playerChestTr;

    Vector3 ChestOffset;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerChestTr = animator.GetBoneTransform(HumanBodyBones.Chest); // 해당 본의 transform 가져오기
    }
   
    private void LateUpdate()
    {
        Aim(Camera.main.GetComponent<CameraController>());
    }
    // Update is called once per frame
    public void Aim(CameraController MainCam)
    {
        var ChestDir = MainCam.transform.position + MainCam.transform.forward * 50f;
        playerChestTr = animator.GetBoneTransform(HumanBodyBones.Chest);
        playerChestTr?.LookAt(ChestDir); //상체를 카메라 보는방향으로 보기

        playerChestTr.rotation = playerChestTr.rotation * Quaternion.Euler(ChestOffset);
    }
}
