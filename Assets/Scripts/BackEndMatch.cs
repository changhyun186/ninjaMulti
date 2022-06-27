using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BackEnd;
using BackEnd.Tcp;
using UnityEngine.SceneManagement;

public class BackEndMatch : MonoBehaviour
{
    public OtherPlayer other;
    public MyPlayer player;
    ErrorInfo errorinfo;
    [SerializeField]
    bool isConnectMatchServer;
    string RoomToken;
    WaitForSeconds repeatTime = new WaitForSeconds(0.1f);
    MatchUserGameRecord MyStartData, EnemyStartData;
    [SerializeField]
    GameObject WaitStartGame;

    [SerializeField]
    UnityEvent onMatchStart,onMatchSuccess,onGameStart;

    AsyncOperation op;
    public void Init()
    {
        {
            //WaitStartGame.SetActive(true);
            //GameObject.FindWithTag("MyStart").GetComponent<StartProfile>().Init("운영자", 1024, Skin.None);
            //GameObject.FindWithTag("EnemyStart").GetComponent<StartProfile>().Init("시", 1402, Skin.None);
        }
        try//전판 인게임서버에서 나오지 못했을 경우를 대비
        {
            Backend.Match.LeaveGameServer();
        }
        catch
        {

        }


        Backend.Match.OnMatchMakingResponse = (args) =>
        {
            print(args.Reason);
            switch (args.ErrInfo)
            {
                case ErrorCode.Match_InProgress:
                    print("매칭 신청 성공");
                    onMatchStart?.Invoke();
                    //MatchingEventScript.Instance.MatchMaking();
                    break;

                case ErrorCode.Success:
                    print("매칭 성공");
                    onMatchSuccess?.Invoke();
                    RoomToken = args.RoomInfo.m_inGameRoomToken; //인게임 서버접속시 안쓰이고 게임방 입장에 쓰임
                    JoinInGameServer(args.RoomInfo.m_inGameServerEndPoint.m_address, args.RoomInfo.m_inGameServerEndPoint.m_port);//1단계
                    break;

                case ErrorCode.Match_MatchMakingCanceled:
                    print("매칭 취소");
                    //MatchingEventScript.Instance.MatchMakingStop();
                    break;

                default:
                    print(args.ErrInfo);
                    break;


            }



        };

        void JoinInGameServer(string serverAddress, ushort serverPort) // 임의의 함수
        {
            print("go");
            bool isReconnect = false;//재접속한건지. 디폴트값은 false
            //ErrorInfo errorInfo = null;
            
            if (!Backend.Match.JoinGameServer(serverAddress, serverPort, isReconnect, out ErrorInfo errorInfo))//2단계
            {
                print("joingamesfdetver");
                StopMatching();
            }
            else
            {
                print(errorInfo.Detail);
                print(errorInfo.Reason);
            }

        }

        Backend.Match.OnSessionJoinInServer = (args) => {
            print("joingame room");
            Backend.Match.JoinGameRoom(RoomToken); //3단계
        };

        Backend.Match.OnMatchMakingRoomCreate = (args) =>
        {
            Backend.Match.RequestMatchMaking(MatchType.Random, MatchModeType.OneOnOne, "2022-06-26T04:51:26.707Z");
        };

        Backend.Match.OnSessionListInServer = (args) =>//겜방 들어가자마자
        {
            print("ingameserver");
            //MatchDoor.instance.isClose(true);
            //op = SceneManager.LoadSceneAsync("Ingame");
            //op.allowSceneActivation = false;

            //CharacterFromWeb.Instance.userRecord = new List<MatchUserGameRecord>(args.GameRecords);
            //foreach (var a in args.GameRecords)
            //{
            //    print(a.m_sessionId);
            //}

            //if (args.GameRecords.Count == CharacterFromWeb.FullCount)
            //{
            //    CharacterFromWeb.Instance.isLast = true;
            //    print("im last");
            //    CharacterFromWeb.Instance.SetUserCharacters(2);
            //}
        };
        Backend.Match.OnMatchInGameAccess = (args) =>//다른애들 들어올 때마다
        {
            print(args.GameRecord.m_nickname);

        };





        Backend.Match.OnMatchInGameStart = () =>
        {
            onGameStart?.Invoke();
            print("gameStart");
            other.gameObject.SetActive(true);

        };

        Backend.Match.OnJoinMatchMakingServer = (args) =>
        {
            isConnectMatchServer = true;
            print("매칭서버 입장(람다식)");

        };
        Backend.Match.OnLeaveMatchMakingServer = (args) =>
        {
            isConnectMatchServer = false;
            print("매칭서버 퇴장(람다식)");
        };
    }
    public void Matchready()
    {
        if (Backend.Match.JoinMatchMakingServer(out errorinfo))
        {
            Debug.Log("매칭서버진입성공");
            Debug.Log(errorinfo);
        }
        else
        {
            Debug.Log("에러");
            Debug.Log(errorinfo);
        }

    }

    public void FastMatchMaking()
    {
        //if (!isConnectMatchServer)
            //Matchready();
        try
        {
            print("패스트매치");
            Backend.Match.CreateMatchRoom();
            print("afhter create room");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            SceneManager.LoadScene("startscene");
        }
    }


    public void StopMatching()
    {
        Backend.Match.CancelMatchMaking();
    }


    IEnumerator Poller()
    {
        while (true)
        {
            Backend.Match.Poll();
            //print(Backend.Match.Poll());
            yield return repeatTime;
        }
    }

    private void OnDestroy()
    {
        Backend.Match.LeaveMatchRoom();
    }
}
