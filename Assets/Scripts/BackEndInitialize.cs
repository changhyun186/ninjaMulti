using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
public class BackEndInitialize : MonoBehaviour
{

    private void Awake()
    {
        Backend.Initialize(HandleBackendCallback);
    }

    void HandleBackendCallback()
    {
        if (Backend.IsInitialized)
        {
            Debug.Log("�ڳ�SDK �ʱ�ȭ �Ϸ�");

            //if (!Backend.Utils.GetGoogleHash().Equals(""))
                Debug.Log(Backend.Utils.GetGoogleHash());

            Debug.Log(Backend.Utils.GetServerTime());
        }
        else
        {
            Debug.LogError("Failed to initialize the backend");
        }

    }
}

//using BackEnd;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BackEndInitialize : MonoBehaviour
//{
//    void Start()
//    {
//        Backend.Initialize(BRO =>
//        {
//            Debug.Log("�ڳ� �ʱ�ȭ ���� " + BRO);

//            // ����
//            if (BRO.IsSuccess())
//            {
//                // �ؽ�Ű 
//                Debug.Log(Backend.Utils.GetGoogleHash());
//            }

//            // ����
//            else
//            {
//                Debug.LogError("�ʱ�ȭ ����: " + BRO.GetErrorCode());
//            }
//        });
//    }
//}