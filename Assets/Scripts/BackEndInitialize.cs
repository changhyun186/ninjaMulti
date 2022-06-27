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
            Debug.Log("뒤끝SDK 초기화 완료");

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
//            Debug.Log("뒤끝 초기화 진행 " + BRO);

//            // 성공
//            if (BRO.IsSuccess())
//            {
//                // 해쉬키 
//                Debug.Log(Backend.Utils.GetGoogleHash());
//            }

//            // 실패
//            else
//            {
//                Debug.LogError("초기화 실패: " + BRO.GetErrorCode());
//            }
//        });
//    }
//}