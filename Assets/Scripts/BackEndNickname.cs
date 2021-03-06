using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using BackEnd;
using System.Text.RegularExpressions;
using TMPro;
public class BackEndNickname : MonoBehaviour
{
    public TMP_InputField NickNameInput;

    public TMP_InputField ChangeNickNameInput;
    public GameObject ChangeNickNamePanel;
    public UnityEvent onCreateName;
    private void Awake()
    {
    }
    public static string GetMyNickName()
    {
        return Backend.BMember.GetUserInfo().GetReturnValuetoJSON()["row"]["nickname"].ToString();
    }

    // 한글, 영어, 숫자만 입력 가능하게
    private bool CheckNickname(TMP_InputField text)
    {
        if (text.text.Length > 8)
            return false;
        return Regex.IsMatch(text.text, "^[0-9a-zA-Z가-힣]*$");
    }
    // 닉네임 생성
    public void OnClickCreateName()
    {
        // 한글, 영어, 숫자로만 닉네임을 만들었는지 체크
        if (!CheckNickname(NickNameInput))
        {
            print("한글, 영어, 숫자만 가능");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.CreateNickname(NickNameInput.text);

        if (BRO.IsSuccess())
        {
            print("닉네임 생성 완료");
            ChangeNickNamePanel.SetActive(false);
            FindObjectOfType<BackEndMatch>().StartCoroutine("Poller");
            FindObjectOfType<BackEndMatch>().Matchready();
            onCreateName?.Invoke();
        }

        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    print("이미 중복된 닉네임이 있습니다.");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) print("20자 이상의 닉네임 비허용");
                    else if (BRO.GetMessage().Contains("blank")) print("닉네임에 앞/뒤 공백이 있습니다.");
                    break;

                default:
                    print("서버 공통 에러 발생: " + BRO.GetErrorCode());
                    break;
            }
        }
    }

    public void OnClickChangeName()
    {
        if(ChangeNickNameInput.text.Equals(""))
        {
            //PanelManager.Instance.Clicked("ChangeNickName");
            return;

        }
            
        // 한글, 영어, 숫자로만 닉네임을 만들었는지 체크
        if (!CheckNickname(ChangeNickNameInput))
        {
            //PopUpLogScript.Instance.PopUp("한글, 영어, 숫자만 가능");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.UpdateNickname(ChangeNickNameInput.text);

        if (BRO.IsSuccess())
        {
            //PopUpLogScript.Instance.PopUp("닉네임 변경 완료");
            //PanelManager.Instance.Clicked("ChangeNickName");
        }

        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    //PopUpLogScript.Instance.PopUp("이미 중복된 닉네임이 있습니다.");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) ; //PopUpLogScript.Instance.PopUp("20자 이상의 닉네임 비허용");
                    else if (BRO.GetMessage().Contains("blank")) ; //PopUpLogScript.Instance.PopUp("닉네임에 앞/뒤 공백이 있습니다.");
                        break;

                default:
                    print("서버 공통 에러 발생: " + BRO.GetErrorCode());
                    break;
            }
        }


    }
}
