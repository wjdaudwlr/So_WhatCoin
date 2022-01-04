using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class RankingManager : MonoBehaviour
{
    public Text[] nameText;
    public Text[] moneyText;

    public List<RankingData> rankingDatas = new List<RankingData>();

    public void Start()
    {
        ResetRanking();
    }

    public void ResetRanking()
    {
        StartCoroutine(DataPost());

        
    }

    IEnumerator DataPost()
    {
        string url = "http://10.120.74.70:3001/rank";

        WWWForm form = new WWWForm();
     
        UnityWebRequest www = UnityWebRequest.Get(url);

        Debug.Log("1");
        yield return www.SendWebRequest(); // ������ �ö����� ��ٸ���



        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            
        }
        else if (www.downloadHandler.text != "false")
        {
            int i = 0;
            rankingDatas.Clear();
            Debug.Log(www.downloadHandler.text);

            JArray jArray = new JArray();
            jArray = JArray.Parse(www.downloadHandler.text);

            Debug.Log(jArray);
            foreach (JObject jo in jArray)
            {
                RankingData obj = new RankingData();

                obj.money = ulong.Parse(jo["playerMoney"].ToString());
                obj.name = jo["name"].Value<string>();

                rankingDatas.Add(obj);
            }
            
            foreach (var item in rankingDatas)
            {
                if (i <= 20)
                {
                    Debug.Log("name : " + item.name + "      money : " + item.money);
                    nameText[i].text = item.name;
                    moneyText[i].text = item.money.ToString();
                }
                i++;
            }

        }
        else if (www.downloadHandler.text == "false")
        {
            Debug.Log(false);
        }
    }





    [System.Serializable]
    public class RankingData
    {
        public string name;
        public ulong money;
    }
}
