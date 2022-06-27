using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using BackEnd.Tcp;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Linq;
using DG.Tweening;
public enum keyInputByte
{
    C,E, F, Q, R,
    leftShiftUp,leftShiftDown,space,leftMouse,rightMouseDown, rightMouseUp, leftMouseDown, leftMouseUp,

}

public enum packetType
{
    PositionPacket, DamagePacket,InputPacket
}
public class BackEndIngame : MonoSingleTon<BackEndIngame>
{
    public float maxHp = 100;
    public float hp = 100;
    public Image hpBar;
    public OtherPlayer other_Player;
    public Transform other_target;

    public UnityEvent<Vector3> inputMoveEvent;
    public UnityEvent<Vector3> inputMouseEvent;
    public UnityEvent inputShiftDownEvent;
    public UnityEvent inputShiftUpEvent;
    public UnityEvent inputSpaceEvent;
    public UnityEvent inputMouseLeftDownEvent;
    public UnityEvent inputMouseLeftStayEvent;
    public UnityEvent inputMouseLeftUpEvent;
    public UnityEvent<Vector3> inputMouseRightDownEvent;
    public UnityEvent inputMouseRightUpEvent;
    public UnityEvent inputCEvent;
    public UnityEvent inputREvent;
    public UnityEvent inputTEvent;
    public UnityEvent inputEEvent;
    public UnityEvent inputVEvent;
    public UnityEvent inputQEvent;
    public UnityEvent inputEscEvent;

    public float posLerp,rotLerp;

    [SerializeField]
    TMPro.TMP_Text scoreText;
    public int myScore, enemyScore;

    public float otherSpeed;
    private void Start()
    {
        Backend.Match.OnMatchRelay = _args =>
         {
             if(_args.BinaryUserData.SequenceEqual(new byte[] { 0, 5, 2, 8 }))
             {
                 if (!_args.From.IsRemote) return;
                 myScore++;
                 scoreText.text = myScore + " : " + enemyScore;
             }
             MatchRelayEventArgs args = _args;

             if (!args.From.IsRemote) return; 
             print(args.From.IsRemote ? "적에게받음" : "내거받음");
             packetType p = (packetType)(args.BinaryUserData[0]);
             print("타입"+p);
             switch(p)
             {
                 case packetType.PositionPacket:
                     PositionPacket pp= ByteToPacket<PositionPacket>(args.BinaryUserData);
                     print("pp타입변환한ㄹ거" + pp.pos+" "+pp.rot+" "+pp.targetPos);
                     //other_Player.transform.position = Vector3.Lerp(other_Player.transform.position, pp.pos,posLerp); print("pp.pos" + pp.pos);
                     other_Player.transform.DOMove(pp.pos, otherSpeed);
                     //var dir = other_Player.transform.position - pp.pos;
                     //dir.Normalize();
                     //other_Player.rigidbody.AddForce(dir*otherSpeed);
                     other_Player.transform.rotation = Quaternion.Lerp(other_Player.transform.rotation,pp.rot,rotLerp); print("pp.rot" + pp.rot);
                     other_target.transform.position = pp.targetPos;
                     other_Player.rigidbody.velocity = pp.velocity;
                     inputMoveEvent?.Invoke(pp.velocity);
                     break;

                case packetType.DamagePacket:
                     DamagePacket dp = ByteToPacket<DamagePacket>(args.BinaryUserData);
                     hp -= dp.damage;
                     print(dp.damage);
                     hpBar.fillAmount = hp / maxHp;
                     if(hp<0)
                     {
                         FindObjectOfType<MyPlayer>().transform.position = new Vector3();
                         hp = 100;
                         hpBar.fillAmount = 1;
                         enemyScore++;
                         scoreText.text = myScore + " : " + enemyScore;
                         Backend.Match.SendDataToInGameRoom(new byte[] { 0, 5, 2, 8 });
                     }
                     break;
                 case packetType.InputPacket:
                     InputKeyPacket ip = ByteToPacket<InputKeyPacket>(args.BinaryUserData);
                     keyInputByte key = ip.keyInput;
                     print(key.ToString());
                     switch(key)
                     {
                         case keyInputByte.C:
                             inputCEvent?.Invoke();
                             break;
                         case keyInputByte.R:
                             inputREvent?.Invoke();
                             break;
                         case keyInputByte.E:
                             inputEEvent?.Invoke();
                             break;

                         case keyInputByte.leftShiftDown:
                             inputShiftDownEvent?.Invoke();
                             break;
                         case keyInputByte.leftShiftUp:
                             inputShiftUpEvent?.Invoke();
                             break;
                         case keyInputByte.space:
                             inputSpaceEvent?.Invoke();
                             break;
                         case keyInputByte.rightMouseDown:
                             inputMouseRightDownEvent?.Invoke(other_target.position);
                             break;
                         case keyInputByte.rightMouseUp:
                             inputMouseRightUpEvent?.Invoke();
                             break;
                         case keyInputByte.leftMouseDown:
                             inputMouseLeftDownEvent?.Invoke();
                             break;
                         case keyInputByte.leftMouseUp:
                             inputMouseLeftUpEvent?.Invoke();
                             break;
                         case keyInputByte.Q:
                             inputQEvent?.Invoke();
                             break;

                     }
                     break;
             }
         };
    }

    public void Send(byte[] arr)
    {
        if (gameObject.activeSelf == false) return;
        Backend.Match.SendDataToInGameRoom(arr);
    }

    public void SendC()
    {
        SendInput(keyInputByte.C);
    }
    public void SendR()
    {
        SendInput(keyInputByte.R);
    }
    public void SendE()
    {
        SendInput(keyInputByte.E);
    }
    public void SendQ()
    {
        SendInput(keyInputByte.Q);
    }
    public void SendLeftShiftDown()
    {
        SendInput(keyInputByte.leftShiftDown);
    }

    public void SendRightMouseDown()
    {
        SendInput(keyInputByte.rightMouseDown);
    }
    public void SendRightMouseUp()
    {
        SendInput(keyInputByte.rightMouseUp);
    }
    public void SendLeftMouseDown()
    {
        SendInput(keyInputByte.leftMouseDown);
    }
    public void SendLeftMouseUp()
    {
        SendInput(keyInputByte.leftMouseUp);
    }

    public void SendSpace()
    {
        SendInput(keyInputByte.space);
    }

    public void SendLeftShiftUp()
    {
        SendInput(keyInputByte.leftShiftUp);
    }






    //public SelectedCardVO dic;
    //public CharacterVO _character;
    //public CharacterVO character
    //{
    //    get { return _character; }
    //    set
    //    {
    //        _character = value;
    //        card.Init(value);
    //        //GameManager.Instance.SetImageOfButton(Backend.UserNickName, value.name);
    //    }
    //}
    //public IngameCard card;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    MatchDoor.instance.isClose(false);//�빮������
    //    Invoke("RemoveDoor", 4);
    //    if(CharacterFromWeb.Instance.isLast)
    //    {
    //        GameManager.SendFirst(CharacterFromWeb.Instance.dic);
    //    }

    //}

    //void RemoveDoor() => Destroy(MatchDoor.instance.gameObject);
    //public void RequireData()
    //{
    //    Backend.Match.SendDataToInGameRoom(new byte[] { (byte)PACKET_NUM.RequireData });
    //}
    public static byte[] StringToByte(string str)
    {
        List<byte> list = new List<byte>(Encoding.UTF8.GetBytes(str.Substring(1,str.Length-1)));
        print(str[0]);
        list.Insert(0, byte.Parse(str[0].ToString()));
        byte[] StrByte = list.ToArray();
        return StrByte;
    }
    public static T ByteToPacket<T>(byte[] strByte)
    {
        var list =  new List<byte>(strByte);
        list.RemoveAt(0);
        strByte = list.ToArray();
        string str = Encoding.Default.GetString(strByte);
        return JsonUtility.FromJson<T>(str);
    }

    public void SendInput(keyInputByte keyCode)
    {
        InputKeyPacket ip = new InputKeyPacket();
        ip.keyInput = keyCode;
        byte[] arr = BackEndIngame.StringToByte((int)packetType.InputPacket + JsonUtility.ToJson(ip));
        BackEndIngame.Instance.Send(arr);
    }
}

[System.Serializable]
public struct PositionPacket
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 targetPos;
    public Vector3 velocity;
}

[System.Serializable]
public struct DamagePacket
{
    public float damage;
}

[System.Serializable]
public struct InputKeyPacket
{
    public keyInputByte keyInput;
}