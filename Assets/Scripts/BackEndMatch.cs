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
            //GameObject.FindWithTag("MyStart").GetComponent<StartProfile>().Init("���", 1024, Skin.None);
            //GameObject.FindWithTag("EnemyStart").GetComponent<StartProfile>().Init("��", 1402, Skin.None);
        }
        try//���� �ΰ��Ӽ������� ������ ������ ��츦 ���
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
                    print("��Ī ��û ����");
                    onMatchStart?.Invoke();
                    //MatchingEventScript.Instance.MatchMaking();
                    break;

                case ErrorCode.Success:
                    print("��Ī ����");
                    onMatchSuccess?.Invoke();
                    RoomToken = args.RoomInfo.m_inGameRoomToken; //�ΰ��� �������ӽ� �Ⱦ��̰� ���ӹ� ���忡 ����
                    JoinInGameServer(args.RoomInfo.m_inGameServerEndPoint.m_address, args.RoomInfo.m_inGameServerEndPoint.m_port);//1�ܰ�
                    break;

                case ErrorCode.Match_MatchMakingCanceled:
                    print("��Ī ���");
                    //MatchingEventScript.Instance.MatchMakingStop();
                    break;

                default:
                    print(args.ErrInfo);
                    break;


            }



        };

        void JoinInGameServer(string serverAddress, ushort serverPort) // ������ �Լ�
        {
            print("go");
            bool isReconnect = false;//�������Ѱ���. ����Ʈ���� false
            //ErrorInfo errorInfo = null;
            
            if (!Backend.Match.JoinGameServer(serverAddress, serverPort, isReconnect, out ErrorInfo errorInfo))//2�ܰ�
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
            Backend.Match.JoinGameRoom(RoomToken); //3�ܰ�
        };

        Backend.Match.OnMatchMakingRoomCreate = (args) =>
        {
            Backend.Match.RequestMatchMaking(MatchType.Random, MatchModeType.OneOnOne, "2022-06-26T04:51:26.707Z");
        };

        Backend.Match.OnSessionListInServer = (args) =>//�׹� ���ڸ���
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
        Backend.Match.OnMatchInGameAccess = (args) =>//�ٸ��ֵ� ���� ������
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
            print("��Ī���� ����(���ٽ�)");

        };
        Backend.Match.OnLeaveMatchMakingServer = (args) =>
        {
            isConnectMatchServer = false;
            print("��Ī���� ����(���ٽ�)");
        };
    }
    public void Matchready()
    {
        if (Backend.Match.JoinMatchMakingServer(out errorinfo))
        {
            Debug.Log("��Ī�������Լ���");
            Debug.Log(errorinfo);
        }
        else
        {
            Debug.Log("����");
            Debug.Log(errorinfo);
        }

    }

    public void FastMatchMaking()
    {
        //if (!isConnectMatchServer)
            //Matchready();
        try
        {
            print("�н�Ʈ��ġ");
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
