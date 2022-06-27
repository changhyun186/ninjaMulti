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

    // 회원가입1 - 동기 방식
    public void OnClickSignUp()
    {
        // 회원 가입을 한뒤 결과를 BackEndReturnObject 타입으로 반환한다.
        //string error = Backend.BMember.CustomSignUp(idInput.text, paInput.text, "Test1").GetErrorCode();
        if (paInput_Sign.text.Length < 5)
        {
            format("패스워드가 너무 짧습니다."); return;
        }

        int error = int.Parse(Backend.BMember.CustomSignUp(idInput_Sign.text, paInput_Sign.text, "common").GetStatusCode());
        print(error);

        // 회원 가입 실패 처리
        switch (error)
        {
            case 409://"DuplicatedParameterException":
                Debug.Log("중복된 customId 가 존재하는 경우");
                format("중복된 아이디가 존재합니다.");
                break;

            case 201:
                Debug.Log("회원 가입 완료");
                logpanel.SetActive(false);
                format("회원가입 성공");
                break;

            default:
                Debug.Log("default");
                format("access failed");

                break;



        }

        Debug.Log("동기 방식============================================= ");

    }
    public void OnClickLogin1()
    {
        
        // string error = Backend.BMember.CustomLogin(idInput.text, paInput.text).GetErrorCode();
        var err = Backend.BMember.CustomLogin(idInput.text, paInput.text, "common");
        int error = int.Parse(err.GetStatusCode());
        print(err.GetMessage());

        // 로그인 실패 처리
        switch (error)
        {
            // 아이디 또는 비밀번호가 틀렸을 경우

            case 401://"BadUnauthorizedException":
                Debug.Log("아이디 또는 비밀번호가 틀렸다.");
                format("로그인 실패");
                break;


            case 403://"BadPlayer":  //  이 경우 콘솔에서 입력한 차단된 사유가 에러코드가 된다.
                Debug.Log("차단된 유저");
                format("차단된 유저입니다.");
                break;

            case 410:
                Debug.Log("1년토큰만료");
                format("휴먼 계정입니다. 문의해주십시오.");
                break;

            case 200:

                Debug.Log("로그인 완료");
                format("로그인 중...");
                StartCoroutine("Loading");
                FlipAuthCanvas();
                break;

            default:
                Debug.Log("default");
                format("access failed");
                break;
        }
        Debug.Log("동기 방식============================================= ");
    }
    public void AutoLogin()
    {
        int error =int.Parse(Backend.BMember.LoginWithTheBackendToken().GetStatusCode());
        
        switch (error)
        {

            case 201:
                Debug.Log("자동 로그인 완료");
                format("로딩중...");
                FlipAuthCanvas();
                StartCoroutine("Loading");
                break;
            // 토근 기간 만료
            case 410:
                Debug.Log("1년뒤 refresh_token이 만료된 경우");
                break;

            // 토근 조건부 만료
            case 401:
                Debug.Log("다른 기기로 로그인 하여 refresh_token이 만료된 경우");
                break;

            case 403:  //  이 경우 콘솔에서 입력한 차단된 사유가 에러코드가 된다.
                Debug.Log("차단된 유저");
                format("차단된 유저입니다.");
                break;

            case 400:
                Debug.Log("자동 로그인 실패");
                format("로그인 하세요.");
                break;

            default:
                break;
        }

        Debug.Log("동기 방식============================================= ");
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
          
        text.text = "로그인 하세요.";

        
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

