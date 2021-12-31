using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

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
        TextAsset text = Resources.Load<TextAsset>("playerData");
        playerData = JsonConvert.DeserializeObject<PlayerData>(text.text);
    }

}


[System.Serializable]
public class PlayerData
{
    public string name;
    public ulong playerMoney;
    public ulong clickMoney = 25;
    public uint automatcIncome = 0;
    public uint typingSpeed = 1;
    public Dictionary<string, bool> itemDict;
    public Dictionary<string, int> coinDict;
}

