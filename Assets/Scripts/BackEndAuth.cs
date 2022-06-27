using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BackEnd;
using TMPro;
public class BackEndAuth : MonoBehaviour
{
    //BackEndIngamescript serverfx;
    public GameObject logpanel;
    public TMP_InputField idInput;
    public TMP_InputField paInput;

    public TMP_InputField idInput_Sign;
    public TMP_InputField paInput_Sign;

    [SerializeField]
    private TMP_Text text;


    [SerializeField]
    GameObject AuthCanvas;
    [SerializeField]
    GameObject LoadingIcon;
    [SerializeField]
    Image Loadingbar;
    private void Start()
    {  
        //print(Backend.Utils.GetGoogleHash());
        AutoLogin();
        //serverfx = GetComponent<BackEndIngamescript>();
    }

    // ȸ������1 - ���� ���
    public void OnClickSignUp()
    {
        // ȸ�� ������ �ѵ� ����� BackEndReturnObject Ÿ������ ��ȯ�Ѵ�.
        //string error = Backend.BMember.CustomSignUp(idInput.text, paInput.text, "Test1").GetErrorCode();
        if (paInput_Sign.text.Length < 5)
        {
            format("�н����尡 �ʹ� ª���ϴ�."); return;
        }

        int error = int.Parse(Backend.BMember.CustomSignUp(idInput_Sign.text, paInput_Sign.text, "common").GetStatusCode());
        print(error);

        // ȸ�� ���� ���� ó��
        switch (error)
        {
            case 409://"DuplicatedParameterException":
                Debug.Log("�ߺ��� customId �� �����ϴ� ���");
                format("�ߺ��� ���̵� �����մϴ�.");
                break;

            case 201:
                Debug.Log("ȸ�� ���� �Ϸ�");
                logpanel.SetActive(false);
                format("ȸ������ ����");
                break;

            default:
                Debug.Log("default");
                format("access failed");

                break;



        }

        Debug.Log("���� ���============================================= ");

    }
    public void OnClickLogin1()
    {
        
        // string error = Backend.BMember.CustomLogin(idInput.text, paInput.text).GetErrorCode();
        var err = Backend.BMember.CustomLogin(idInput.text, paInput.text, "common");
        int error = int.Parse(err.GetStatusCode());
        print(err.GetMessage());

        // �α��� ���� ó��
        switch (error)
        {
            // ���̵� �Ǵ� ��й�ȣ�� Ʋ���� ���

            case 401://"BadUnauthorizedException":
                Debug.Log("���̵� �Ǵ� ��й�ȣ�� Ʋ�ȴ�.");
                format("�α��� ����");
                break;


            case 403://"BadPlayer":  //  �� ��� �ֿܼ��� �Է��� ���ܵ� ������ �����ڵ尡 �ȴ�.
                Debug.Log("���ܵ� ����");
                format("���ܵ� �����Դϴ�.");
                break;

            case 410:
                Debug.Log("1����ū����");
                format("�޸� �����Դϴ�. �������ֽʽÿ�.");
                break;

            case 200:

                Debug.Log("�α��� �Ϸ�");
                format("�α��� ��...");
                StartCoroutine("Loading");
                FlipAuthCanvas();
                break;

            default:
                Debug.Log("default");
                format("access failed");
                break;
        }
        Debug.Log("���� ���============================================= ");
    }
    public void AutoLogin()
    {
        int error =int.Parse(Backend.BMember.LoginWithTheBackendToken().GetStatusCode());
        
        switch (error)
        {

            case 201:
                Debug.Log("�ڵ� �α��� �Ϸ�");
                format("�ε���...");
                FlipAuthCanvas();
                StartCoroutine("Loading");
                break;
            // ��� �Ⱓ ����
            case 410:
                Debug.Log("1��� refresh_token�� ����� ���");
                break;

            // ��� ���Ǻ� ����
            case 401:
                Debug.Log("�ٸ� ���� �α��� �Ͽ� refresh_token�� ����� ���");
                break;

            case 403:  //  �� ��� �ֿܼ��� �Է��� ���ܵ� ������ �����ڵ尡 �ȴ�.
                Debug.Log("���ܵ� ����");
                format("���ܵ� �����Դϴ�.");
                break;

            case 400:
                Debug.Log("�ڵ� �α��� ����");
                format("�α��� �ϼ���.");
                break;

            default:
                break;
        }

        Debug.Log("���� ���============================================= ");
    }

    public void FlipAuthCanvas()
    {
        Flip(AuthCanvas);
        Flip(LoadingIcon);
    }

    public void Flip(GameObject obj)
    {
        print(obj.name);
            obj.SetActive(!obj.activeSelf);
    }

    public void LogOut()
    {
        if (Backend.BMember.Logout().GetErrorCode() == null)
        {
            FlipAuthCanvas();
            SceneManager.LoadScene("startscene");
        }
          
        text.text = "�α��� �ϼ���.";

        
    }


    void format(string str)
    {
        //if (str == "failed to login" || str == "access failed" || str == "ID has already existed.")
        //    text.color = Color.red;
        //else
        //    text.color = Color.black;

        text.text = string.Format(str);
    }

    //public void setBack()
    //{
    //    t1.text = string.Format("");
    //    t2.text = string.Format("");
    //    logpanel.SetActive(false);
    //    panel2.SetActive(false);
    //    button.SetActive(false);
    //    panel.SetActive(true);
    //}
    public void Reload()
    {

        SceneManager.LoadScene("startscene");
    }

    public IEnumerator Loading()
    {
       AsyncOperation op = SceneManager.LoadSceneAsync("main");
        op.allowSceneActivation = false;
       while(!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                Loadingbar.fillAmount = op.progress;
                
            }
            else
            {
                
                Loadingbar.fillAmount = Mathf.LerpAngle(Loadingbar.fillAmount, 1, 0.05f);
                if(Loadingbar.fillAmount>=0.999f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
           

        }
       
    }

}

