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

    // ÇÑ±Û, ¿µ¾î, ¼ýÀÚ¸¸ ÀÔ·Â °¡´ÉÇÏ°Ô
    private bool CheckNickname(TMP_InputField text)
    {
        if (text.text.Length > 8)
            return false;
        return Regex.IsMatch(text.text, "^[0-9a-zA-Z°¡-ÆR]*$");
    }
    // ´Ð³×ÀÓ »ý¼º
    public void OnClickCreateName()
    {
        // ÇÑ±Û, ¿µ¾î, ¼ýÀÚ·Î¸¸ ´Ð³×ÀÓÀ» ¸¸µé¾ú´ÂÁö Ã¼Å©
        if (!CheckNickname(NickNameInput))
        {
            print("ÇÑ±Û, ¿µ¾î, ¼ýÀÚ¸¸ °¡´É");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.CreateNickname(NickNameInput.text);

        if (BRO.IsSuccess())
        {
            print("´Ð³×ÀÓ »ý¼º ¿Ï·á");
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
                    print("ÀÌ¹Ì Áßº¹µÈ ´Ð³×ÀÓÀÌ ÀÖ½À´Ï´Ù.");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) print("20ÀÚ ÀÌ»óÀÇ ´Ð³×ÀÓ ºñÇã¿ë");
                    else if (BRO.GetMessage().Contains("blank")) print("´Ð³×ÀÓ¿¡ ¾Õ/µÚ °ø¹éÀÌ ÀÖ½À´Ï´Ù.");
                    break;

                default:
                    print("¼­¹ö °øÅë ¿¡·¯ ¹ß»ý: " + BRO.GetErrorCode());
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
            
        // ÇÑ±Û, ¿µ¾î, ¼ýÀÚ·Î¸¸ ´Ð³×ÀÓÀ» ¸¸µé¾ú´ÂÁö Ã¼Å©
        if (!CheckNickname(ChangeNickNameInput))
        {
            //PopUpLogScript.Instance.PopUp("ÇÑ±Û, ¿µ¾î, ¼ýÀÚ¸¸ °¡´É");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.UpdateNickname(ChangeNickNameInput.text);

        if (BRO.IsSuccess())
        {
            //PopUpLogScript.Instance.PopUp("´Ð³×ÀÓ º¯°æ ¿Ï·á");
            //PanelManager.Instance.Clicked("ChangeNickName");
        }

        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    //PopUpLogScript.Instance.PopUp("ÀÌ¹Ì Áßº¹µÈ ´Ð³×ÀÓÀÌ ÀÖ½À´Ï´Ù.");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) ; //PopUpLogScript.Instance.PopUp("20ÀÚ ÀÌ»óÀÇ ´Ð³×ÀÓ ºñÇã¿ë");
                    else if (BRO.GetMessage().Contains("blank")) ; //PopUpLogScript.Instance.PopUp("´Ð³×ÀÓ¿¡ ¾Õ/µÚ °ø¹éÀÌ ÀÖ½À´Ï´Ù.");
                        break;

                default:
                    print("¼­¹ö °øÅë ¿¡·¯ ¹ß»ý: " + BRO.GetErrorCode());
                    break;
            }
        }


    }
}
