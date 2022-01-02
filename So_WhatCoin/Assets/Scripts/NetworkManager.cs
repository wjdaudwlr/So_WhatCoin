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
    public Text stateText;

    public void Login()
    {
        StartCoroutine(DataPost());
    }

    public void SignUp()
    {
        StartCoroutine(DataPostSignUp());
    }

    IEnumerator DataPost()
    {
        string url = "http://10.120.74.70:3001/auth/login";

        WWWForm form = new WWWForm();
        form.AddField("email", loginEmailInputField.text);
        form.AddField("password", loginPsswordInputField.text);
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        Debug.Log("1");
        yield return www.SendWebRequest(); // 응답이 올때까지 기다린다

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else if(www.downloadHandler.text != "false")
        {
            Debug.Log(www.downloadHandler.text);

            playerdata = www.downloadHandler.text;

            LoadingSceneController.LoadScene("GameScene");
        }
    }

    IEnumerator DataPostSignUp()
    {
        string url = "http://10.120.74.70:3001/auth/signup";

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
        }
        else if (www.downloadHandler.text != "false")
        {
            Debug.Log(www.downloadHandler);
            if (www.downloadHandler.text == "true") {
                stateText.text = "회원가입 성공";
                SignUpOnOff();
            }
            else
            {
                stateText.text = "회원가입 실패";
            }
        }
    }

    IEnumerator DataPostSave()
    {
        string url = "http://10.120.74.70:3001/auth/save";
        string jsonData = JsonConvert.SerializeObject(playerdata);

        WWWForm form = new WWWForm();

        form.AddField("userdata", jsonData);

        UnityWebRequest www = UnityWebRequest.Post(url, form);

        Debug.Log("1");
        yield return www.SendWebRequest(); // 응답이 올때까지 기다린다


        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else if (www.downloadHandler.text != "false")
        {
            Debug.Log("성공");
        }
    }


    void OnApplicationQuit()
    {
        StartCoroutine(DataPostSave());
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