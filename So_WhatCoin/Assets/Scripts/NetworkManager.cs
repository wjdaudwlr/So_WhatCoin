using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    public InputField emailinputField;
    public InputField passwordinputField;

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

        yield return www.SendWebRequest(); // ������ �ö����� ��ٸ���

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text); // ���� ������ Ȯ��
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