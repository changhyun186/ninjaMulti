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

    // �ѱ�, ����, ���ڸ� �Է� �����ϰ�
    private bool CheckNickname(TMP_InputField text)
    {
        if (text.text.Length > 8)
            return false;
        return Regex.IsMatch(text.text, "^[0-9a-zA-Z��-�R]*$");
    }
    // �г��� ����
    public void OnClickCreateName()
    {
        // �ѱ�, ����, ���ڷθ� �г����� ��������� üũ
        if (!CheckNickname(NickNameInput))
        {
            print("�ѱ�, ����, ���ڸ� ����");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.CreateNickname(NickNameInput.text);

        if (BRO.IsSuccess())
        {
            print("�г��� ���� �Ϸ�");
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
                    print("�̹� �ߺ��� �г����� �ֽ��ϴ�.");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) print("20�� �̻��� �г��� �����");
                    else if (BRO.GetMessage().Contains("blank")) print("�г��ӿ� ��/�� ������ �ֽ��ϴ�.");
                    break;

                default:
                    print("���� ���� ���� �߻�: " + BRO.GetErrorCode());
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
            
        // �ѱ�, ����, ���ڷθ� �г����� ��������� üũ
        if (!CheckNickname(ChangeNickNameInput))
        {
            //PopUpLogScript.Instance.PopUp("�ѱ�, ����, ���ڸ� ����");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.UpdateNickname(ChangeNickNameInput.text);

        if (BRO.IsSuccess())
        {
            //PopUpLogScript.Instance.PopUp("�г��� ���� �Ϸ�");
            //PanelManager.Instance.Clicked("ChangeNickName");
        }

        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    //PopUpLogScript.Instance.PopUp("�̹� �ߺ��� �г����� �ֽ��ϴ�.");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) ; //PopUpLogScript.Instance.PopUp("20�� �̻��� �г��� �����");
                    else if (BRO.GetMessage().Contains("blank")) ; //PopUpLogScript.Instance.PopUp("�г��ӿ� ��/�� ������ �ֽ��ϴ�.");
                        break;

                default:
                    print("���� ���� ���� �߻�: " + BRO.GetErrorCode());
                    break;
            }
        }


    }
}
