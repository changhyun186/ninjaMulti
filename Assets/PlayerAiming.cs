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
        playerChestTr = animator.GetBoneTransform(HumanBodyBones.Chest); // �ش� ���� transform ��������
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
        playerChestTr?.LookAt(ChestDir); //��ü�� ī�޶� ���¹������� ����

        playerChestTr.rotation = playerChestTr.rotation * Quaternion.Euler(ChestOffset);
    }
}
