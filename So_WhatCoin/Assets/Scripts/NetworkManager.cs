using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class NetworkManager : MonoBehaviour
{
    [Header("Login")]
    public InputField loginEmailInputField;
    public InputField loginPsswordInputField;

    [Header("Sing UP")]
    public InputField signupEmailInputField;
    public InputField signupPsswordInputField;
    public InputField signupNameInputField;
    public GameObject signupPanel;

    public static string playerdata;
    string playerEmail, playerPassword;
    public Text stateText;

    public void Start()
    {
        playerEmail = PlayerPrefs.GetString("playerEmail");
        playerPassword = PlayerPrefs.GetString("playerPassword");

        Debug.Log("playerEmail : " + playerEmail + "\nplayerPassword : " + playerPassword);

        if (playerEmail != null && playerPassword != null)
        {
            StartCoroutine(DataPost(playerEmail, playerPassword));
        }
    }

    public void Login()
    {
        StartCoroutine(DataPost(loginEmailInputField.text, loginPsswordInputField.text));
    }

    public void SignUp()
    {
        StartCoroutine(DataPostSignUp());
    }

    IEnumerator DataPost(string email, string password)
    {
        string url = "http://louis7308.iptime.org:3001/auth/login";

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        Debug.Log("1");
        yield return www.SendWebRequest(); // 응답이 올때까지 기다린다

      

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            stateText.text = "로그인 실패";
            stateText.GetComponent<FadeEffect>().FadeText(1, 0);
        }
        else if(www.downloadHandler.text != "false")
        {
            Debug.Log(www.downloadHandler.text);

            playerdata = www.downloadHandler.text;

            PlayerPrefs.SetString("playerEmail", email);
            PlayerPrefs.SetString("playerPassword", password);

            LoadingSceneController.LoadScene("GameScene");
        }
        else if(www.downloadHandler.text == "false")
        {
            stateText.text = "로그인 실패";
            stateText.GetComponent<FadeEffect>().FadeText(1, 0);
        }
    }

    IEnumerator DataPostSignUp()
    {
        string url = "http://louis7308.iptime.org:3001/auth/signup";

        WWWForm form = new WWWForm();

        form.AddField("email", signupEmailInputField.text);
        form.AddField("password", signupPsswordInputField.text);
        form.AddField("name", signupNameInputField.text);

        UnityWebRequest www = UnityWebRequest.Post(url, form);

        Debug.Log("1");
        yield return www.SendWebRequest(); // 응답이 올때까지 기다린다


        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            stateText.text = "회원가입 실패";
            stateText.GetComponent<FadeEffect>().FadeText(1, 0);
        }
        else if (www.downloadHandler.text != "false")
        {
            Debug.Log(www.downloadHandler);
            if (www.downloadHandler.text == "true") {
                stateText.text = "회원가입 성공";
                stateText.GetComponent<FadeEffect>().FadeText(1, 0);
                signupEmailInputField.text = null;
                signupPsswordInputField.text = null;
                signupNameInputField.text = null;
                SignUpOnOff();
            }
            else
            {
                stateText.text = "회원가입 실패";
                stateText.GetComponent<FadeEffect>().FadeText(1, 0);
                signupEmailInputField.text = null;
                signupPsswordInputField.text = null;
                signupNameInputField.text = null;
            }
        }
    }



    public void SignUpOnOff()
    {
        if (signupPanel.activeSelf)
        {
            signupPanel.SetActive(false);
        }
        else
            signupPanel.SetActive(true);
    }
}