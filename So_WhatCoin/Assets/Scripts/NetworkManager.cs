using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviour
{
    public InputField emailinputField;
    public InputField passwordinputField;
    public static string playerdata;  

    public void Login()
    {
        StartCoroutine(DataPost());
    }

    IEnumerator DataPost()
    {      
        string url = "http://10.120.74.70:3001/auth/login";

        WWWForm form = new WWWForm();
        form.AddField("email", emailinputField.text);
        form.AddField("password", passwordinputField.text);
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

    public void SignUpOnOff(GameObject obj)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }
        else
            obj.SetActive(true);
    }
}