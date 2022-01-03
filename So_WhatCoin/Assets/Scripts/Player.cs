using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq ;

public class Player : MonoBehaviour
{
    public PlayerData playerData = new PlayerData();

    [ContextMenu("To Json Data")]
    public void SavePlayerDataToJson()
    {
        string jsonData = JsonConvert.SerializeObject(playerData);
        string path = Path.Combine(Application.dataPath, "Resources\\playerData.json");
        Debug.Log(jsonData);
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("From Json Data")]
    public void LoadPlayerDataToJson()
    {
        //TextAsset text = Resources.Load<TextAsset>();

        JObject jobj = new JObject();

        jobj = JObject.Parse(NetworkManager.playerdata);

        foreach (KeyValuePair<string, JToken> pair in jobj)
        {
            playerData = JsonConvert.DeserializeObject<PlayerData>(pair.Value.ToString());
            Debug.Log(pair.Value);
        }
    }

}


[System.Serializable]
public class PlayerData
{
    public string name;
    public string email;
    public ulong playerMoney;
    public ulong clickMoney;
    public uint automatcIncome;
    public uint typingSpeed;
    public Dictionary<string, bool> itemDict;
    public Dictionary<string, int> coinDict;
}

